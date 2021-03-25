using System;
using System.Collections.Generic;
using System.Text;

namespace WhatBins.Extractors.ChorleyCouncil
{
    class Parser
    {
    }
}



//static void Main(string[] args)
//{
//    var doc = new HtmlDocument();
//    doc.LoadHtml(thetable);

//    var rows = doc.DocumentNode.SelectNodes("/table/tbody/tr");
//    var pattern = InstantPattern.CreateWithInvariantCulture("MMMM yyyy");

//    List<thing> things = new List<thing>();

//    foreach (var row in rows)
//    {
//        var dateColumn = row.SelectSingleNode("td[1]");

//        var date = pattern.Parse(dateColumn.InnerText);

//        // The selected dates
//        var binDays = row.SelectNodes("td[position()>1]");

//        foreach (var binDay in binDays)
//        {
//            var dayColumn = binDay.SelectSingleNode("p");

//            if (dayColumn.InnerText.Trim() != string.Empty)
//            {
//                var day = int.Parse(dayColumn.InnerText.Trim());

//                var theBinDay = date.Value.Plus(Duration.FromDays(day - 1));

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
//            }
//            else
//            {
//                // No more days for the month.
//                break;
//            }
//        }
//    }


//    foreach (var thing in things)
//    {
//        Console.WriteLine($"Date: {thing.Date}, Bins: {string.Join(", ", thing.Bins.Select(a => a.Colour))}");
//    }

//    Console.ReadLine();
//}

//private const string Blue = "Blue%20Bin2";
//private const string Grey = "Grey%20Garden%20Waste%20Bin2";
//private const string Brown = "Brown%20Bin2";
//private const string Green = "Green%20Bin2";
