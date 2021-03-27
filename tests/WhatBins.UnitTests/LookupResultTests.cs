namespace WhatBins.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Bogus;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using WhatBins.Types;
    using WhatBins.UnitTests.Fakes;
    using Xunit;

    public class LookupResultTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldSetState()
            {
                CollectionState collectionState = new CollectionStateFaker().Generate();

                LookupResult result = new LookupResult(collectionState);

                using (new AssertionScope())
                {
                    result.State.Should().Be(collectionState);
                    result.Collections.Should().BeEmpty();
                }
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenCollectionsIsNull()
            {
                CollectionState collectionState = new CollectionStateFaker().Generate();

                Action a = () => new LookupResult(collectionState, null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldSetCollections()
            {
                CollectionState collectionState = new CollectionStateFaker().Generate();
                IEnumerable<Collection> collections = new CollectionFaker().Generate(new Faker().Random.Number(1, 5));

                LookupResult result = new LookupResult(collectionState, collections);

                using (new AssertionScope())
                {
                    result.State.Should().Be(collectionState);
                    result.Collections.Should().BeEquivalentTo(collections);
                }
            }
        }
    }
}
