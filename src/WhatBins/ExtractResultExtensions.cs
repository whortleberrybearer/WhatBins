namespace WhatBins
{
    using WhatBins.Types;

    public static class ExtractResultExtensions
    {
        public static LookupResult ToLookupResult(this ExtractResult extractResult)
        {
            return new LookupResult(extractResult.State, extractResult.Collections);
        }
    }
}
