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

        public Result<ExtractResult> Extract(PostCode postCode)
        {
            Result<HtmlDocument> requestResult = this.requestor.RequestCollectionsPage();

            return EnsureRequestSucceeded(requestResult) ?? this.ProcessCollectionsPageAndContinue(postCode, requestResult.Value!);
        }

        private static Result<ExtractResult>? EnsureRequestSucceeded(Result<HtmlDocument> requestResult)
        {
            if (requestResult.IsFailed)
            {
                // We have failed with our initial request, so just report as unsupported at this time.
                // It may be the page is no longer available.
                // TODO: Log failure of request
                return Result.Ok(new ExtractResult(CollectionState.Unsupported));
            }

            return null;
        }

        private Result<ExtractResult> ProcessCollectionsPageAndContinue(PostCode postCode, HtmlDocument htmlDocument)
        {
            Result<HtmlDocument> requestResult = this.requestor.RequestPostCodeLookup(
                postCode,
                this.parser.ExtractRequestState(htmlDocument));

            return EnsureRequestSucceeded(requestResult) ?? this.ProcessPostCodeLookupAndContinue(requestResult.Value!);
        }

        private Result<ExtractResult> ProcessPostCodeLookupAndContinue(HtmlDocument htmlDocument)
        {
            if (!this.parser.IsWithinBoundary(htmlDocument))
            {
                // Response indicates the post code is not in the area.
                return Result.Ok(new ExtractResult(CollectionState.Unsupported));
            }

            Result<HtmlDocument> requestResult = this.requestor.RequestUprnLookup(
                this.parser.ExtractUprn(htmlDocument),
                this.parser.ExtractRequestState(htmlDocument));

            return EnsureRequestSucceeded(requestResult) ?? this.ProcessUprnLookupAndContinue(requestResult.Value!);
        }

        private Result<ExtractResult> ProcessUprnLookupAndContinue(HtmlDocument htmlDocument)
        {
            Result<HtmlDocument> requestResult = this.requestor.RequestCollectionsLookup(
                this.parser.ExtractRequestState(htmlDocument));

            return EnsureRequestSucceeded(requestResult) ?? this.ExtractCollections(requestResult.Value!);
        }

        private Result<ExtractResult> ExtractCollections(HtmlDocument htmlDocument)
        {
            if (!this.parser.DoesCollectAtAddress(htmlDocument))
            {
                return Result.Ok(new ExtractResult(CollectionState.NoCollection));
            }

            IEnumerable<Collection> collections = this.parser.ExtractCollections(htmlDocument);

            // It shouldn't happen, but id there are no collections returned, set the state as no collections.
            return Result.Ok(collections.Any() ? new ExtractResult(CollectionState.Collection, collections) : new ExtractResult(CollectionState.NoCollection));
        }
    }
}