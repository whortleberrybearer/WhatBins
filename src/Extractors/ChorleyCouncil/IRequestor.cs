namespace WhatBins.Extractors.ChorleyCouncil
{
    using WhatBins.Types;

    public interface IRequestor
    {
        RequestResult RequestCollectionsPage();

        RequestResult RequestPostCodeLookup(PostCode postCode, RequestState requestState);

        RequestResult RequestUprnLookup(Uprn uprn, RequestState requestState);

        RequestResult RequestCollectionsLookup(RequestState requestState);
    }
}