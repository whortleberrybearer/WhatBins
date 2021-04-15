namespace WhatBins.Extractors.ChorleyCouncil
{
    using FluentResults;
    using HtmlAgilityPack;
    using WhatBins.Types;

    public interface IRequestor
    {
        Result<HtmlDocument> RequestCollectionsPage();

        Result<HtmlDocument> RequestPostCodeLookup(PostCode postCode, RequestState requestState);

        Result<HtmlDocument> RequestUprnLookup(Uprn uprn, RequestState requestState);

        Result<HtmlDocument> RequestCollectionsLookup(RequestState requestState);
    }
}