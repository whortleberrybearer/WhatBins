namespace WhatBins.UnitTests
{
    using FluentAssertions;
    using WhatBins.Types;
    using WhatBins.UnitTests.Fakes;
    using Xunit;

    public class ExtractResultExtensionsTests
    {
        public class ToLookupResultTests
        {
            [Fact]
            public void ShouldSetClassProperties()
            {
                ExtractResult extractResult = new ExtractResultFaker().Generate();

                LookupResult result = extractResult.ToLookupResult();

                result.Should().BeEquivalentTo(extractResult);
            }
        }
    }
}
