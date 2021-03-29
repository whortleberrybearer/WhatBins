namespace WhatBins.Extractors.ChorleyCouncil.UnitTests
{
    using FluentAssertions;
    using FluentAssertions.Execution;
    using RestSharp;
    using WhatBins.Extractors.ChorleyCouncil.UnitTests.Fakes;
    using WhatBins.Types;
    using WhatBins.Types.Fakes;
    using Xunit;

    public class RequestBuilderTests
    {
        public class BuildCollectionsPageRequestTests
        {
            private readonly RequestBuilder sut = new RequestBuilder();

            [Fact]
            public void ShouldReturnRequest()
            {
                IRestRequest result = this.sut.BuildCollectionsPageRequest();

                result.Should().NotBeNull();
            }
        }

        public class BuildPostCodeLookupRequestTests
        {
            private readonly RequestBuilder sut = new RequestBuilder();

            [Fact]
            public void ShouldReturnRequestWithRequestParameters()
            {
                PostCode postCode = new PostCodeFaker().Generate();
                RequestState requestState = new RequestStateFaker().Generate();

                IRestRequest result = this.sut.BuildPostCodeLookupRequest(postCode, requestState);

                result.Should().NotBeNull();

                using (new AssertionScope())
                {
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__EVENTTARGET").Which.Value.Should().Be(string.Empty);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__EVENTARGUMENT").Which.Value.Should().Be(string.Empty);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__VIEWSTATE").Which.Value.Should().Be(requestState.ViewState);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__VIEWSTATEGENERATOR").Which.Value.Should().Be(requestState.ViewStateGenerator);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__EVENTVALIDATION").Which.Value.Should().Be(requestState.EventValidation);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "ctl00$toastQueue").Which.Value.Should().Be(string.Empty);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "ctl00$MainContent$addressSearch$txtPostCodeLookup").Which.Value.Should().Be((string)postCode);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "ctl00$MainContent$addressSearch$btnFindAddress").Which.Value.Should().Be("Find Address");
                }
            }
        }

        public class BuildUprnLookupRequestTests
        {
            private readonly RequestBuilder sut = new RequestBuilder();

            [Fact]
            public void ShouldReturnRequestWithRequestParameters()
            {
                Uprn uprn = new UprnFaker().Generate();
                RequestState requestState = new RequestStateFaker().Generate();

                IRestRequest result = this.sut.BuildUprnLookupRequest(uprn, requestState);

                result.Should().NotBeNull();

                using (new AssertionScope())
                {
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__EVENTTARGET").Which.Value.Should().Be("ctl00$MainContent$addressSearch$ddlAddress");
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__EVENTARGUMENT").Which.Value.Should().Be(string.Empty);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__VIEWSTATE").Which.Value.Should().Be(requestState.ViewState);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__VIEWSTATEGENERATOR").Which.Value.Should().Be(requestState.ViewStateGenerator);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__EVENTVALIDATION").Which.Value.Should().Be(requestState.EventValidation);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "ctl00$toastQueue").Which.Value.Should().Be(string.Empty);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "ctl00$MainContent$addressSearch$ddlAddress").Which.Value.Should().Be((string)uprn);
                }
            }
        }

        public class CollectionsLookupRequestTests
        {
            private readonly RequestBuilder sut = new RequestBuilder();

            [Fact]
            public void ShouldReturnRequestWithRequestParameters()
            {
                RequestState requestState = new RequestStateFaker().Generate();

                IRestRequest result = this.sut.BuildCollectionsLookupRequest(requestState);

                result.Should().NotBeNull();

                using (new AssertionScope())
                {
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__EVENTTARGET").Which.Value.Should().Be(string.Empty);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__EVENTARGUMENT").Which.Value.Should().Be(string.Empty);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__VIEWSTATE").Which.Value.Should().Be(requestState.ViewState);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__VIEWSTATEGENERATOR").Which.Value.Should().Be(requestState.ViewStateGenerator);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "__EVENTVALIDATION").Which.Value.Should().Be(requestState.EventValidation);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "ctl00$toastQueue").Which.Value.Should().Be(string.Empty);
                    result.Parameters.Should().Contain(parameter => parameter.Name == "ctl00$MainContent$btnSearch").Which.Value.Should().Be("Search");
                }
            }
        }
    }
}
