namespace WhatBins.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // TODO: Const for new ExtractResult(CollectionState.Unsupported)
    // TODO: Const for new ExtractResult(CollectionState.NoCollections)

    public class ExtractResult
    {
        // TODO: Remove
        public ExtractResult(CollectionState state)
            : this(state, Enumerable.Empty<Collection>())
        {
        }

        // TODO: Remove state
        public ExtractResult(CollectionState state, IEnumerable<Collection> collections)
        {
            this.Collections = collections?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(collections));
            this.State = state;
        }

        public CollectionState State { get; }

        public IEnumerable<Collection> Collections { get; }
    }
}