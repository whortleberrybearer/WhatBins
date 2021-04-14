namespace WhatBins.Extractors.ChorleyCouncil.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using FluentResults;
    using FluentResults.Extensions.FluentAssertions;
    using HtmlAgilityPack;
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
            private readonly MockRepository mockRepository;
            private readonly Mock<IRequestor> requestorMock;
            private readonly Mock<IParser> parserMock;
            private readonly CollectionExtractor sut;

            public ExtractTests()
            {
                this.mockRepository = new MockRepository(MockBehavior.Strict);
                this.requestorMock = this.mockRepository.Create<IRequestor>();
                this.parserMock = this.mockRepository.Create<IParser>();
                this.sut = new CollectionExtractor(this.requestorMock.Object, this.parserMock.Object);
            }

            [Fact]
            public void ShouldReturnFailureWhenPageRequestFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestPageMocks(Result.Fail<HtmlDocument>(string.Empty));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnFailureWhenPageResponseHasNoState()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestPostCodeLookupMocks(postCode, Result.Fail<HtmlDocument>(string.Empty), hasRequestState: false);

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnFailureWhenPostCodeLookupRequestFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestPostCodeLookupMocks(postCode, Result.Fail<HtmlDocument>(string.Empty));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnFailureWhenIsSupportedParseFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestUprnLookupMocks(postCode, isSupportedResult: Result.Fail<bool>(string.Empty));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnFailureWhenUprnParseFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestUprnLookupMocks(postCode, extractUprnResult: Result.Fail<Uprn>(string.Empty));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnUnsupportedWhenPostCodeNotSupported()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestUprnLookupMocks(postCode, isSupportedResult: Result.Ok(false));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.HaveValue(Collection.Unsupported);
            }

            [Fact]
            public void ShouldReturnFailureWhenPostCodePageResponseHasNoState()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestUprnLookupMocks(postCode, hasRequestState: false);

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnFailureWhenUprnLookupRequestFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestUprnLookupMocks(postCode, result: Result.Fail<HtmlDocument>(string.Empty));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnFailureWhenUrpnPageResponseHasNoState()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestCollectionLookupsMocks(postCode, Result.Fail<HtmlDocument>(string.Empty), hasRequestState: false);

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnFailureWhenCollectionsLookupRequestFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupRequestCollectionLookupsMocks(postCode, Result.Fail<HtmlDocument>(string.Empty));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnNoCollectionsWhenNoCollectionsReported()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupExtractCollectionsMocks(postCode, Result.Ok(false));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.HaveValue(Collection.NoCollection);
            }

            [Fact]
            public void ShouldReturnNoCollectionsWhenNoCollectionsExtracted()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupExtractCollectionsMocks(postCode, collectionExtractionsResult: Result.Ok(Enumerable.Empty<CollectionDay>()));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.HaveValue(Collection.NoCollection);
            }

            [Fact]
            public void ShouldReturnCollectionsWhenCollectionsExtracted()
            {
                PostCode postCode = new PostCodeFaker().Generate();
                IEnumerable<CollectionDay> collectionDays = new CollectionDayFaker().Generate(3);

                this.SetupExtractCollectionsMocks(postCode, collectionExtractionsResult: Result.Ok(collectionDays));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeSuccess().And.Subject.Value.Should().BeEquivalentTo(new Collection(collectionDays));
            }

            [Fact]
            public void ShouldReturnFailureWhenDoesDoCollectionExtractionsFail()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupExtractCollectionsMocks(postCode, doesDoCollectionsResult: Result.Fail<bool>(string.Empty));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnFailureWhenCollectionExtractionsFail()
            {
                PostCode postCode = new PostCodeFaker().Generate();

                this.SetupExtractCollectionsMocks(
                    postCode,
                    collectionExtractionsResult: Result.Fail<IEnumerable<CollectionDay>>(string.Empty));

                Result<Collection> result = this.sut.Extract(postCode);

                result.Should().BeFailure();
            }

            private static Result<RequestState> CreateRequestStateResult(bool isFailure)
            {
                if (isFailure)
                {
                    return Result.Fail<RequestState>(string.Empty);
                }

                return Result.Ok(new RequestStateFaker().Generate());
            }

            private void SetupRequestPageMocks(Result<HtmlDocument> result)
            {
                this.requestorMock
                    .Setup(requestor => requestor.RequestCollectionsPage())
                    .Returns(result);
            }

            private void SetupRequestPostCodeLookupMocks(PostCode postCode, Result<HtmlDocument> result, bool hasRequestState = true)
            {
                Result<RequestState> requestStateResult = CreateRequestStateResult(!hasRequestState);
                Result<HtmlDocument> previousRequestResult = Result.Ok(new HtmlDocument());

                this.SetupRequestPageMocks(previousRequestResult);

                this.parserMock
                    .Setup(parser => parser.ExtractRequestState(previousRequestResult.ValueOrDefault))
                    .Returns(requestStateResult);

                this.requestorMock
                    .Setup(requestor => requestor.RequestPostCodeLookup(postCode, requestStateResult.ValueOrDefault))
                    .Returns(result);
            }

            private void SetupRequestUprnLookupMocks(
                PostCode postCode,
                Result<bool>? isSupportedResult = null,
                Result<Uprn>? extractUprnResult = null,
                Result<HtmlDocument>? result = null,
                bool hasRequestState = true)
            {
                if (isSupportedResult is null)
                {
                    isSupportedResult = Result.Ok(true);
                }

                if (extractUprnResult is null)
                {
                    extractUprnResult = Result.Ok(new UprnFaker().Generate());
                }

                if (result is null)
                {
                    result = Result.Ok(new HtmlDocument());
                }

                Result<RequestState> requestStateResult = CreateRequestStateResult(!hasRequestState);
                Result<HtmlDocument> previousRequestResult = Result.Ok(new HtmlDocument());

                this.SetupRequestPostCodeLookupMocks(postCode, previousRequestResult);

                this.parserMock
                    .Setup(parser => parser.IsWithinBoundary(previousRequestResult.ValueOrDefault))
                    .Returns(isSupportedResult);
                this.parserMock
                    .Setup(parser => parser.ExtractRequestState(previousRequestResult.ValueOrDefault))
                    .Returns(requestStateResult);
                this.parserMock
                    .Setup(parser => parser.ExtractUprn(previousRequestResult.ValueOrDefault))
                    .Returns(extractUprnResult);

                this.requestorMock
                    .Setup(requestor => requestor.RequestUprnLookup(extractUprnResult.ValueOrDefault, requestStateResult.ValueOrDefault))
                    .Returns(result);
            }

            private void SetupRequestCollectionLookupsMocks(PostCode postCode, Result<HtmlDocument> result, bool hasRequestState = true)
            {
                Result<RequestState> requestStateResult = CreateRequestStateResult(!hasRequestState);
                Result<HtmlDocument> previousRequestResult = Result.Ok(new HtmlDocument());

                this.SetupRequestUprnLookupMocks(postCode, result: previousRequestResult);

                this.parserMock
                    .Setup(parser => parser.ExtractRequestState(previousRequestResult.ValueOrDefault))
                    .Returns(requestStateResult);

                this.requestorMock
                    .Setup(requestor => requestor.RequestCollectionsLookup(requestStateResult.ValueOrDefault))
                    .Returns(result);
            }

            private void SetupExtractCollectionsMocks(
                PostCode postCode,
                Result<bool>? doesDoCollectionsResult = null,
                Result<IEnumerable<CollectionDay>>? collectionExtractionsResult = null)
            {
                if (doesDoCollectionsResult is null)
                {
                    doesDoCollectionsResult = Result.Ok(true);
                }

                if (collectionExtractionsResult is null)
                {
                    collectionExtractionsResult = Result.Ok(new CollectionDayFaker().Generate(3).AsEnumerable());
                }

                Result<HtmlDocument> previousRequestResult = Result.Ok(new HtmlDocument());

                this.SetupRequestCollectionLookupsMocks(postCode, previousRequestResult);

                this.parserMock
                    .Setup(parser => parser.DoesCollectAtAddress(previousRequestResult.ValueOrDefault))
                    .Returns(doesDoCollectionsResult);
                this.parserMock
                    .Setup(parser => parser.ExtractCollections(previousRequestResult.ValueOrDefault))
                    .Returns(collectionExtractionsResult);
            }
        }
    }
}
