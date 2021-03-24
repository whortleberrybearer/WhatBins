namespace WhatBins.UnitTests
{
    using FluentAssertions;
    using FluentAssertions.Execution;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class LookupResultTests
    {
        public class ConstructorTests
        {
            [Theory]
            [AutoMoqDomainData]
            public void ShouldSetState(
                CollectionState collectionState)
            {
                LookupResult result = new LookupResult(collectionState);

                result.State.Should().Be(collectionState);
            }

            [Theory]
            [AutoMoqDomainData]
            public void ShouldThrowArgumentNullExceptionWhenCollectionsIsNull(
                CollectionState collectionState)
            {
                Action a = () => new LookupResult(collectionState, null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [AutoMoqDomainData]
            public void ShouldSetCollections(
                CollectionState collectionState,
                IEnumerable<Collection> collections)
            {
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
