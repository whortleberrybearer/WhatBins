namespace WhatBins.Types
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class CollectionExtraction
    {
        public CollectionExtraction(IEnumerable<CollectionDay> collectionDays)
            : this(CollectionState.Collection, collectionDays)
        {
        }

        private CollectionExtraction(CollectionState state)
            : this(state, Enumerable.Empty<CollectionDay>())
        {
        }

        private CollectionExtraction(CollectionState state, IEnumerable<CollectionDay> collectionDays)
        {
            if (collectionDays is null)
            {
                throw new ArgumentNullException(nameof(collectionDays));
            }

            this.CollectionDays = new ReadOnlyCollection<CollectionDay>(collectionDays.ToList());
            this.State = state;
        }

        public static CollectionExtraction Unsupported { get; } = new CollectionExtraction(CollectionState.Unsupported);

        public static CollectionExtraction NoCollection { get; } = new CollectionExtraction(CollectionState.NoCollection);

        public CollectionState State { get; }

        public IEnumerable<CollectionDay> CollectionDays { get; }
    }
}