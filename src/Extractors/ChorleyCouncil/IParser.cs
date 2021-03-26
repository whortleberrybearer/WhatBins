namespace WhatBins.Extractors.ChorleyCouncil
{
    using System.Collections.Generic;
    using HtmlAgilityPack;
    using WhatBins.Types;

    public interface IParser
    {
        bool DoesDoCollections(HtmlDocument htmlDocument);

        IEnumerable<Collection> ExtractCollections(HtmlDocument htmlDocument);

        RequestState ExtractRequestState(HtmlDocument htmlDocument);

        Uprn ExtractUprn(HtmlDocument htmlDocument);

        bool IsSupported(HtmlDocument htmlDocument);
    }
}