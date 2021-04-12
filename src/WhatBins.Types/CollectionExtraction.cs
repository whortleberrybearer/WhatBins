namespace WhatBins.Types
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class CollectionExtraction
    {
        public CollectionExtraction(IEnumerable<Collection> collections)
            : this(CollectionState.Collection, collections)
        {
        }

        private CollectionExtraction(CollectionState state)
            : this(state, Enumerable.Empty<Collection>())
        {
        }

        private CollectionExtraction(CollectionState state, IEnumerable<Collection> collections)
        {
            if (collections is null)
            {
                throw new ArgumentNullException(nameof(collections));
            }

            this.Collections = new ReadOnlyCollection<Collection>(collections.ToList());
            this.State = state;
        }

        public static CollectionExtraction Unsupported { get; } = new CollectionExtraction(CollectionState.Unsupported);

        public static CollectionExtraction NoCollection { get; } = new CollectionExtraction(CollectionState.NoCollection);

        public CollectionState State { get; }

        public IEnumerable<Collection> Collections { get; }
    }
}