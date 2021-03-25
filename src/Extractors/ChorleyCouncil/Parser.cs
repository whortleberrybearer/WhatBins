namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using System.Collections.Generic;
    using HtmlAgilityPack;
    using NodaTime;
    using NodaTime.Text;
    using WhatBins.Types;

    // TODO: probably an extractor
    class Parser : IParser
    {
        private static readonly LocalDatePattern MonthPattern = LocalDatePattern.CreateWithInvariantCulture("MMMM yyyy");
        private static IDictionary<string, BinColour> BinColourLookup = new Dictionary<string, BinColour>()
        {
            { "Blue%20Bin2", BinColour.Blue },
            { "Grey%20Garden%20Waste%20Bin2", BinColour.Grey },
            { "Brown%20Bin2", BinColour.Brown },
            { "Green%20Bin2", BinColour.Green }
        };

        public bool IsSupported(string html)
        {
            return !html.Contains("No addresses found within Chorley Council boundaries for this address.");
        }

        public bool DoesDoCollections(string html)
        {
            return !html.Contains("Our records indicate that we don't collect waste from your property");
        }

        public object ExtractCollections(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            List<Collection> collections = new List<Collection>();

            foreach (HtmlNode rowNode in doc.DocumentNode.SelectNodes("/table/tbody/tr"))
            {
                collections.AddRange(this.ProcessMonthRow(rowNode));
            }

            throw new NotImplementedException();
        }

        private IEnumerable<Collection> ProcessMonthRow(HtmlNode rowNode)
        {
            HtmlNode dateColumn = rowNode.SelectSingleNode("td[1]");
            ParseResult<LocalDate> monthParseResult = MonthPattern.Parse(dateColumn.InnerText);

            if (monthParseResult.Success)
            {
                foreach (HtmlNode columnNode in rowNode.SelectNodes("td[position()>1]"))
                {
                    this.ProcessDayColumn(columnNode, monthParseResult.Value);
                }
            }
            else
            {
                // TODO: lof the failure.
            }

            throw new NotImplementedException();
        }

        private void ProcessDayColumn(HtmlNode columnNode, LocalDate month)
        {
            HtmlNode dayColumnNode = columnNode.SelectSingleNode("p");

            if (dayColumnNode.InnerText.Trim() != string.Empty)
            {
                if (int.TryParse(dayColumnNode.InnerText.Trim(), out int day))
                {
                    // Need to subtract 1 from the day as the month will already be on the 1st.  So if you was 1, it would be setting it
                    // to the 2nd of the month instead of the 1st without the subtract.
                    LocalDate collectionDate = month.Plus(Period.FromDays(day - 1));
                }
                else
                {
                    // TODO: Date buggered, not a lot can do
                }

            //                

            //                var binImages = binDay.SelectNodes("table//td/img");

            //                List<Bin> bins = new List<Bin>();

            //                foreach (var binImage in binImages)
            //                {
            //                    var src = binImage.GetAttributeValue("src", string.Empty);

            //                    var fileName = Path.GetFileNameWithoutExtension(src);

            //                    if (fileName == Blue)
            //                    {
            //                        bins.Add(new Bin() { Colour = BinColour.Blue });
            //                    }
            //                    else if (fileName == Grey)
            //                    {
            //                        bins.Add(new Bin() { Colour = BinColour.Grey });
            //                    }
            //                    else if (fileName == Brown)
            //                    {
            //                        bins.Add(new Bin() { Colour = BinColour.Brown });
            //                    }
            //                    else if (fileName == Green)
            //                    {
            //                        bins.Add(new Bin() { Colour = BinColour.Green });
            //                    }
            //                }

            //                things.Add(new thing() { Date = theBinDay, Bins = bins });
            }
            else
            {
                // No more days for the month.
                break;
            }

            throw new NotImplementedException();
        }
    }
}