namespace WhatBins
{
    using WhatBins.Types;

    public interface IBinCollectionsFinder
    {
        LookupResult Lookup(PostCode postCode);
    }
}