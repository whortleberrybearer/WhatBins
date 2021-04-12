namespace WhatBins.GoogleFunctions.WhatBinsLookup
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using FluentResults;
    using Google.Cloud.Functions.Framework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using NodaTime;
    using NodaTime.Serialization.JsonNet;
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
            if (!context.Request.Query.TryGetValue("postcode", out StringValues values) && (values.Count != 1))
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
            Result<Collection> lookupResult = this.binCollectionsFinder.Lookup(postCode);

            // Serialiser settings required to covert dates and enums to suitable values.
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            settings.Converters.Add(new StringEnumConverter());

            // If the lookup failed, return that the lookup is unsupported.
            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(lookupResult.ValueOrDefault ?? Collection.Unsupported, settings));
        }
    }
}
