//namespace WhatBins.UnitTests
//{
//    using FluentAssertions;
//    using FluentAssertions.Execution;
//    using Xunit;

//    public class BinCollectionsFinderTests
//    {
//        public class LookupTests
//        {
//            [Theory]
//            [AutoMoqDomainData]
//            public void ShouldReturnUnsupportedWhenPostCodeUnknown(
//                BinCollectionsFinder sut)
//            {
//                PostCode postCode = new PostCode("SW1A 1AA");

//                LookupResult result = sut.Lookup(postCode);

//                using (new AssertionScope())
//                {
//                    result.Should().NotBeNull();
//                    result.State.Should().Be(CollectionState.Unsupported);
//                }
//            }
//        }
//    }
//}
