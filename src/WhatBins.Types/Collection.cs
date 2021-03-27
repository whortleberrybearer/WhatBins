namespace WhatBins.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NodaTime;

    public class Collection
    {
        public Collection(LocalDate date, IEnumerable<Bin> bins)
        {
            this.Bins = bins?.ToList()?.AsReadOnly() ?? throw new ArgumentNullException(nameof(bins));
            this.Date = date;
        }

        public LocalDate Date { get; }

        public IEnumerable<Bin> Bins { get; }

        //public static bool operator ==(Collection left, Collection right)
        //{
        //    return left.Equals(right);
        //}

        //public static bool operator !=(Collection left, Collection right)
        //{
        //    return !(left == right);
        //}

        //public override bool Equals(object? obj)
        //{
        //    if (obj is Collection collection)
        //    {
        //        return this.Equals(collection);
        //    }

        //    return false;
        //}

        //public bool Equals(Collection other)
        //{
        //    return (this.Date == other.Date) && this.Bins.SequenceEqual(other.Bins);
        //}

        //public override int GetHashCode()
        //{
        //    return (this.Date, this.Bins).GetHashCode();
        //}
    }
}
