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
            this.Collections = collections?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(collections));
            this.State = state;
        }

        public CollectionState State { get; }

        public IEnumerable<Collection> Collections { get; }
    }
}