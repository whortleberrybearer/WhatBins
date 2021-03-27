namespace WhatBins.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Bogus;
    using FluentAssertions;
    using Moq;
    using WhatBins.Types;
    using WhatBins.Tests.Fakes;
    using Xunit;

    public class BinCollectionsFinderTests
    {
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
                    this.mockRepository.Create<ICollectionExtractor>()
                };
                this.sut = new BinCollectionsFinder(this.collectionExtractorMocks.Select(mock => mock.Object));
            }

            [Fact]
            public void ShouldReturnUnsupportedWhenPostCodeCanNotBeExtracted()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                foreach (Mock<ICollectionExtractor> collectionExtractorMock in collectionExtractorMocks)
                {
                    collectionExtractorMock
                        .Setup(extractor => extractor.CanExtract(postCode))
                        .Returns(false);
                }

                LookupResult result = this.sut.Lookup(postCode);

                result.Should().BeEquivalentTo(new LookupResult(CollectionState.Unsupported));
            }

            [Fact]
            public void ShouldCheckAllExtractorsWhenCanExtractButStillUnsupported()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                foreach (Mock<ICollectionExtractor> collectionExtractorMock in collectionExtractorMocks)
                {
                    collectionExtractorMock
                        .Setup(extractor => extractor.CanExtract(postCode))
                        .Returns(true);

                    collectionExtractorMock
                        .Setup(extractor => extractor.Extract(postCode))
                        .Returns(new ExtractResult(CollectionState.Unsupported));
                }

                LookupResult result = this.sut.Lookup(postCode);

                result.Should().BeEquivalentTo(new LookupResult(CollectionState.Unsupported));

                this.mockRepository.VerifyAll();
            }

            [Theory]
            [InlineData(CollectionState.Collection)]
            [InlineData(CollectionState.NoCollection)]
            public void ShouldReturnResultWhenSupportedCollectionFound(
                CollectionState collectionState)
            {
                PostCode postCode = new PostCodeFaker().Generate();
                IEnumerable<Collection> collections = new CollectionFaker().Generate(new Faker().Random.Number(1, 5));

                foreach (Mock<ICollectionExtractor> collectionExtractorMock in collectionExtractorMocks)
                {
                    collectionExtractorMock
                        .Setup(extractor => extractor.CanExtract(postCode))
                        .Returns(true);
                }

                // Once nothing has been found for the first check, the seconds once should be returned.
                // The third should never be called as a result was found with the second.
                this.collectionExtractorMocks[0]
                    .Setup(extractor => extractor.Extract(postCode))
                    .Returns(new ExtractResult(CollectionState.Unsupported));
                this.collectionExtractorMocks[1]
                    .Setup(extractor => extractor.Extract(postCode))
                    .Returns(new ExtractResult(collectionState, collections));

                LookupResult result = this.sut.Lookup(postCode);

                result.Should().BeEquivalentTo(new LookupResult(collectionState, collections));
            }
        }
    }
}
