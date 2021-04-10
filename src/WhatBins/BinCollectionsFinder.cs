namespace WhatBins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentResults;
    using WhatBins.Types;

    public class BinCollectionsFinder : IBinCollectionsFinder
    {
        private readonly IEnumerable<ICollectionExtractor> collectionExtractors;

        public BinCollectionsFinder(IEnumerable<ICollectionExtractor> collectionExtractors)
        {
            this.collectionExtractors = collectionExtractors ?? throw new ArgumentNullException(nameof(collectionExtractors));
        }

        public Result<LookupResult> Lookup(PostCode postCode)
        {
            ExtractResult? result = null;

            foreach (ICollectionExtractor collectionExtractor in this.collectionExtractors.Where(extractor => extractor.CanExtract(postCode)))
            {
                result = collectionExtractor.Extract(postCode);

                // If the result is unsupported, we have not found the correct extractor to check, so keep checking.
                if (result.State != CollectionState.Unsupported)
                {
                    break;
                }
            }

            return Result.Ok(result?.ToLookupResult() ?? new LookupResult(CollectionState.Unsupported));
        }
    }
}