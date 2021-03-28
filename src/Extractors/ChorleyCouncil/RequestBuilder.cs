namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using RestSharp;
    using WhatBins.Types;

    public class RequestBuilder : IRequestBuilder
    {
        public IRestRequest BuildCollectionsPageRequest()
        {
            throw new NotImplementedException();
        }

        public IRestRequest BuildPostCodeLookupRequest(PostCode postCode, RequestState requestState)
        {
            IRestRequest request = CreateAndPopulatePostRequest(requestState);

            // These additional parameters are required to identify what type the request is.
            request.AddParameter("ctl00$MainContent$addressSearch$txtPostCodeLookup", (string)postCode, ParameterType.GetOrPost);
            request.AddParameter("ctl00$MainContent$addressSearch$btnFindAddress", "Find Address", ParameterType.GetOrPost);
            request.AddParameter("ctl00$toastQueue", string.Empty, ParameterType.GetOrPost);

            return request;
        }

        public IRestRequest BuildUprnLookupRequest(Uprn uprn, RequestState requestState)
        {
            IRestRequest request = CreateAndPopulatePostRequest(requestState, "ctl00$MainContent$addressSearch$ddlAddress");

            request.AddParameter("ctl00$MainContent$addressSearch$ddlAddress", (string)uprn, ParameterType.GetOrPost);
            request.AddParameter("ctl00$toastQueue", string.Empty, ParameterType.GetOrPost);

            return request;
        }

        public IRestRequest BuildCollectionsLookupRequest(RequestState requestState)
        {
            IRestRequest request = CreateAndPopulatePostRequest(requestState);

            request.AddParameter("ctl00$MainContent$btnSearch", "Search", ParameterType.GetOrPost);
            request.AddParameter("ctl00$toastQueue", string.Empty, ParameterType.GetOrPost);

            return request;
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
    }
}
