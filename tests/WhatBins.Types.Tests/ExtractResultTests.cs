namespace WhatBins.Types.Tests
{
    using System;
    using System.Collections.Generic;
    using Bogus;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using WhatBins.Types;
    using WhatBins.Types.Fakes;
    using Xunit;

    public class ExtractResultTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldSetState()
            {
                CollectionState collectionState = new CollectionStateFaker().Generate();

                ExtractResult result = new ExtractResult(collectionState);

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

                Action a = () => new ExtractResult(collectionState, null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldSetCollections()
            {
                CollectionState collectionState = new CollectionStateFaker().Generate();
                IEnumerable<Collection> collections = new CollectionFaker().Generate(3);

                ExtractResult result = new ExtractResult(collectionState, collections);

                using (new AssertionScope())
                {
                    result.State.Should().Be(collectionState);
                    result.Collections.Should().BeEquivalentTo(collections);
                }
            }
        }
    }
}
