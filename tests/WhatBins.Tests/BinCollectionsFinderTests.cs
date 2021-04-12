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
            [InlineData(CollectionState.Collection)]
            [InlineData(CollectionState.NoCollection)]
            public void ShouldReturnResultWhenSupportedCollectionFound(CollectionState collectionState)
            {
                PostCode postCode = new PostCodeFaker().Generate();
                IEnumerable<CollectionDay> collectionDays = new CollectionDayFaker().Generate(new Faker().Random.Number(1, 5));

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
                    .Returns(Result.Ok(new Collection(collectionDays)));

                Result<Collection> result = this.sut.Lookup(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(new Collection(collectionDays));
            }
        }
    }
}
