namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FluentResults;
    using HtmlAgilityPack;
    using NodaTime;
    using NodaTime.Text;
    using Serilog;
    using Serilog.Core;
    using WhatBins.Types;

    public class Parser : IParser
    {
        private static readonly YearMonthPattern MonthPattern = YearMonthPattern.CreateWithInvariantCulture("MMMM yyyy");
        private static readonly IDictionary<string, BinColour> BinColourLookup = new Dictionary<string, BinColour>()
        {
            { "Blue%20Bin2", BinColour.Blue },
            { "Grey%20Garden%20Waste%20Bin2", BinColour.Grey },
            { "Brown%20Bin2", BinColour.Brown },
            { "Green%20Bin2", BinColour.Green },
        };

        private readonly ILogger log;

        public Parser(ILogger log)
        {
            this.log = log ?? Logger.None;
        }

        public Result<bool> IsWithinBoundary(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException(nameof(htmlDocument));
            }

            return Result.Ok(!htmlDocument.ParsedText.Contains("No addresses found within Chorley Council boundaries for this address."));
        }

        public Result<bool> DoesCollectAtAddress(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException(nameof(htmlDocument));
            }

            return Result.Ok(!htmlDocument.ParsedText.Contains("Our records indicate that we don't collect waste from your property"));
        }

        public Result<RequestState> ExtractRequestState(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException(nameof(htmlDocument));
            }

            Result<RequestState> result = new Result<RequestState>();
            HtmlNode? viewStateNode = htmlDocument.GetElementbyId("__VIEWSTATE");

            if (viewStateNode is null)
            {
                result = result.WithError("ViewState node not found.");
            }

            HtmlNode? viewStateGeneratorNode = htmlDocument.GetElementbyId("__VIEWSTATEGENERATOR");

            if (viewStateGeneratorNode is null)
            {
                result = result.WithError("ViewStateGenerator node not found.");
            }

            HtmlNode? eventValidationNode = htmlDocument.GetElementbyId("__EVENTVALIDATION");

            if (eventValidationNode is null)
            {
                result = result.WithError("EventValidation node not found.");
            }

            if (result.IsFailed)
            {
                return result;
            }

            return Result.Ok(new RequestState(
                viewStateNode!.GetAttributeValue("value", string.Empty),
                viewStateGeneratorNode!.GetAttributeValue("value", string.Empty),
                eventValidationNode!.GetAttributeValue("value", string.Empty)));
        }

        public Result<Uprn> ExtractUprn(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException(nameof(htmlDocument));
            }

            // This runs on the assumption that all the addresses in the post code are collected at the same time, so can just select the first.
            HtmlNode? selectedOption = htmlDocument.DocumentNode.SelectSingleNode(".//*[contains(@name, 'ctl00$MainContent$addressSearch$ddlAddress')]/option[2]");

            if (selectedOption is null)
            {
                return Result.Fail("Uprn node not found.");
            }

            return Result.Ok(new Uprn(selectedOption.GetAttributeValue("value", string.Empty)));
        }

        public Result<IEnumerable<CollectionDay>> ExtractCollections(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException(nameof(htmlDocument));
            }

            List<CollectionDay> collectionDays = new List<CollectionDay>();

            // All the collections are stored in a table, with a month per row, then dates per column.
            foreach (HtmlNode rowNode in htmlDocument.DocumentNode.SelectNodes(".//table[contains(@class, \"WasteCollection\")]/tr"))
            {
                Result<IEnumerable<CollectionDay>> monthRowResult = this.ProcessMonthRow(rowNode);

                if (monthRowResult.IsFailed)
                {
                    return monthRowResult;
                }

                collectionDays.AddRange(monthRowResult.Value);
            }

            return Result.Ok(collectionDays.AsEnumerable());
        }

        private Result<IEnumerable<CollectionDay>> ProcessMonthRow(HtmlNode rowNode)
        {
            HtmlNode? dateColumn = rowNode.SelectSingleNode("td[1]");
            ParseResult<YearMonth> monthParseResult = MonthPattern.Parse(dateColumn?.InnerText!);

            if (!monthParseResult.Success)
            {
                // As we can not get the month, don't continue to parse the row.  It is likely that the rest of the table wont parse as well,
                // resulting in no collections.
                return Result.Fail<IEnumerable<CollectionDay>>(new ExceptionalError(monthParseResult.Exception));
            }

            return this.ProcessDayColumns(rowNode.SelectNodes("td[position()>1]"), monthParseResult.Value);
        }

        private Result<IEnumerable<CollectionDay>> ProcessDayColumns(HtmlNodeCollection dayColumnNodes, YearMonth yearMonth)
        {
            List<CollectionDay> collectionDays = new List<CollectionDay>();

            foreach (HtmlNode dayColumnNode in dayColumnNodes)
            {
                // The date is stored on a separate paragraph, followed by the bin images.
                HtmlNode? dayTextNode = dayColumnNode.SelectSingleNode("p");

                if (!string.IsNullOrEmpty(dayTextNode?.InnerText?.Trim()))
                {
                    if (!int.TryParse(dayColumnNode.InnerText.Trim(), out int day))
                    {
                        return Result.Fail<IEnumerable<CollectionDay>>("Invalid date");
                    }

                    try
                    {
                        collectionDays.Add(new CollectionDay(yearMonth.OnDayOfMonth(day), this.ProcessDayColumn(dayColumnNode)));
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        // This can happen is the day is not in the month of the year.
                        return Result.Fail<IEnumerable<CollectionDay>>(new ExceptionalError(ex));
                    }
                }
                else
                {
                    // No more days for the month.
                    break;
                }
            }

            return Result.Ok(collectionDays.AsEnumerable());
        }

        private IEnumerable<Bin> ProcessDayColumn(HtmlNode dayColumnNode)
        {
            List<Bin> bins = new List<Bin>();

            // The bin image nodes are stored in a separate table.
            foreach (var binImageNode in dayColumnNode.SelectNodes("table//td/img"))
            {
                string imgSrc = binImageNode.GetAttributeValue("src", string.Empty);

                if (imgSrc != string.Empty)
                {
                    string fileName = Path.GetFileNameWithoutExtension(imgSrc);

                    if (BinColourLookup.TryGetValue(fileName, out BinColour binColour))
                    {
                        bins.Add(new Bin(binColour));
                    }
                    else
                    {
                        this.log.Warning("Unable to determine BinColour for fileName {fileName}", fileName);
                    }
                }
            }

            return bins;
        }
    }
}