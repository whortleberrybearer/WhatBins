﻿namespace WhatBins.Extractors.ChorleyCouncil.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bogus;
    using FluentAssertions;
    using FluentResults;
    using FluentResults.Extensions.FluentAssertions;
    using Moq;
    using WhatBins.Extractors.ChorleyCouncil.UnitTests.Fakes;
    using WhatBins.Types;
    using WhatBins.Types.Fakes;
    using Xunit;

    public class CollectionExtractorTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenRequestorNull()
            {
                Mock<IParser> parserMock = new Mock<IParser>();

                Action a = () => new CollectionExtractor(null!, parserMock.Object);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenParserNull()
            {
                Mock<IRequestor> requestorMock = new Mock<IRequestor>();

                Action a = () => new CollectionExtractor(requestorMock.Object, null!);

                a.Should().Throw<ArgumentNullException>();
            }
        }

        public class CanExtractTests
        {
            private readonly MockRepository mockRepository;
            private readonly Mock<IRequestor> requestorMock;
            private readonly Mock<IParser> parserMock;
            private readonly CollectionExtractor sut;

            public CanExtractTests()
            {
                this.mockRepository = new MockRepository(MockBehavior.Strict);
                this.requestorMock = this.mockRepository.Create<IRequestor>();
                this.parserMock = this.mockRepository.Create<IParser>();
                this.sut = new CollectionExtractor(this.requestorMock.Object, this.parserMock.Object);
            }

            [Theory]
            [InlineData("PR6 1DD")]
            [InlineData("PR7 1DD")]
            [InlineData("PR25 1DD")]
            [InlineData("PR26 1DD")]
            [InlineData("BL6 1DD")]
            [InlineData("L40 1DD")]
            public void ShouldReturnTrueWhenPostCodeInArea(
                string postCodeString)
            {
                PostCode postCode = new PostCode(postCodeString);

                Result<bool> result = this.sut.CanExtract(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeTrue();
            }

            [Theory]
            [InlineData("PR1 1RR")]
            [InlineData("IG4 3RR")]
            public void ShouldReturnFalseWhenPostCodeInArea(
                string postCodeString)
            {
                PostCode postCode = new PostCode(postCodeString);

                Result<bool> result = this.sut.CanExtract(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeFalse();
            }
        }

        public class ExtractTests
        {
            private readonly Faker faker;
            private readonly MockRepository mockRepository;
            private readonly Mock<IRequestor> requestorMock;
            private readonly Mock<IParser> parserMock;
            private readonly CollectionExtractor sut;

            public ExtractTests()
            {
                this.faker = new Faker();
                this.mockRepository = new MockRepository(MockBehavior.Strict);
                this.requestorMock = this.mockRepository.Create<IRequestor>();
                this.parserMock = this.mockRepository.Create<IParser>();
                this.sut = new CollectionExtractor(this.requestorMock.Object, this.parserMock.Object);
            }

            [Fact]
            public void ShouldReturnUnsupportedWhenPageRequestFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestPageMocks(RequestResult.Failed);

                Result<ExtractResult> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(new ExtractResult(CollectionState.Unsupported));
            }

            [Fact]
            public void ShouldReturnUnsupportedWhenPostCodeLookupRequestFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestPostCodeLookupMocks(postCode, RequestResult.Failed);

                Result<ExtractResult> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(new ExtractResult(CollectionState.Unsupported));
            }

            [Fact]
            public void ShouldReturnUnsupportedWhenPostCodeNotSupported()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestUprnLookupMocks(postCode, false, RequestResult.Failed);

                Result<ExtractResult> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(new ExtractResult(CollectionState.Unsupported));
            }

            [Fact]
            public void ShouldReturnUnsupportedWhenUprnLookupRequestFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestUprnLookupMocks(postCode, true, RequestResult.Failed);

                Result<ExtractResult> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(new ExtractResult(CollectionState.Unsupported));
            }

            [Fact]
            public void ShouldReturnUnsupportedWhenCollectionsLookupRequestFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestCollectionLookupsMocks(postCode, RequestResult.Failed);

                Result<ExtractResult> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(new ExtractResult(CollectionState.Unsupported));
            }

            [Fact]
            public void ShouldReturnNoCollectionsWhenNoCollectionsReported()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupExtractCollectionsMocks(postCode, false, Enumerable.Empty<Collection>());

                Result<ExtractResult> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(new ExtractResult(CollectionState.NoCollection));
            }

            [Fact]
            public void ShouldReturnNoCollectionsWhenNoCollectionsExtracted()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupExtractCollectionsMocks(postCode, true, Enumerable.Empty<Collection>());

                Result<ExtractResult> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(new ExtractResult(CollectionState.NoCollection));
            }

            [Fact]
            public void ShouldReturnCollectionsWhenCollectionsExtracted()
            {
                PostCode postCode = new PostCodeFaker().Generate();
                IEnumerable<Collection> collections = new CollectionFaker().Generate(3);

                this.SetupExtractCollectionsMocks(postCode, true, collections);

                Result<ExtractResult> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(new ExtractResult(CollectionState.Collection, collections));
            }

            private void SetupRequestPageMocks(RequestResult result)
            {
                this.requestorMock
                    .Setup(requestor => requestor.RequestCollectionsPage())
                    .Returns(result);
            }

            private void SetupRequestPostCodeLookupMocks(PostCode postCode, RequestResult result)
            {
                string html = this.faker.Random.String();
                RequestState requestState = new RequestStateFaker().Generate();
                RequestResult previousRequestResult = RequestResult.Succeeded(html);

                this.SetupRequestPageMocks(previousRequestResult);

                this.parserMock
                    .Setup(parser => parser.ExtractRequestState(previousRequestResult.HtmlDocument!))
                    .Returns(requestState);

                this.requestorMock
                    .Setup(requestor => requestor.RequestPostCodeLookup(postCode, requestState))
                    .Returns(result);
            }

            private void SetupRequestUprnLookupMocks(PostCode postCode, bool isSupported, RequestResult result)
            {
                string html = this.faker.Random.String();
                Uprn uprn = new UprnFaker().Generate();
                RequestState requestState = new RequestStateFaker().Generate();
                RequestResult previousRequestResult = RequestResult.Succeeded(html);

                this.SetupRequestPostCodeLookupMocks(postCode, previousRequestResult);

                this.parserMock
                    .Setup(parser => parser.IsWithinBoundary(previousRequestResult.HtmlDocument!))
                    .Returns(isSupported);
                this.parserMock
                    .Setup(parser => parser.ExtractRequestState(previousRequestResult.HtmlDocument!))
                    .Returns(requestState);
                this.parserMock
                    .Setup(parser => parser.ExtractUprn(previousRequestResult.HtmlDocument!))
                    .Returns(uprn);

                this.requestorMock
                    .Setup(requestor => requestor.RequestUprnLookup(uprn, requestState))
                    .Returns(result);
            }

            private void SetupRequestCollectionLookupsMocks(PostCode postCode, RequestResult result)
            {
                string html = this.faker.Random.String();
                RequestState requestState = new RequestStateFaker().Generate();
                RequestResult previousRequestResult = RequestResult.Succeeded(html);

                this.SetupRequestUprnLookupMocks(postCode, true, previousRequestResult);

                this.parserMock
                    .Setup(parser => parser.ExtractRequestState(previousRequestResult.HtmlDocument!))
                    .Returns(requestState);

                this.requestorMock
                    .Setup(requestor => requestor.RequestCollectionsLookup(requestState))
                    .Returns(result);
            }

            private void SetupExtractCollectionsMocks(PostCode postCode, bool doesDoCollections, IEnumerable<Collection> collections)
            {
                string html = this.faker.Random.String();
                RequestResult previousRequestResult = RequestResult.Succeeded(html);

                this.SetupRequestCollectionLookupsMocks(postCode, previousRequestResult);

                this.parserMock
                    .Setup(parser => parser.DoesCollectAtAddress(previousRequestResult.HtmlDocument!))
                    .Returns(doesDoCollections);
                this.parserMock
                    .Setup(parser => parser.ExtractCollections(previousRequestResult.HtmlDocument!))
                    .Returns(collections);
            }
        }
    }
}
