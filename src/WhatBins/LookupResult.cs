namespace WhatBins
{
    using System.Collections.Generic;

    public struct LookupResult
    {
        public CollectionState State { get; }

        public IEnumerable<Collection> Collections { get; }
    }
}
