////namespace WhatBins
////{
////    using System;
////    using System.Collections.Generic;
////    using System.Linq;
////    using WhatBins.Types;

////    // TODO: This and the extract result can be condensed.
////    public class LookupResult
////    {
////        public LookupResult(CollectionState state)
////            : this(state, Enumerable.Empty<Collection>())
////        {
////        }

////        public LookupResult(CollectionState state, IEnumerable<Collection> collections)
////        {
////            this.Collections = collections?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(collections));
////            this.State = state;
////        }

////        public CollectionState State { get; }

////        public IEnumerable<Collection> Collections { get; }
////    }
////}
