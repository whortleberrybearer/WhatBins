using WhatBins.Types;

namespace WhatBins
{
    public interface IBinCollectionsFinder
    {
        LookupResult Lookup(PostCode postCode);
    }
}