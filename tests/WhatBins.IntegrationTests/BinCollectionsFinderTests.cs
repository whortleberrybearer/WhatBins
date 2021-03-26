namespace WhatBins.IntegrationTests
{
    using FluentAssertions;
    using FluentAssertions.Execution;
    using WhatBins.Extractors.ChorleyCouncil;
    using WhatBins.Types;
    using Xunit;

    // These tests call the live sites, so tests that expect data to be returned can only check there is data,
    // not what the content is.
    public class BinCollectionsFinderTests
    {
        public class LookupTests
        {
            [Theory]
            [AutoMoqDomainData]
            public void ShouldReturnUnsupportedWhenPostCodeUnknown(
                int i
                //BinCollectionsFinder sut)
                )
            {
                BinCollectionsFinder sut = new BinCollectionsFinder(new ICollectionExtractor[] { new CollectionExtractor() });

                PostCode postCode = new PostCode("SW1A 1AA");

                LookupResult result = sut.Lookup(postCode);

                using (new AssertionScope())
                {
                    result.Should().NotBeNull();
                    result.State.Should().Be(CollectionState.Unsupported);
                }
            }

            [Theory]
            [AutoMoqDomainData]
            public void ShouldReturnNoCollectionWhenNoBinsCollected(
                BinCollectionsFinder sut)
            {
                PostCode postCode = new PostCode("PR7 1DP");

                LookupResult result = sut.Lookup(postCode);

                using (new AssertionScope())
                {
                    result.Should().NotBeNull();
                    result.State.Should().Be(CollectionState.NoCollection);
                }
            }

            [Theory]
            [AutoMoqDomainData]
            public void ShouldReturnCollectionDetailsWhenAvailable(
                BinCollectionsFinder sut)
            {
                PostCode postCode = new PostCode("PR7 6PJ");

                LookupResult result = sut.Lookup(postCode);

                using (new AssertionScope())
                {
                    result.Should().NotBeNull();
                    result.State.Should().Be(CollectionState.Collection);
                    result.Collections.Should().NotBeEmpty();
                }
            }
        }
    }
}
