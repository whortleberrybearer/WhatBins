namespace WhatBins.Extractors.ChorleyCouncil.UnitTests
{
    using FluentAssertions;
    using HtmlAgilityPack;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using WhatBins.Types;
    using Xunit;

    public class ParserTests
    {
        private static readonly HtmlDocument notSupportedHtmlDocument;
        private static readonly HtmlDocument noCollectionsHtmlDocument;
        private static readonly HtmlDocument collectionsHtmlDocument;

        static ParserTests()
        {
            notSupportedHtmlDocument = new HtmlDocument();
            notSupportedHtmlDocument.Load("RequestResponses//NotSupported.html");

            noCollectionsHtmlDocument = new HtmlDocument();
            noCollectionsHtmlDocument.Load("RequestResponses//NoCollections.html");

            collectionsHtmlDocument = new HtmlDocument();
            collectionsHtmlDocument.Load("RequestResponses//Collections.html");
        }

        public class IsSupportedTests
        {
            private readonly Parser sut = new Parser();

            public static IEnumerable<object[]> SupportedHtmlDocuments()
            {
                return new List<object[]>()
                {
                    new object[] { noCollectionsHtmlDocument },
                    new object[] { collectionsHtmlDocument },
                };
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlDocumentIsNull()
            {
                Action a = () => this.sut.IsSupported(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldReturnFalseWhenDocumentContainsExpectedText()
            {
                bool result = this.sut.IsSupported(notSupportedHtmlDocument);

                result.Should().BeFalse();
            }

            [Theory]
            [MemberData(nameof(SupportedHtmlDocuments))]
            public void ShouldReturnTrueWhenDocumentDoesNotContainsExpectedText(HtmlDocument htmlDocument)
            {
                bool result = this.sut.IsSupported(htmlDocument);

                result.Should().BeTrue();
            }
        }

        public class DoesDoCollectionsTests
        {
            private readonly Parser sut = new Parser();

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlDocumentIsNull()
            {
                Action a = () => this.sut.DoesDoCollections(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldReturnFalseWhenDocumentContainsExpectedText()
            {
                bool result = this.sut.DoesDoCollections(noCollectionsHtmlDocument);

                result.Should().BeFalse();
            }

            [Fact]
            public void ShouldReturnTrueWhenDocumentDoesNotContainsExpectedText()
            {
                bool result = this.sut.DoesDoCollections(collectionsHtmlDocument);

                result.Should().BeTrue();
            }
        }

        public class ExtractRequestStateTests
        {
            private readonly Parser sut = new Parser();

            //public static IEnumerable<object[]> HtmlDocuments()
            //{
            //    return new List<object[]>()
            //    {
            //        new object[] { collectionsPageHtmlDocument },
            //        new object[] { postCodeLookupHtmlDocument },
            //        new object[] { uprnLookupHtmlDocument },
            //        new object[] { collectionsLookupHtmlDocument }
            //    };
            //}

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlDocumentIsNull()
            {
                Action a = () => this.sut.ExtractRequestState(null!);

                a.Should().Throw<ArgumentNullException>();
            }
        }

        public class ExtractUprnTests
        {
            private readonly Parser sut = new Parser();

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlDocumentIsNull()
            {
                Action a = () => this.sut.ExtractUprn(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldExtractUprn()
            {
                Uprn result = this.sut.ExtractUprn(uprnLookupHtmlDocument);

                result.Should().Be(new Uprn(""));
            }
        }

        public class ExtractCollectionsTests
        {
            private readonly Parser sut = new Parser();

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlDocumentIsNull()
            {
                Action a = () => this.sut.ExtractCollections(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldExtractCollections()
            {
                IEnumerable<Collection> result = this.sut.ExtractCollections(collectionsHtmlDocument);

                var expectedResult = new Collection[]
                {
                    new Collection(new LocalDate(2021, 3, 23), new Bin[] { new Bin(BinColour.Blue) }),
                    new Collection(new LocalDate(2021, 3, 30), new Bin[] { new Bin(BinColour.Green) }),
                    new Collection(new LocalDate(2021, 4, 6), new Bin[] { new Bin(BinColour.Blue), new Bin(BinColour.Brown) }),
                    new Collection(new LocalDate(2021, 4, 13), new Bin[] { new Bin(BinColour.Green) }),
                    new Collection(new LocalDate(2021, 4, 20), new Bin[] { new Bin(BinColour.Blue) }),
                    new Collection(new LocalDate(2021, 4, 27), new Bin[] { new Bin(BinColour.Green) }),
                    new Collection(new LocalDate(2021, 5, 4), new Bin[] { new Bin(BinColour.Blue), new Bin(BinColour.Brown) }),
                    new Collection(new LocalDate(2021, 5, 11), new Bin[] { new Bin(BinColour.Green) }),
                    new Collection(new LocalDate(2021, 5, 18), new Bin[] { new Bin(BinColour.Blue) })
                };

                result.Should().BeEquivalentTo(expectedResult);
            }
        }
    }
}
