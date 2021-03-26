namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public bool CanExtract(PostCode postCode)
        {
            return CouncilPostCodeAreas.Contains(postCode.Outcode);
        }

        public ExtractResult Extract(PostCode postCode)
        {
            RequestResult requestResult = this.requestor.DoRequest1();

            return Something(requestResult) ?? this.Continue1(postCode, requestResult.HtmlDocument!);
        }

        private ExtractResult Continue1(PostCode postCode, HtmlDocument htmlDocument)
        {
            RequestResult requestResult = this.requestor.DoRequest2(
                postCode,
                this.parser.ExtractRequestState(htmlDocument));

            return Something(requestResult) ?? this.Continue2(requestResult.HtmlDocument!);
        }

        private ExtractResult Continue2(HtmlDocument htmlDocument)
        {
            if (!this.parser.IsSupported(htmlDocument))
            {
                // Response indicates the post code is not in the area.
                return new ExtractResult(CollectionState.Unsupported);
            }

            RequestResult requestResult = this.requestor.DoRequest3(
                this.parser.ExtractUprn(htmlDocument),
                this.parser.ExtractRequestState(htmlDocument));

            return Something(requestResult) ?? this.Continue3(requestResult.HtmlDocument!);
        }

        private ExtractResult Continue3(HtmlDocument htmlDocument)
        {
            if (!this.parser.DoesDoCollections(htmlDocument))
            {
                return new ExtractResult(CollectionState.NoCollection);
            }

            RequestResult requestResult = this.requestor.DoRequest4(
                this.parser.ExtractRequestState(htmlDocument));

            return Something(requestResult) ?? this.Continue4(requestResult.HtmlDocument!);
        }

        private ExtractResult Continue4(HtmlDocument htmlDocument)
        {
            IEnumerable<Collection> collections = this.parser.ExtractCollections(htmlDocument);

            // It shouldn't happen, but id there are no collections returned, set the state as no collections.
            return collections.Any() ? new ExtractResult(CollectionState.Collection, collections) : new ExtractResult(CollectionState.NoCollection);
        }

        private static ExtractResult? Something(RequestResult requestResult)
        {
            if (!requestResult.Success)
            {
                // We have failed with our initial request, so just report as unsupported at this time.
                // It may be the page is no longer available.
                return new ExtractResult(CollectionState.Unsupported);
            }

            return null;
        }
    }
}