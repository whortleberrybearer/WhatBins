namespace WhatBins.UnitTests
{
    using Bogus;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class LookupResultTests
    {
        public class ConstructorTests
        {
            private Faker faker = new Faker();

            [Fact]
            public void ShouldSetState()
            {
                CollectionState collectionState = this.faker.Random.Enum<CollectionState>();

                LookupResult result = new LookupResult(collectionState);

                result.State.Should().Be(collectionState);
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenCollectionsIsNull()
            {
                CollectionState collectionState = this.faker.Random.Enum<CollectionState>();

                Action a = () => new LookupResult(collectionState, null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldSetCollections()
            {
                CollectionState collectionState = this.faker.Random.Enum<CollectionState>();
                IEnumerable<Collection> collections = null;

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
