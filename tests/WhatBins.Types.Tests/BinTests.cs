namespace WhatBins.Types.Tests
{
    using FluentAssertions;
    using WhatBins.Types.Fakes;
    using Xunit;

    public class BinTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldSetBinColour()
            {
                BinColour binColour = new BinColourFaker().Generate();

                Bin result = new Bin(binColour);

                result.Colour.Should().Be(binColour);
            }
        }
    }
}
