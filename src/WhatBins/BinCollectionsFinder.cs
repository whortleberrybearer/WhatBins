namespace WhatBins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WhatBins.Types;

    public class BinCollectionsFinder : IBinCollectionsFinder
    {
        private IEnumerable<ICollectionExtractor> collectionExtractors;

        public BinCollectionsFinder(IEnumerable<ICollectionExtractor> collectionExtractors)
        {
            this.collectionExtractors = collectionExtractors ?? throw new ArgumentNullException(nameof(collectionExtractors));
        }

        public LookupResult Lookup(PostCode postCode)
        {
            ExtractResult? result = null;

            foreach (ICollectionExtractor collectionExtractor in this.collectionExtractors.Where(extractor => extractor.CanExtract(postCode)))
            {
                result = collectionExtractor.Extract(postCode);

                // If the result is unsupported, we have not found the correct extractor to check, so keep checking.
                if (result.Value.State != CollectionState.Unsupported)
                {
                    break;
                }
            }

            return result.HasValue ? result.Value.ToLookupResult() : new LookupResult(CollectionState.Unsupported);
        }
    }
}