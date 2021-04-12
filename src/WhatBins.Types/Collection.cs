namespace WhatBins.Types
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Collection
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
    }
}