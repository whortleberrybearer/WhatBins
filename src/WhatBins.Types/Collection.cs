namespace WhatBins.Types
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public struct Collection
    {
        public Collection(IEnumerable<CollectionDay> collectionDays)
            : this(CollectionState.Collection, collectionDays)
        {
        }

        private Collection(CollectionState state)
            : this(state, Enumerable.Empty<CollectionDay>())
        {
        }

        private Collection(CollectionState state, IEnumerable<CollectionDay> collectionDays)
        {
            if (collectionDays is null)
            {
                throw new ArgumentNullException(nameof(collectionDays));
            }

            this.CollectionDays = new ReadOnlyCollection<CollectionDay>(collectionDays.ToList());
            this.State = state;
        }

        public static Collection Unsupported { get; } = new Collection(CollectionState.Unsupported);

        public static Collection NoCollection { get; } = new Collection(CollectionState.NoCollection);

        public CollectionState State { get; }

        public IEnumerable<CollectionDay> CollectionDays { get; }

        public static bool operator ==(Collection left, Collection right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Collection left, Collection right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Collection collection)
            {
                return this.Equals(collection);
            }

            return false;
        }

        public bool Equals(Collection collection)
        {
            return (this.State == collection.State) && this.CollectionDays.SequenceEqual(collection.CollectionDays);
        }

        public override int GetHashCode()
        {
            return new { this.State, this.CollectionDays }.GetHashCode();
        }
    }
}