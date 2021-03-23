namespace WhatBins.IntegrationTests
{
    using System;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using Xunit;

    public enum CollectionState
    {
        Unsupported,
        NoCollection,
        Collection
    }

    public class LookupResult
    {
        public CollectionState State { get; set; }

        public IEnumerable<Collection> Collections { get; set; }
    }

    public class Collection
    {

    }

    public class something
    {
        public LookupResult Lookup(PostCode postCode)
        {
            throw new NotImplementedException();
        }
    }

    public struct PostCode
    {
        private string value;;

        public PostCode(string value) 
            : this()
        {
            this.value = value;
        }
    }

    // These tests call the live sites, so tests that expect data to be returned can only check there is data,
    // not what the content is.
    public class UnitTest1
    {
        [Theory]
        [AutoMoqDomainData]
        public void ShouldReturnUnsupportedWhenPostCodeUnknown(
            something sut)
        {
            PostCode postCode = new PostCode("SW1A 1AA");

            LookupResult result = sut.Lookup(postCode);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.State.Should().Be(CollectionState.Unsupported);
            }
        }

        [Theory]
        [AutoMoqDomainData]
        public void ShouldReturnNoCollectionWhenNoBinsCollected(
            something sut)
        {
            PostCode postCode = new PostCode("PR7 1DP");

            LookupResult result = sut.Lookup(postCode);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.State.Should().Be(CollectionState.NoCollection);
            }
        }

        [Theory]
        [AutoMoqDomainData]
        public void ShouldReturnCollectionDetailsWhenAvailable(
            something sut)
        {
            PostCode postCode = new PostCode("PR7 6PJ");

            LookupResult result = sut.Lookup(postCode);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.State.Should().Be(CollectionState.Collection);
                result.Collections.Should().NotBeEmpty();
            }
        }
    }
}
