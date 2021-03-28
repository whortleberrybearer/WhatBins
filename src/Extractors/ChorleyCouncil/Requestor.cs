namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using RestSharp;
    using WhatBins.Types;

    public class Requestor : IRequestor
    {
        private static readonly Uri BaseUri = new Uri("https://myaccount.chorley.gov.uk/wastecollections.aspx");
        private readonly IRestClient client;
        private readonly IRequestBuilder requestBuilder;

        public Requestor()
            : this(new RestClient(BaseUri), new RequestBuilder())
        {
        }

        public Requestor(IRestClient client, IRequestBuilder requestBuilder)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.requestBuilder = requestBuilder ?? throw new ArgumentNullException(nameof(requestBuilder));
        }

        public RequestResult RequestCollectionsPage()
        {
            IRestRequest request = this.requestBuilder.BuildCollectionsPageRequest();

            return CreateRequestResult(this.client.Get(request));
        }

        public RequestResult RequestPostCodeLookup(PostCode postCode, RequestState requestState)
        {
            IRestRequest request = this.requestBuilder.BuildPostCodeLookupRequest(postCode, requestState);

            return CreateRequestResult(this.client.Post(request));
        }

        public RequestResult RequestUprnLookup(Uprn uprn, RequestState requestState)
        {
            IRestRequest request = this.requestBuilder.BuildUprnLookupRequest(uprn, requestState);

            return CreateRequestResult(this.client.Post(request));
        }

        public RequestResult RequestCollectionsLookup(RequestState requestState)
        {
            IRestRequest request = this.requestBuilder.BuildCollectionsLookupRequest(requestState);

            return CreateRequestResult(this.client.Post(request));
        }

        private static RequestResult CreateRequestResult(IRestResponse response)
        {
            if (!response.IsSuccessful)
            {
                return RequestResult.Failed;
            }

            return RequestResult.Succeeded(response.Content);
        }
    }
}