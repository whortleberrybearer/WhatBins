namespace WhatBins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WhatBins.Types;

    public class BinCollectionsFinder
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

                // If the result is unsupported, we have not found the correct council to check, so keep checking.  Otherwise
                // we have found the correct council and can exit.
                if (result.State != CollectionState.Unsupported)
                {
                    break;
                }
            }

            return result?.ToLookupResult() ?? new LookupResult(CollectionState.Unsupported);
        }
    }
}