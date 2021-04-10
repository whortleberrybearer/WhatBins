namespace WhatBins
{
    using FluentResults;
    using WhatBins.Types;

    public interface IBinCollectionsFinder
    {
        Result<LookupResult> Lookup(PostCode postCode);
    }
}