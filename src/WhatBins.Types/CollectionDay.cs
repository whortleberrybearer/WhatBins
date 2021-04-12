namespace WhatBins.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NodaTime;

    public class CollectionDay
    {
        public CollectionDay(LocalDate date, IEnumerable<Bin> bins)
        {
            this.Bins = bins?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(bins));
            this.Date = date;
        }

        public LocalDate Date { get; }

        public IEnumerable<Bin> Bins { get; }
    }
}
