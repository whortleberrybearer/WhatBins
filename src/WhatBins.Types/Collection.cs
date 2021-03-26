namespace WhatBins.Types
{
    using NodaTime;
    using System.Collections.Generic;
    using System.Linq;

    public class Collection
    {
        public Collection(LocalDate date, IEnumerable<Bin> bins)
        {
            this.Bins = bins?.ToArray() ?? throw new System.ArgumentNullException(nameof(bins));
            this.Date = date;
        }

        public LocalDate Date { get; }

        public IEnumerable<Bin> Bins { get; }
    }
}
