namespace WhatBins.Extractors.ChorleyCouncil.UnitTests
{
    using FluentAssertions;
    using HtmlAgilityPack;
    using System;
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
            private Parser sut = new Parser();

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlDocumentIsNull()
            {
                Action a = () => this.sut.IsSupported(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldReturnTrueWhenDocumentContainsExpectedText()
            {
                bool result = this.sut.IsSupported(collectionsHtmlDocument);

                result.Should().BeTrue();
            }

            [Fact]
            public void ShouldReturnFalseWhenDocumentDoesNotContainsExpectedText()
            {
                bool result = this.sut.IsSupported(collectionsHtmlDocument);

                result.Should().BeFalse();
            }
        }

        public class DoesDoCollectionsTests
        {
            private Parser sut = new Parser();

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlDocumentIsNull()
            {
                Action a = () => this.sut.DoesDoCollections(null!);

                a.Should().Throw<ArgumentNullException>();
            }
        }

        public class ExtractRequestStateTests
        {
            private Parser sut = new Parser();

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlDocumentIsNull()
            {
                Action a = () => this.sut.ExtractRequestState(null!);

                a.Should().Throw<ArgumentNullException>();
            }
        }

        public class ExtractUprnTests
        {
            private Parser sut = new Parser();

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlDocumentIsNull()
            {
                Action a = () => this.sut.ExtractUprn(null!);

                a.Should().Throw<ArgumentNullException>();
            }
        }

        public class ExtractCollectionsTests
        {
            private Parser sut = new Parser();

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlDocumentIsNull()
            {
                Action a = () => this.sut.ExtractCollections(null!);

                a.Should().Throw<ArgumentNullException>();
            }
        }
    }
}
