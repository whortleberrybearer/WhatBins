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
            private BinCollectionsFinder sut;

            public LookupTests()
            {
                this.sut = new BinCollectionsFinder(new ICollectionExtractor[] { new CollectionExtractor() });
            }

            [Theory]
            [InlineData("SW1A 1AA")]
            public void ShouldReturnUnsupportedWhenPostCodeUnknown(
                string postCodeString)
            {
                PostCode postCode = new PostCode(postCodeString);

                LookupResult result = sut.Lookup(postCode);

                using (new AssertionScope())
                {
                    result.Should().NotBeNull();
                    result.State.Should().Be(CollectionState.Unsupported);
                }
            }

            [Theory]
            [InlineData("PR7 1DP")]
            public void ShouldReturnNoCollectionWhenNoBinsCollected(
                string postCodeString)
            {
                PostCode postCode = new PostCode(postCodeString);

                LookupResult result = sut.Lookup(postCode);

                using (new AssertionScope())
                {
                    result.Should().NotBeNull();
                    result.State.Should().Be(CollectionState.NoCollection);
                }
            }

            [Theory]
            [InlineData("PR7 6PJ")]
            public void ShouldReturnCollectionDetailsWhenAvailable(
                string postCodeString)
            {
                PostCode postCode = new PostCode(postCodeString);

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