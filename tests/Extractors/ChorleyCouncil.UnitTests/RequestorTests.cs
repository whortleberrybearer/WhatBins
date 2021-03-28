namespace WhatBins.Extractors.ChorleyCouncil.UnitTests
{
    using System;
    using FluentAssertions;
    using Moq;
    using RestSharp;
    using WhatBins.Extractors.ChorleyCouncil.UnitTests.Fakes;
    using WhatBins.Types;
    using WhatBins.Types.Fakes;
    using Xunit;

    public class RequestorTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenClientIsNull()
            {
                Action a = () => new Requestor(null!);

                a.Should().Throw<ArgumentNullException>();
            }
        }

        public class RequestCollectionsPageTests
        {
            private readonly MockRepository mockRepository;
            private readonly Mock<IRestClient> restClientMock;
            private readonly Requestor sut;

            public RequestCollectionsPageTests()
            {
                this.mockRepository = new MockRepository(MockBehavior.Strict);
                this.restClientMock = this.mockRepository.Create<IRestClient>();
                this.sut = new Requestor(this.restClientMock.Object);
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails()
            {
                restClientMock
                    .Setup(client => client.Execute(It.IsAny<RestRequest>(), Method.GET))
                    .Returns(new RestResponse());

                RequestResult result = this.sut.RequestCollectionsPage();

                result.Should().BeEquivalentTo(RequestResult.Failed);
            }
        }

        public class RequestPostCodeLookupTests
        {
            private readonly MockRepository mockRepository;
            private readonly Mock<IRestClient> restClientMock;
            private readonly Requestor sut;

            public RequestPostCodeLookupTests()
            {
                this.mockRepository = new MockRepository(MockBehavior.Strict);
                this.restClientMock = this.mockRepository.Create<IRestClient>();
                this.sut = new Requestor(this.restClientMock.Object);
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails()
            {
                PostCode postCode = new PostCodeFaker().Generate();
                RequestState requestState = new RequestStateFaker().Generate();

                restClientMock
                    .Setup(client => client.Execute(It.IsAny<RestRequest>(), Method.POST))
                    .Returns(new RestResponse());

                RequestResult result = this.sut.RequestPostCodeLookup(postCode, requestState);

                result.Should().BeEquivalentTo(RequestResult.Failed);
            }
        }

        public class RequestUprnLookupTests
        {
            private readonly MockRepository mockRepository;
            private readonly Mock<IRestClient> restClientMock;
            private readonly Requestor sut;

            public RequestUprnLookupTests()
            {
                this.mockRepository = new MockRepository(MockBehavior.Strict);
                this.restClientMock = this.mockRepository.Create<IRestClient>();
                this.sut = new Requestor(this.restClientMock.Object);
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails()
            {
                Uprn uprn = new UprnFaker().Generate();
                RequestState requestState = new RequestStateFaker().Generate();

                restClientMock
                    .Setup(client => client.Execute(It.IsAny<RestRequest>(), Method.POST))
                    .Returns(new RestResponse());

                RequestResult result = this.sut.RequestUprnLookup(uprn, requestState);

                result.Should().BeEquivalentTo(RequestResult.Failed);
            }
        }

        public class RequestCollectionsLookupTests
        {
            private readonly MockRepository mockRepository;
            private readonly Mock<IRestClient> restClientMock;
            private readonly Requestor sut;

            public RequestCollectionsLookupTests()
            {
                this.mockRepository = new MockRepository(MockBehavior.Strict);
                this.restClientMock = this.mockRepository.Create<IRestClient>();
                this.sut = new Requestor(this.restClientMock.Object);
            }

            [Fact]
            public void ShouldReturnRequestFailedWhenRequestFails()
            {
                RequestState requestState = new RequestStateFaker().Generate();

                restClientMock
                    .Setup(client => client.Execute(It.IsAny<RestRequest>(), Method.POST))
                    .Returns(new RestResponse());

                RequestResult result = this.sut.RequestCollectionsLookup(requestState);

                result.Should().BeEquivalentTo(RequestResult.Failed);
            }
        }
    }
}
