﻿#pragma warning disable S1135 // Track uses of "TODO" tags
namespace WhatBins
{
    using System;
    using System.Collections.Generic;
    using FluentResults;
    using WhatBins.Types;

    public class BinCollectionsFinder : IBinCollectionsFinder
    {
        private readonly IEnumerable<ICollectionExtractor> collectionExtractors;

        public BinCollectionsFinder(IEnumerable<ICollectionExtractor> collectionExtractors)
        {
            this.collectionExtractors = collectionExtractors ?? throw new ArgumentNullException(nameof(collectionExtractors));
        }

        public Result<Collection> Lookup(PostCode postCode)
        {
            foreach (ICollectionExtractor collectionExtractor in this.collectionExtractors)
            {
                Result<bool> canExtractResult = collectionExtractor.CanExtract(postCode);

                if (canExtractResult.IsSuccess && canExtractResult.Value)
                {
                    Result<Collection> extractResult = collectionExtractor.Extract(postCode);

                    // If the result is unsupported, we have not found the correct extractor to check, so keep checking.
                    if (extractResult.IsSuccess && (extractResult.Value.State != CollectionState.Unsupported))
                    {
                        return extractResult;
                    }
                    else if (extractResult.IsFailed)
                    {
                        // TODO: Log any failures.
                    }
                }
                else if (canExtractResult.IsFailed)
                {
                    // TODO: Log any failures.
                }
            }

            // As we have failed to make a successful extraction, the postcode must be unsupported.
            return Result.Ok(Collection.Unsupported);
        }
    }
}
#pragma warning restore S1135 // Track uses of "TODO" tags