namespace WhatBins.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ExtractResult
    {
        public ExtractResult(CollectionState state)
            : this(state, Enumerable.Empty<Collection>())
        {
        }

        public ExtractResult(CollectionState state, IEnumerable<Collection> collections)
        {
            Collections = collections?.ToList()?.AsReadOnly() ?? throw new ArgumentNullException(nameof(collections));
            State = state;
        }

        public CollectionState State { get; }

        public IEnumerable<Collection> Collections { get; }

        //public static bool operator ==(ExtractResult left, ExtractResult right)
        //{
        //    return left.Equals(right);
        //}

        //public static bool operator !=(ExtractResult left, ExtractResult right)
        //{
        //    return !(left == right);
        //}

        //public override bool Equals(object? obj)
        //{
        //    if (obj is ExtractResult extractResult)
        //    {
        //        return this.Equals(extractResult);
        //    }

        //    return false;
        //}

        //public bool Equals(ExtractResult other)
        //{
        //    return (this.State == other.State) && this.Collections.SequenceEqual(other.Collections);
        //}

        //public override int GetHashCode()
        //{
        //    return (this.State, this.Collections).GetHashCode();
        //}

        //public override string ToString()
        //{
        //    return this.State.ToString();
        //}
    }
}