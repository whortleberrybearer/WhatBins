namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentResults;
    using HtmlAgilityPack;
    using WhatBins.Types;

    public class CollectionExtractor : ICollectionExtractor
    {
        private static readonly IEnumerable<string> CouncilPostCodeAreas = new string[] { "PR6", "PR7", "PR25", "PR26", "BL6", "L40" };
        private readonly IParser parser;
        private readonly IRequestor requestor;

        public CollectionExtractor()
            : this(new Requestor(), new Parser())
        {
        }

        public CollectionExtractor(IRequestor requestor, IParser parser)
        {
            this.requestor = requestor ?? throw new ArgumentNullException(nameof(requestor));
            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public Result<bool> CanExtract(PostCode postCode)
        {
            return Result.Ok(CouncilPostCodeAreas.Contains(postCode.Outcode));
        }

        public Result<CollectionExtraction> Extract(PostCode postCode)
        {
            Result<HtmlDocument> requestResult = this.requestor.RequestCollectionsPage();

            return EnsureRequestSucceeded(requestResult) ?? this.ProcessCollectionsPageAndContinue(postCode, requestResult.Value!);
        }

        private static Result<CollectionExtraction>? EnsureRequestSucceeded(Result<HtmlDocument> requestResult)
        {
            if (requestResult.IsFailed)
            {
                // We have failed with our initial request, so just report as unsupported at this time.
                // It may be the page is no longer available.
                // TODO: Log failure of request
                return Result.Ok(CollectionExtraction.Unsupported);
            }

            return null;
        }

        private Result<CollectionExtraction> ProcessCollectionsPageAndContinue(PostCode postCode, HtmlDocument htmlDocument)
        {
            Result<RequestState> extractStateResult = this.parser.ExtractRequestState(htmlDocument);

            if (extractStateResult.IsFailed)
            {
                return extractStateResult.ToResult<CollectionExtraction>();
            }

            Result<HtmlDocument> requestResult = this.requestor.RequestPostCodeLookup(
                postCode,
                extractStateResult.Value);

            return EnsureRequestSucceeded(requestResult) ?? this.ProcessPostCodeLookupAndContinue(requestResult.Value!);
        }

        private Result<CollectionExtraction> ProcessPostCodeLookupAndContinue(HtmlDocument htmlDocument)
        {
            Result<bool> isWithinBoundaryResult = this.parser.IsWithinBoundary(htmlDocument);

            if (isWithinBoundaryResult.IsFailed)
            {
                return isWithinBoundaryResult.ToResult<CollectionExtraction>();
            }

            if (!isWithinBoundaryResult.Value)
            {
                // Response indicates the post code is not in the area.
                return Result.Ok(CollectionExtraction.Unsupported);
            }

            Result<RequestState> extractStateResult = this.parser.ExtractRequestState(htmlDocument);

            if (extractStateResult.IsFailed)
            {
                return extractStateResult.ToResult<CollectionExtraction>();
            }

            Result<Uprn> extractUprnResult = this.parser.ExtractUprn(htmlDocument);

            if (extractUprnResult.IsFailed)
            {
                return extractUprnResult.ToResult<CollectionExtraction>();
            }

            Result<HtmlDocument> requestResult = this.requestor.RequestUprnLookup(
                extractUprnResult.Value,
                extractStateResult.Value);

            return EnsureRequestSucceeded(requestResult) ?? this.ProcessUprnLookupAndContinue(requestResult.Value!);
        }

        private Result<CollectionExtraction> ProcessUprnLookupAndContinue(HtmlDocument htmlDocument)
        {
            Result<RequestState> extractStateResult = this.parser.ExtractRequestState(htmlDocument);

            if (extractStateResult.IsFailed)
            {
                return extractStateResult.ToResult<CollectionExtraction>();
            }

            Result<HtmlDocument> requestResult = this.requestor.RequestCollectionsLookup(extractStateResult.Value);

            return EnsureRequestSucceeded(requestResult) ?? this.ExtractCollections(requestResult.Value!);
        }

        private Result<CollectionExtraction> ExtractCollections(HtmlDocument htmlDocument)
        {
            Result<bool> doesCollectResult = this.parser.DoesCollectAtAddress(htmlDocument);

            if (doesCollectResult.IsFailed)
            {
                return doesCollectResult.ToResult<CollectionExtraction>();
            }

            if (!doesCollectResult.Value)
            {
                return Result.Ok(CollectionExtraction.NoCollection);
            }

            Result<IEnumerable<Collection>> collectionsResult = this.parser.ExtractCollections(htmlDocument);

            if (collectionsResult.IsFailed)
            {
                return collectionsResult.ToResult<CollectionExtraction>();
            }

            // It shouldn't happen, but id there are no collections returned, set the state as no collections.
            return Result.Ok(collectionsResult.Value.Any() ? new CollectionExtraction(collectionsResult.Value) : CollectionExtraction.NoCollection);
        }
    }
}