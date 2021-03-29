namespace WhatBins.GoogleFunctions.WhatBinsLookup
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Google.Cloud.Functions.Framework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using WhatBins.Extractors.ChorleyCouncil;
    using WhatBins.Types;

    public class Function : IHttpFunction
    {
        private readonly ILogger logger;
        private readonly IBinCollectionsFinder binCollectionsFinder;

        public Function(ILogger<Function> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            IEnumerable<ICollectionExtractor> collectionExtractors = new ICollectionExtractor[]
            {
                new CollectionExtractor(),
            };
            this.binCollectionsFinder = new BinCollectionsFinder(collectionExtractors);
        }

        public async Task HandleAsync(HttpContext context)
        {
            // Get the post code from the query if available.
            if (!context.Request.Query.TryGetValue("postcode", out StringValues values) && (values.Count == 1))
            {
                // Postcode is missing.
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return;
            }

            PostCode postCode;

            try
            {
                postCode = new PostCode(values[0]);
            }
            catch (ArgumentException)
            {
                // Postcode is missing.
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return;
            }

            this.logger.LogInformation("Looking up collections for {postcode}", postCode);

            // As we now have a valid post code, try and get the collections.
            LookupResult lookupResult = this.binCollectionsFinder.Lookup(postCode);

            await context.Response.WriteAsync(JsonSerializer.Serialize(lookupResult));
        }
    }
}
