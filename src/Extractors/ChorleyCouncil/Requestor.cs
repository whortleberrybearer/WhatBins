namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using FluentResults;
    using HtmlAgilityPack;
    using RestSharp;
    using WhatBins.Types;

    public class Requestor : IRequestor
    {
#pragma warning disable S1075 // URIs should not be hardcoded
        private static readonly Uri BaseUri = new Uri("https://myaccount.chorley.gov.uk/wastecollections.aspx");
#pragma warning restore S1075 // URIs should not be hardcoded
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

        public Result<HtmlDocument> RequestCollectionsPage()
        {
            IRestRequest request = this.requestBuilder.BuildCollectionsPageRequest();

            return CreateRequestResult(this.client.Get(request));
        }

        public Result<HtmlDocument> RequestPostCodeLookup(PostCode postCode, RequestState requestState)
        {
            IRestRequest request = this.requestBuilder.BuildPostCodeLookupRequest(postCode, requestState);

            return CreateRequestResult(this.client.Post(request));
        }

        public Result<HtmlDocument> RequestUprnLookup(Uprn uprn, RequestState requestState)
        {
            IRestRequest request = this.requestBuilder.BuildUprnLookupRequest(uprn, requestState);

            return CreateRequestResult(this.client.Post(request));
        }

        public Result<HtmlDocument> RequestCollectionsLookup(RequestState requestState)
        {
            IRestRequest request = this.requestBuilder.BuildCollectionsLookupRequest(requestState);

            return CreateRequestResult(this.client.Post(request));
        }

        private static Result<HtmlDocument> CreateRequestResult(IRestResponse response)
        {
            if (!response.IsSuccessful)
            {
                return Result.Fail(response.ErrorMessage);
            }

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response.Content);

            return Result.Ok(htmlDocument);
        }
    }
}