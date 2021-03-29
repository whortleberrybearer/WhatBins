namespace WhatBins.Extractors.ChorleyCouncil
{
    using RestSharp;
    using WhatBins.Types;

    public interface IRequestBuilder
    {
        IRestRequest BuildCollectionsPageRequest();

        IRestRequest BuildPostCodeLookupRequest(PostCode postCode, RequestState requestState);

        IRestRequest BuildUprnLookupRequest(Uprn uprn, RequestState requestState);

        IRestRequest BuildCollectionsLookupRequest(RequestState requestState);
    }
}
