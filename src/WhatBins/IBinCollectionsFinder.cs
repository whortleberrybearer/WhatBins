namespace WhatBins
{
    using FluentResults;
    using WhatBins.Types;

    public interface IBinCollectionsFinder
    {
        Result<Collection> Lookup(PostCode postCode);
    }
}