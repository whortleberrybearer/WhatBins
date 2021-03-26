﻿namespace WhatBins.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct ExtractResult
    {
        public ExtractResult(CollectionState state)
            : this(state, Enumerable.Empty<Collection>())
        {
        }

        public ExtractResult(CollectionState state, IEnumerable<Collection> collections)
        {
            Collections = collections?.ToArray() ?? throw new ArgumentNullException(nameof(collections));
            State = state;
        }

        public CollectionState State { get; }

        public IEnumerable<Collection> Collections { get; }
    }
}