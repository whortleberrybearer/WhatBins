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

    public class CollectionDayTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldSetStateAndBins()
            {
                LocalDate date = LocalDate.FromDateTime(new Faker().Date.Soon());
                IEnumerable<Bin> bins = new BinFaker().Generate(3);

                CollectionDay result = new CollectionDay(date, bins);

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

                Action a = () => new CollectionDay(date, null!);

                a.Should().Throw<ArgumentNullException>();
            }
        }
    }
}
