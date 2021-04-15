namespace WhatBins.GoogleFunctions.WhatBinsLookup
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using FluentResults;
    using Google.Cloud.Functions.Framework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using NodaTime;
    using NodaTime.Serialization.JsonNet;
    using Serilog;
    using Serilog.Context;
    using Serilog.Sinks.ILogger;
    using WhatBins.Extractors.ChorleyCouncil;
    using WhatBins.Types;

    public class Function : IHttpFunction
    {
        private readonly ILogger log;
        private readonly IBinCollectionsFinder binCollectionsFinder;

        public Function(Microsoft.Extensions.Logging.ILogger<Function> logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.log = new LoggerConfiguration()
                .WriteTo.ILogger(logger)
                .CreateLogger();

            IEnumerable<ICollectionExtractor> collectionExtractors = new ICollectionExtractor[]
            {
                new CollectionExtractor(this.log),
            };
            this.binCollectionsFinder = new BinCollectionsFinder(this.log, collectionExtractors);
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

            using (LogContext.PushProperty("traceIdentifier", context.TraceIdentifier))
            {
                this.log.Debug("Looking up collections for {postcode}", postCode);

                // As we now have a valid post code, try and get the collections.
                Result<Collection> lookupResult = this.binCollectionsFinder.Lookup(postCode);

                // Serialiser settings required to covert dates and enums to suitable values.
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                settings.Converters.Add(new StringEnumConverter());

                if (lookupResult.IsSuccess)
                {
                    this.log
                        .ForContext("collection", lookupResult.Value)
                        .Information("Collections lookup for {postcode} succeeded with state {collectionState}", postCode, lookupResult.Value.State);
                }
                else
                {
                    this.log
                        .ForContext("errors", lookupResult.Errors)
                        .Error("Collections lookup for {postcode} failed", postCode);
                }

                // If the lookup failed, return that the lookup is unsupported.
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(lookupResult.ValueOrDefault ?? Collection.Unsupported, settings));
            }
        }
    }
}
