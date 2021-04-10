namespace WhatBins.Extractors.ChorleyCouncil.UnitTests
{
    using System;
    using System.Net;
    using Bogus;
    using FluentAssertions;
    using FluentResults;
    using FluentResults.Extensions.FluentAssertions;
    using HtmlAgilityPack;
    using Moq;
    using RestSharp;
    using WhatBins.Extractors.ChorleyCouncil.UnitTests.Fakes;
    using WhatBins.Types;
    using WhatBins.Types.Fakes;
    using Xunit;

    public class RequestorTests
    {
        private static IRestResponse CreateFailedResponse()
        {
            return new RestResponse()
            {
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = HttpStatusCode.InternalServerError,
            };
        }

        private static IRestResponse CreateOkResponse(string html)
        {
            return new RestResponse()
            {
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = HttpStatusCode.OK,
                Content = html,
            };
        }

        private static void ValidateSucceededResult(string html, Result<HtmlDocument> result)
        {
            // Need to compare the document manually as the equivalent comparison with fail due to the nature of the object.
            // The actual HTML the document parsed should match and can be compared against.
            result.Should().BeSuccess();
            result.Value.Should().NotBeNull();
            result.Value!.ParsedText.Should().Be(html);
        }

        public class ConstructorTests
        {
            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenClientIsNull()
            {
                Mock<IRequestBuilder> requestBuilderMock = new Mock<IRequestBuilder>();

                Action a = () => new Requestor(null!, requestBuilderMock.Object);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenRequestBuilderIsNull()
            {
                Mock<IRestClient> clientMock = new Mock<IRestClient>();

                Action a = () => new Requestor(clientMock.Object, null!);

                a.Should().Throw<ArgumentNullException>();
            }
        }

        public class RequestCollectionsPageTests
        {
            private readonly MockRepository mockRepository;
            private readonly Mock<IRestClient> restClientMock;
            private readonly Mock<IRequestBuilder> requestBuilderMock;
            private readonly Requestor sut;

            public RequestCollectionsPageTests()
            {
                this.mockRepository = new MockRepository(MockBehavior.Strict);
                this.restClientMock = this.mockRepository.Create<IRestClient>();
                this.requestBuilderMock = this.mockRepository.Create<IRequestBuilder>();
                this.sut = new Requestor(this.restClientMock.Object, this.requestBuilderMock.Object);
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails()
            {
                this.SetupMocks(CreateFailedResponse());

                Result<HtmlDocument> result = this.sut.RequestCollectionsPage();

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails1()
            {
                string html = new Faker().Random.String();

                this.SetupMocks(CreateOkResponse(html));

                Result<HtmlDocument> result = this.sut.RequestCollectionsPage();

                ValidateSucceededResult(html, result);
            }

            private void SetupMocks(IRestResponse response)
            {
                IRestRequest request = new RestRequest();

                this.requestBuilderMock
                    .Setup(builder => builder.BuildCollectionsPageRequest())
                    .Returns(request);
                this.restClientMock
                    .Setup(client => client.Execute(request, Method.GET))
                    .Returns(response);
            }
        }

        public class RequestPostCodeLookupTests
        {
            private readonly MockRepository mockRepository;
            private readonly Mock<IRestClient> restClientMock;
            private readonly Mock<IRequestBuilder> requestBuilderMock;
            private readonly Requestor sut;

            public RequestPostCodeLookupTests()
            {
                this.mockRepository = new MockRepository(MockBehavior.Strict);
                this.restClientMock = this.mockRepository.Create<IRestClient>();
                this.requestBuilderMock = this.mockRepository.Create<IRequestBuilder>();
                this.sut = new Requestor(this.restClientMock.Object, this.requestBuilderMock.Object);
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();
                RequestState requestState = new RequestStateFaker().Generate();

                this.SetupMocks(postCode, requestState, CreateFailedResponse());

                Result<HtmlDocument> result = this.sut.RequestPostCodeLookup(postCode, requestState);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails1()
            {
                string html = new Faker().Random.String();
                PostCode postCode = new PostCodeFaker().Generate();
                RequestState requestState = new RequestStateFaker().Generate();

                this.SetupMocks(postCode, requestState, CreateOkResponse(html));

                Result<HtmlDocument> result = this.sut.RequestPostCodeLookup(postCode, requestState);

                ValidateSucceededResult(html, result);
            }

            private void SetupMocks(PostCode postCode, RequestState requestState, IRestResponse response)
            {
                IRestRequest request = new RestRequest();

                this.requestBuilderMock
                    .Setup(builder => builder.BuildPostCodeLookupRequest(postCode, requestState))
                    .Returns(request);
                this.restClientMock
                    .Setup(client => client.Execute(request, Method.POST))
                    .Returns(response);
            }
        }

        public class RequestUprnLookupTests
        {
            private readonly MockRepository mockRepository;
            private readonly Mock<IRestClient> restClientMock;
            private readonly Mock<IRequestBuilder> requestBuilderMock;
            private readonly Requestor sut;

            public RequestUprnLookupTests()
            {
                this.mockRepository = new MockRepository(MockBehavior.Strict);
                this.restClientMock = this.mockRepository.Create<IRestClient>();
                this.requestBuilderMock = this.mockRepository.Create<IRequestBuilder>();
                this.sut = new Requestor(this.restClientMock.Object, this.requestBuilderMock.Object);
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails()
            {
                Uprn uprn = new UprnFaker().Generate();
                RequestState requestState = new RequestStateFaker().Generate();

                this.SetupMocks(uprn, requestState, CreateFailedResponse());

                Result<HtmlDocument> result = this.sut.RequestUprnLookup(uprn, requestState);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails1()
            {
                string html = new Faker().Random.String();
                Uprn uprn = new UprnFaker().Generate();
                RequestState requestState = new RequestStateFaker().Generate();

                this.SetupMocks(uprn, requestState, CreateOkResponse(html));

                Result<HtmlDocument> result = this.sut.RequestUprnLookup(uprn, requestState);

                ValidateSucceededResult(html, result);
            }

            private void SetupMocks(Uprn uprn, RequestState requestState, IRestResponse response)
            {
                IRestRequest request = new RestRequest();

                this.requestBuilderMock
                    .Setup(builder => builder.BuildUprnLookupRequest(uprn, requestState))
                    .Returns(request);
                this.restClientMock
                    .Setup(client => client.Execute(request, Method.POST))
                    .Returns(response);
            }
        }

        public class RequestCollectionsLookupTests
        {
            private readonly MockRepository mockRepository;
            private readonly Mock<IRestClient> restClientMock;
            private readonly Mock<IRequestBuilder> requestBuilderMock;
            private readonly Requestor sut;

            public RequestCollectionsLookupTests()
            {
                this.mockRepository = new MockRepository(MockBehavior.Strict);
                this.restClientMock = this.mockRepository.Create<IRestClient>();
                this.requestBuilderMock = this.mockRepository.Create<IRequestBuilder>();
                this.sut = new Requestor(this.restClientMock.Object, this.requestBuilderMock.Object);
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails()
            {
                RequestState requestState = new RequestStateFaker().Generate();

                this.SetupMocks(requestState, CreateFailedResponse());

                Result<HtmlDocument> result = this.sut.RequestCollectionsLookup(requestState);

                result.Should().BeFailure();
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails1()
            {
                string html = new Faker().Random.String();
                RequestState requestState = new RequestStateFaker().Generate();

                this.SetupMocks(requestState, CreateOkResponse(html));

                Result<HtmlDocument> result = this.sut.RequestCollectionsLookup(requestState);

                ValidateSucceededResult(html, result);
            }

            private void SetupMocks(RequestState requestState, IRestResponse response)
            {
                IRestRequest request = new RestRequest();

                this.requestBuilderMock
                    .Setup(builder => builder.BuildCollectionsLookupRequest(requestState))
                    .Returns(request);
                this.restClientMock
                    .Setup(client => client.Execute(request, Method.POST))
                    .Returns(response);
            }
        }
    }
}
