namespace WhatBins.Extractors.ChorleyCouncil.UnitTests
{
    using FluentAssertions;
    using System;
    using Xunit;

    public class ParserTests
    {
        public class IsSupportedTests
        {
            private Parser sut = new Parser();

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlDocumentIsNull()
            {
                Action a = () => this.sut.IsSupported(null!);

                a.Should().Throw<ArgumentNullException>();
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
