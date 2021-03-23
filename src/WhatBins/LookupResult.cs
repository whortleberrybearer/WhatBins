namespace WhatBins
{
    using System.Collections.Generic;

    public struct LookupResult
    {
        public CollectionState State { get; private set; }

        public IEnumerable<Collection> Collections { get; private set; }
    }
}
