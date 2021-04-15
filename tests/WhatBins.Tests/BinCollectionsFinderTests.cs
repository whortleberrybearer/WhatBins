namespace WhatBins.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bogus;
    using FluentAssertions;
    using FluentResults;
    using FluentResults.Extensions.FluentAssertions;
    using Moq;
    using WhatBins.Types;
    using WhatBins.Types.Fakes;
    using Xunit;

    public class BinCollectionsFinderTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenCollectionExtractorsIsNull()
            {
                Action a = () => new BinCollectionsFinder(null!);

                a.Should().Throw<ArgumentNullException>();
            }
        }

        public class LookupTests
        {
            private readonly MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            private readonly BinCollectionsFinder sut;
            private readonly List<Mock<ICollectionExtractor>> collectionExtractorMocks;

            public LookupTests()
            {
                this.collectionExtractorMocks = new List<Mock<ICollectionExtractor>>()
                {
                    this.mockRepository.Create<ICollectionExtractor>(),
                    this.mockRepository.Create<ICollectionExtractor>(),
                    this.mockRepository.Create<ICollectionExtractor>(),
                };
                this.sut = new BinCollectionsFinder(this.collectionExtractorMocks.Select(mock => mock.Object));
            }

            public static IEnumerable<object[]> SupportedCollectionResults()
            {
                return new List<object[]>()
                {
                    new object[] { Collection.NoCollection },
                    new object[] { new Collection(new CollectionDayFaker().Generate(new Faker().Random.Number(1, 5))) },
                };
            }

            [Fact]
            public void ShouldReturnUnsupportedWhenPostCodeCanNotBeExtracted()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                foreach (Mock<ICollectionExtractor> collectionExtractorMock in this.collectionExtractorMocks)
                {
                    collectionExtractorMock
                        .Setup(extractor => extractor.CanExtract(postCode))
                        .Returns(Result.Ok(false));
                }

                Result<Collection> result = this.sut.Lookup(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(Collection.Unsupported);
            }

            [Fact]
            public void ShouldCheckAllExtractorsWhenCanExtractButStillUnsupported()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                foreach (Mock<ICollectionExtractor> collectionExtractorMock in this.collectionExtractorMocks)
                {
                    collectionExtractorMock
                        .Setup(extractor => extractor.CanExtract(postCode))
                        .Returns(Result.Ok(true));

                    collectionExtractorMock
                        .Setup(extractor => extractor.Extract(postCode))
                        .Returns(Result.Ok(Collection.Unsupported));
                }

                Result<Collection> result = this.sut.Lookup(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(Collection.Unsupported);

                this.mockRepository.VerifyAll();
            }

            [Theory]
            [MemberData(nameof(SupportedCollectionResults))]
            public void ShouldReturnResultWhenSupportedCollectionFound(Collection collection)
            {
                PostCode postCode = new PostCodeFaker().Generate();

                foreach (Mock<ICollectionExtractor> collectionExtractorMock in this.collectionExtractorMocks)
                {
                    collectionExtractorMock
                        .Setup(extractor => extractor.CanExtract(postCode))
                        .Returns(Result.Ok(true));
                }

                // Once nothing has been found for the first check, the seconds once should be returned.
                // The third should never be called as a result was found with the second.
                this.collectionExtractorMocks[0]
                    .Setup(extractor => extractor.Extract(postCode))
                    .Returns(Result.Ok(Collection.Unsupported));
                this.collectionExtractorMocks[1]
                    .Setup(extractor => extractor.Extract(postCode))
                    .Returns(Result.Ok(collection));

                Result<Collection> result = this.sut.Lookup(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(collection);
            }

            [Fact]
            public void ShouldContinueCheckingWhenLookupFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                foreach (Mock<ICollectionExtractor> collectionExtractorMock in this.collectionExtractorMocks)
                {
                    collectionExtractorMock
                        .Setup(extractor => extractor.CanExtract(postCode))
                        .Returns(Result.Ok(true));

                    collectionExtractorMock
                        .Setup(extractor => extractor.Extract(postCode))
                        .Returns(Result.Fail<Collection>(string.Empty));
                }

                this.sut.Lookup(postCode);

                this.mockRepository.VerifyAll();
            }

            [Fact]
            public void ShouldContinueCheckingWhenCanExtractFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                foreach (Mock<ICollectionExtractor> collectionExtractorMock in this.collectionExtractorMocks)
                {
                    collectionExtractorMock
                        .Setup(extractor => extractor.CanExtract(postCode))
                        .Returns(Result.Fail<bool>(string.Empty));
                }

                this.sut.Lookup(postCode);

                this.mockRepository.VerifyAll();
            }
        }
    }
}
