namespace WhatBins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WhatBins.Types;

    public struct LookupResult
    {
        public LookupResult(CollectionState state)
            : this(state, Enumerable.Empty<Collection>())
        {
        }

        public LookupResult(CollectionState state, IEnumerable<Collection> collections)
        {
            this.Collections = collections?.ToArray() ?? throw new ArgumentNullException(nameof(collections));
            this.State = state;
        }

        public CollectionState State { get; }

        public IEnumerable<Collection> Collections { get; }
    }
}
