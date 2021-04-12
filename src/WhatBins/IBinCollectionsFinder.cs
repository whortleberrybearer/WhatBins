namespace WhatBins
{
    using FluentResults;
    using WhatBins.Types;

    public interface IBinCollectionsFinder
    {
        Result<CollectionExtraction> Lookup(PostCode postCode);
    }
}