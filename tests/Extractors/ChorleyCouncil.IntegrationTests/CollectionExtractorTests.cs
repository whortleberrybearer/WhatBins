namespace WhatBins.Extractors.ChorleyCouncil.IntegrationTests
{
    using FluentAssertions;
    using FluentAssertions.Execution;
    using FluentResults;
    using FluentResults.Extensions.FluentAssertions;
    using WhatBins.Extractors.ChorleyCouncil;
    using WhatBins.Types;
    using Xunit;

    // These tests call the live sites, so tests that expect data to be returned can only check there is data,
    // not what the content is.
    public class CollectionExtractorTests
    {
        public class LookupTests
        {
            private readonly CollectionExtractor sut = new CollectionExtractor();

            [Theory]
            [InlineData("SW1A 1AA")]
            public void ShouldReturnUnsupportedWhenPostCodeUnknown(
                string postCodeString)
            {
                PostCode postCode = new PostCode(postCodeString);

                Result<Collection> result = this.sut.Extract(postCode);

                // TODO: Could be a value object compare?
                result.Should().BeSuccess().And.Subject.Value.State.Should().Be(CollectionState.Unsupported);
            }

            [Theory]
            [InlineData("PR7 1DP")]
            public void ShouldReturnNoCollectionWhenNoBinsCollected(
                string postCodeString)
            {
                PostCode postCode = new PostCode(postCodeString);

                Result<Collection> result = this.sut.Extract(postCode);

                // TODO: Could be a value object compare?
                result.Should().BeSuccess().And.Subject.Value.State.Should().Be(CollectionState.NoCollection);
            }

            [Theory]
            [InlineData("PR7 6PJ")]
            public void ShouldReturnCollectionDetailsWhenAvailable(
                string postCodeString)
            {
                PostCode postCode = new PostCode(postCodeString);

                Result<Collection> result = this.sut.Extract(postCode);

                // TODO: Could be a value object compare?
                result.Should().BeSuccess();

                using (new AssertionScope())
                {
                    result.Value.State.Should().Be(CollectionState.Collection);
                    result.Value.CollectionDays.Should().NotBeEmpty();
                }
            }
        }
    }
}
