namespace WhatBins.Types.Tests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using WhatBins.Types;
    using WhatBins.Types.Fakes;
    using Xunit;

    public class CollectionTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenCollectionsIsNull()
            {
                Action a = () => new Collection(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldSetCollections()
            {
                IEnumerable<CollectionDay> collectionDays = new CollectionDayFaker().Generate(3);

                Collection result = new Collection(collectionDays);

                result.CollectionDays.Should().BeEquivalentTo(collectionDays);
            }
        }
    }
}
