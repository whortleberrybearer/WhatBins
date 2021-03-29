#pragma warning disable S1135 // Track uses of "TODO" tags
namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using HtmlAgilityPack;
    using NodaTime;
    using NodaTime.Text;
    using WhatBins.Types;

    public class Parser : IParser
    {
        private static readonly LocalDatePattern MonthPattern = LocalDatePattern.CreateWithInvariantCulture("MMMM yyyy");
        private static readonly IDictionary<string, BinColour> BinColourLookup = new Dictionary<string, BinColour>()
        {
            { "Blue%20Bin2", BinColour.Blue },
            { "Grey%20Garden%20Waste%20Bin2", BinColour.Grey },
            { "Brown%20Bin2", BinColour.Brown },
            { "Green%20Bin2", BinColour.Green },
        };

        public bool IsWithinBoundary(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException(nameof(htmlDocument));
            }

            return !htmlDocument.ParsedText.Contains("No addresses found within Chorley Council boundaries for this address.");
        }

        public bool DoesCollectAtAddress(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException(nameof(htmlDocument));
            }

            return !htmlDocument.ParsedText.Contains("Our records indicate that we don't collect waste from your property");
        }

        public RequestState ExtractRequestState(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException(nameof(htmlDocument));
            }

            return new RequestState(
                htmlDocument.GetElementbyId("__VIEWSTATE").GetAttributeValue("value", string.Empty),
                htmlDocument.GetElementbyId("__VIEWSTATEGENERATOR").GetAttributeValue("value", string.Empty),
                htmlDocument.GetElementbyId("__EVENTVALIDATION").GetAttributeValue("value", string.Empty));
        }

        public Uprn ExtractUprn(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException(nameof(htmlDocument));
            }

            // This runs on the assumption that all the addresses in the post code are collected at the same time, so can just select the first.
            HtmlNode selectedOption = htmlDocument.DocumentNode.SelectSingleNode(".//*[contains(@name, 'ctl00$MainContent$addressSearch$ddlAddress')]/option[2]");

            return new Uprn(selectedOption.GetAttributeValue("value", string.Empty));
        }

        public IEnumerable<Collection> ExtractCollections(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                throw new ArgumentNullException(nameof(htmlDocument));
            }

            List<Collection> collections = new List<Collection>();

            // All the collections are stored in a table, with a month per row, then dates per column.
            foreach (HtmlNode rowNode in htmlDocument.DocumentNode.SelectNodes(".//table[contains(@class, \"WasteCollection\")]/tr"))
            {
                collections.AddRange(ProcessMonthRow(rowNode));
            }

            return collections;
        }

        private static IEnumerable<Collection> ProcessMonthRow(HtmlNode rowNode)
        {
            HtmlNode dateColumn = rowNode.SelectSingleNode("td[1]");
            ParseResult<LocalDate> monthParseResult = MonthPattern.Parse(dateColumn.InnerText);

            if (!monthParseResult.Success)
            {
                // TODO: Log invalid month.

                // As we can not get the month, don't continue to parse the row.  It is likely that the rest of the table wont parse as well,
                // resulting in no collections.
                return Enumerable.Empty<Collection>();
            }

            return ProcessDayColumns(rowNode.SelectNodes("td[position()>1]"), monthParseResult.Value);
        }

        private static IEnumerable<Collection> ProcessDayColumns(HtmlNodeCollection dayColumnNodes, LocalDate monthDate)
        {
            List<Collection> collections = new List<Collection>();

            foreach (HtmlNode dayColumnNode in dayColumnNodes)
            {
                // The date is stored on a separate paragraph, followed by the bin images.
                HtmlNode dayTextNode = dayColumnNode.SelectSingleNode("p");

                if (dayTextNode.InnerText.Trim() != string.Empty)
                {
                    if (int.TryParse(dayColumnNode.InnerText.Trim(), out int day))
                    {
                        // Need to subtract 1 from the day as the month will already be on the 1st.  So if you was 1, it would be setting it
                        // to the 2nd of the month instead of the 1st without the subtract.
                        LocalDate collectionDate = monthDate.Plus(Period.FromDays(day - 1));

                        collections.Add(new Collection(collectionDate, ProcessDayColumn(dayColumnNode)));
                    }
                    else
                    {
                        // The date is invalid, so nothing that can be done.
                        // TODO: Log invalid date.
                    }
                }
                else
                {
                    // No more days for the month.
                    break;
                }
            }

            return collections;
        }

        private static IEnumerable<Bin> ProcessDayColumn(HtmlNode dayColumnNode)
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
                        // TODO: Log unknown colour.
                    }
                }
            }

            return bins;
        }
    }
}
#pragma warning restore S1135 // Track uses of "TODO" tags