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
            public void ShouldThrowArgumentNullExceptionWhenCollectionsIsNull()
            {
                Action a = () => new CollectionExtraction(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldSetCollections()
            {
                IEnumerable<Collection> collections = new CollectionFaker().Generate(3);

                CollectionExtraction result = new CollectionExtraction(collections);

                result.Collections.Should().BeEquivalentTo(collections);
            }
        }
    }
}
