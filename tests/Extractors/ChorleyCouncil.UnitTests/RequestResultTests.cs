namespace WhatBins.Extractors.ChorleyCouncil.UnitTests
{
    using System;
    using Bogus;
    using FluentAssertions;
    using Xunit;

    public class RequestResultTests
    {
        public class SucceededTests
        {
            [Fact]
            public void ShouldSetSucceededToTrue()
            {
                Faker faker = new Faker();
                string html = faker.Random.String();

                RequestResult result = RequestResult.Succeeded(html);

                result.Success.Should().BeTrue();
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenHtmlIsNull()
            {
                Action a = () => RequestResult.Succeeded(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldSetHtmlDocumentToHtml()
            {
                Faker faker = new Faker();
                string html = faker.Random.String();

                RequestResult result = RequestResult.Succeeded(html);

                result.HtmlDocument.Should().NotBeNull();
                result.HtmlDocument!.ParsedText.Should().Be(html);
            }
        }

        public class FailedTests
        {
            [Fact]
            public void ShouldSetSucceededToFalse()
            {
                RequestResult result = RequestResult.Failed;

                result.Success.Should().BeFalse();
            }

            [Fact]
            public void ShouldSetHtmlDocumentToNull()
            {
                RequestResult result = RequestResult.Failed;

                result.HtmlDocument.Should().BeNull();
            }
        }
    }
}
