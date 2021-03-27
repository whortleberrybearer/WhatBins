namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using RestSharp;
    using WhatBins.Types;

    public class Requestor : IRequestor
    {
        private static readonly Uri BaseUri = new Uri("https://myaccount.chorley.gov.uk/wastecollections.aspx");
        private IRestClient client;

        public Requestor()
            : this(new RestClient(BaseUri))
        {
        }

        public Requestor(IRestClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public RequestResult RequestCollectionsPage()
        {
            return CreateRequestResult(this.client.Get(new RestRequest(Method.GET)));
        }

        public RequestResult RequestPostCodeLookup(PostCode postCode, RequestState requestState)
        {
            IRestRequest request = CreateAndPopulatePostRequest(requestState);

            // These additional parameters are required to identify what type the request is.
            request.AddParameter("ctl00$MainContent$addressSearch$txtPostCodeLookup", (string)postCode, ParameterType.GetOrPost);
            request.AddParameter("ctl00$MainContent$addressSearch$btnFindAddress", "Find Address", ParameterType.GetOrPost);
            request.AddParameter("ctl00$toastQueue", "");

            return CreateRequestResult(this.client.Post(request));
        }

        public RequestResult RequestUprnLookup(Uprn uprn, RequestState requestState)
        {
            IRestRequest request = CreateAndPopulatePostRequest(requestState, "ctl00$MainContent$addressSearch$ddlAddress");

            request.AddParameter("ctl00$MainContent$addressSearch$ddlAddress", (string)uprn, ParameterType.GetOrPost);
            request.AddParameter("ctl00$toastQueue", "");

            return CreateRequestResult(this.client.Post(request));
        }

        public RequestResult RequestCollectionsLookup(RequestState requestState)
        {
            IRestRequest request = CreateAndPopulatePostRequest(requestState);

            request.AddParameter("ctl00$MainContent$btnSearch", "Search", ParameterType.GetOrPost);
            request.AddParameter("ctl00$toastQueue", "");

            return CreateRequestResult(this.client.Post(request));
        }

        private static IRestRequest CreateAndPopulatePostRequest(RequestState requestState, string? eventTarget = null)
        {
            IRestRequest request = new RestRequest(Method.POST);

            request.AddParameter("__EVENTTARGET", eventTarget ?? string.Empty, ParameterType.GetOrPost);
            request.AddParameter("__EVENTARGUMENT", string.Empty, ParameterType.GetOrPost);
            request.AddParameter("__VIEWSTATE", requestState.ViewState);
            request.AddParameter("__VIEWSTATEGENERATOR", requestState.ViewStateGenerator);
            request.AddParameter("__EVENTVALIDATION", requestState.EventValidation);

            return request;
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