namespace WhatBins
{
    using System;
    using System.Collections.Generic;
    using FluentResults;
    using Serilog;
    using Serilog.Core;
    using WhatBins.Types;

    public class BinCollectionsFinder : IBinCollectionsFinder
    {
        private readonly ILogger log;
        private readonly IEnumerable<ICollectionExtractor> collectionExtractors;

        public BinCollectionsFinder(IEnumerable<ICollectionExtractor> collectionExtractors)
            : this(Logger.None, collectionExtractors)
        {
        }

        public BinCollectionsFinder(
            ILogger log,
            IEnumerable<ICollectionExtractor> collectionExtractors)
        {
            this.log = log ?? Logger.None;
            this.collectionExtractors = collectionExtractors ?? throw new ArgumentNullException(nameof(collectionExtractors));
        }

        public Result<Collection> Lookup(PostCode postCode)
        {
            // The result of this function can still succeed even if errors are found with some of the sources.
            // If all the valid sources report an error, it is treated as a failure, however an success will
            // be treated as such.
            Result result = new Result();

            foreach (ICollectionExtractor collectionExtractor in this.collectionExtractors)
            {
                Result<bool> canExtractResult = collectionExtractor.CanExtract(postCode);

                if (canExtractResult.IsSuccess && canExtractResult.Value)
                {
                    Result<Collection> extractResult = collectionExtractor.Extract(postCode);

                    // If the result is unsupported, we have not found the correct extractor to check, so keep checking.
                    if (extractResult.IsSuccess && (extractResult.Value.State != CollectionState.Unsupported))
                    {
                        if (result.IsFailed)
                        {
                            this.log
                                .ForContext("errors", result.Errors)
                                .Warning("Collections lookup for {postcode} succeeded but with errors", postCode);
                        }

                        return extractResult;
                    }
                    else if (extractResult.IsFailed)
                    {
                        result.WithErrors(extractResult.Errors);
                    }
                }
                else if (canExtractResult.IsFailed)
                {
                    result.WithErrors(canExtractResult.Errors);
                }
            }

            // If we have got this far and we have a failed result, return it as a failure.
            if (result.IsFailed)
            {
                return result;
            }

            // As we have failed to make a successful extraction, the postcode must be unsupported.
            return Result.Ok(Collection.Unsupported);
        }
    }
}