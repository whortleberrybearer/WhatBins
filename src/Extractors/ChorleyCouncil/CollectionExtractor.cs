namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentResults;
    using HtmlAgilityPack;
    using Serilog;
    using WhatBins.Types;

    public class CollectionExtractor : ICollectionExtractor
    {
        private static readonly IEnumerable<string> CouncilPostCodeAreas = new string[] { "PR6", "PR7", "PR25", "PR26", "BL6", "L40" };
        private readonly IParser parser;
        private readonly IRequestor requestor;

        public CollectionExtractor(ILogger log)
            : this(new Requestor(), new Parser(log))
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

        public Result<Collection> Extract(PostCode postCode)
        {
            Result<HtmlDocument> requestResult = this.requestor.RequestCollectionsPage();

            return EnsureRequestSucceeded(requestResult, () => this.ProcessCollectionsPageAndContinue(postCode, requestResult.Value!));
        }

        private static Result<Collection> EnsureRequestSucceeded(Result<HtmlDocument> requestResult, Func<Result<Collection>> nextAction)
        {
            if (requestResult.IsFailed)
            {
                return requestResult.ToResult<Collection>();
            }

            return nextAction();
        }

        private Result<Collection> ProcessCollectionsPageAndContinue(PostCode postCode, HtmlDocument htmlDocument)
        {
            Result<RequestState> extractStateResult = this.parser.ExtractRequestState(htmlDocument);

            if (extractStateResult.IsFailed)
            {
                return extractStateResult.ToResult<Collection>();
            }

            Result<HtmlDocument> requestResult = this.requestor.RequestPostCodeLookup(
                postCode,
                extractStateResult.Value);

            return EnsureRequestSucceeded(requestResult, () => this.ProcessPostCodeLookupAndContinue(requestResult.Value!));
        }

        private Result<Collection> ProcessPostCodeLookupAndContinue(HtmlDocument htmlDocument)
        {
            Result<bool> isWithinBoundaryResult = this.parser.IsWithinBoundary(htmlDocument);

            if (isWithinBoundaryResult.IsFailed)
            {
                return isWithinBoundaryResult.ToResult<Collection>();
            }

            if (!isWithinBoundaryResult.Value)
            {
                // Response indicates the post code is not in the area.
                return Result.Ok(Collection.Unsupported);
            }

            Result<RequestState> extractStateResult = this.parser.ExtractRequestState(htmlDocument);

            if (extractStateResult.IsFailed)
            {
                return extractStateResult.ToResult<Collection>();
            }

            Result<Uprn> extractUprnResult = this.parser.ExtractUprn(htmlDocument);

            if (extractUprnResult.IsFailed)
            {
                return extractUprnResult.ToResult<Collection>();
            }

            Result<HtmlDocument> requestResult = this.requestor.RequestUprnLookup(
                extractUprnResult.Value,
                extractStateResult.Value);

            return EnsureRequestSucceeded(requestResult, () => this.ProcessUprnLookupAndContinue(requestResult.Value!));
        }

        private Result<Collection> ProcessUprnLookupAndContinue(HtmlDocument htmlDocument)
        {
            Result<RequestState> extractStateResult = this.parser.ExtractRequestState(htmlDocument);

            if (extractStateResult.IsFailed)
            {
                return extractStateResult.ToResult<Collection>();
            }

            Result<HtmlDocument> requestResult = this.requestor.RequestCollectionsLookup(extractStateResult.Value);

            return EnsureRequestSucceeded(requestResult, () => this.ExtractCollections(requestResult.Value!));
        }

        private Result<Collection> ExtractCollections(HtmlDocument htmlDocument)
        {
            Result<bool> doesCollectResult = this.parser.DoesCollectAtAddress(htmlDocument);

            if (doesCollectResult.IsFailed)
            {
                return doesCollectResult.ToResult<Collection>();
            }

            if (!doesCollectResult.Value)
            {
                return Result.Ok(Collection.NoCollection);
            }

            Result<IEnumerable<CollectionDay>> collectionDaysResult = this.parser.ExtractCollections(htmlDocument);

            if (collectionDaysResult.IsFailed)
            {
                return collectionDaysResult.ToResult<Collection>();
            }

            // It shouldn't happen, but id there are no collections returned, set the state as no collections.
            return Result.Ok(collectionDaysResult.Value.Any() ? new Collection(collectionDaysResult.Value) : Collection.NoCollection);
        }
    }
}