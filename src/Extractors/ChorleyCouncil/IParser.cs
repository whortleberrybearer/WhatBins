namespace WhatBins.Extractors.ChorleyCouncil
{
    using System.Collections.Generic;
    using FluentResults;
    using HtmlAgilityPack;
    using WhatBins.Types;

    public interface IParser
    {
        Result<bool> DoesCollectAtAddress(HtmlDocument htmlDocument);

        Result<IEnumerable<CollectionDay>> ExtractCollections(HtmlDocument htmlDocument);

        Result<RequestState> ExtractRequestState(HtmlDocument htmlDocument);

        Result<Uprn> ExtractUprn(HtmlDocument htmlDocument);

        Result<bool> IsWithinBoundary(HtmlDocument htmlDocument);
    }
}