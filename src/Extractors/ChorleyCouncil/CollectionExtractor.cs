namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            this.requestor.DoRequest1(postCode);

            // Do request 1
            // Checl if supported
            // Do request 2
            // Do request 3
            // Parse and return data

            throw new NotImplementedException();
        }
    }
}