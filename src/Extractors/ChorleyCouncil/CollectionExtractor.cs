namespace WhatBins
{
    using System.Collections.Generic;
    using System.Linq;
    using WhatBins.Types;

    public class CollectionExtractor : ICollectionExtractor
    {
        private static readonly IEnumerable<string> CouncilPostCodeAreas = new string[] { "PR6", "PR7", "PR25", "PR26", "BL6", "L40" };

        public bool CanExtract(PostCode postCode)
        {
            return CouncilPostCodeAreas.Contains(postCode.Outcode);
        }

        public ExtractResult Extract(PostCode postCode)
        {
            throw new System.NotImplementedException();

            // Do request 1
            // Checl if supported
            // Do request 2
            // Do request 3
            // Parse and return data
        }
    }
}