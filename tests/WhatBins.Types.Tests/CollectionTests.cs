namespace WhatBins.Types.Tests
{
    using System;
    using System.Collections.Generic;
    using Bogus;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using NodaTime;
    using WhatBins.Types;
    using WhatBins.Types.Fakes;
    using Xunit;

    public class CollectionTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldSetStateAndBins()
            {
                LocalDate date = LocalDate.FromDateTime(new Faker().Date.Soon());
                IEnumerable<Bin> bins = new BinFaker().Generate(3);

                Collection result = new Collection(date, bins);

                using (new AssertionScope())
                {
                    result.Date.Should().Be(date);
                    result.Bins.Should().BeEquivalentTo(bins);
                }
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenCollectionsIsNull()
            {
                LocalDate date = LocalDate.FromDateTime(new Faker().Date.Soon());

                Action a = () => new Collection(date, null!);

                a.Should().Throw<ArgumentNullException>();
            }
        }
    }
}
