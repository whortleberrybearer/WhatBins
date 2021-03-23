namespace WhatBins
{
    using System.Collections.Generic;

    public class LookupResult
    {
        public CollectionState State { get; set; }

        public IEnumerable<Collection> Collections { get; set; }
    }
}
