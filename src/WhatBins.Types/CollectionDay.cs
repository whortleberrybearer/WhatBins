namespace WhatBins.Types
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NodaTime;

    public class CollectionDay
    {
        public CollectionDay(LocalDate date, IEnumerable<Bin> bins)
        {
            if (bins is null)
            {
                throw new ArgumentNullException(nameof(bins));
            }

            this.Bins = new ReadOnlyCollection<Bin>(bins.ToList());
            this.Date = date;
        }

        public LocalDate Date { get; }

        public IEnumerable<Bin> Bins { get; }
    }
}
