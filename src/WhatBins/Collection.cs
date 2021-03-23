namespace WhatBins
{
    using NodaTime;
    using System.Collections.Generic;

    public class Collection
    {
        public Instant Date { get; }

        public IEnumerable<Bin> Bins { get; }
    }
}
