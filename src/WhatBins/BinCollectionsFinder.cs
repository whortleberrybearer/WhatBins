namespace WhatBins
{
    using System;

    public class BinCollectionsFinder
    {
        public LookupResult Lookup(PostCode postCode)
        {
            return new LookupResult();
        }
    }
}












//using HtmlAgilityPack;
//using RestSharp;
//using SimpleBrowser;
//using System;
//using System.IO;
//using NodaTime;
//using NodaTime.Text;
//using System.Collections.Generic;
//using System.Linq;

//namespace ConsoleApp1
//{
//    class Program
//    {
//        public enum BinColour
//        {
//            Blue,
//            Green,
//            Brown,
//            Grey
//        }

//        public record Bin
//        {
//            public BinColour Colour { get; init; }
//    }

//    public record thing
//    {
//            public Instant Date { get; init; }

//    public IEnumerable<Bin> Bins { get; init; }
//}

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

//private static string thetable = File.ReadAllText("TextFile1.html");

//        //static void Main(string[] args)
//        //{
//        //    string something = "UPRN100010384440";
//        //    string url = "https://myaccount.chorley.gov.uk/wastecollections.aspx";
//        //    string postCode = "pr7 7hb";

//        //    var theFormId = "ctl08";
//        //    var postCodeLookupInputId = "MainContent_addressSearch_txtPostCodeLookup";
//        //    var findButtonId = "MainContent_addressSearch_btnFindAddress";

//        //    RestClient client = new RestClient();
//        //    var response = client.Get(new RestRequest(url));

//        //    if (response.IsSuccessful)
//        //    {
//        //        var doc = new HtmlDocument();
//        //        doc.LoadHtml(response.Content);

//        //        var viewStateElement = doc.GetElementbyId("__VIEWSTATE");
//        //        var viewStateGeneratorElement = doc.GetElementbyId("__VIEWSTATEGENERATOR");
//        //        var eventValidationElement = doc.GetElementbyId("__EVENTVALIDATION");

//        //        var postCodeRequest = new RestRequest(url, Method.POST);

//        //        postCodeRequest.AddParameter("__EVENTTARGET", "", ParameterType.GetOrPost);
//        //        postCodeRequest.AddParameter("__EVENTARGUMENT", "", ParameterType.GetOrPost);
//        //        postCodeRequest.AddParameter("__VIEWSTATE", viewStateElement.Attributes["value"].Value, ParameterType.GetOrPost);
//        //        postCodeRequest.AddParameter("__VIEWSTATEGENERATOR", viewStateGeneratorElement.Attributes["value"].Value, ParameterType.GetOrPost);
//        //        postCodeRequest.AddParameter("__EVENTVALIDATION", eventValidationElement.Attributes["value"].Value, ParameterType.GetOrPost);
//        //        postCodeRequest.AddParameter("ctl00$MainContent$addressSearch$txtPostCodeLookup", postCode, ParameterType.GetOrPost);
//        //        postCodeRequest.AddParameter("ctl00$MainContent$addressSearch$btnFindAddress", "Find Address", ParameterType.GetOrPost);
//        //        postCodeRequest.AddParameter("ctl00$toastQueue", "");

//        //        response = client.Post(postCodeRequest);


//        //        // Now need to select the 
//        //        doc.LoadHtml(response.Content);

//        //        viewStateElement = doc.GetElementbyId("__VIEWSTATE");
//        //        viewStateGeneratorElement = doc.GetElementbyId("__VIEWSTATEGENERATOR");
//        //        eventValidationElement = doc.GetElementbyId("__EVENTVALIDATION");

//        //        // TODO: need to handle if this isnt avalire.  Taking a punt on the first value
//        //        var selectedOption = doc.DocumentNode.SelectSingleNode(".//*[contains(@name, 'ctl00$MainContent$addressSearch$ddlAddress')]/option[2]");
//        //        something = selectedOption.GetAttributeValue("value", string.Empty);

//        //        var thingRequest = new RestRequest(url, Method.POST);

//        //        thingRequest.AddParameter("__EVENTTARGET", "ctl00$MainContent$addressSearch$ddlAddress", ParameterType.GetOrPost);
//        //        thingRequest.AddParameter("__EVENTARGUMENT", "", ParameterType.GetOrPost);
//        //        thingRequest.AddParameter("__LASTFOCUS", "", ParameterType.GetOrPost);
//        //        thingRequest.AddParameter("__VIEWSTATE", viewStateElement.Attributes["value"].Value, ParameterType.GetOrPost);
//        //        thingRequest.AddParameter("__VIEWSTATEGENERATOR", viewStateGeneratorElement.Attributes["value"].Value, ParameterType.GetOrPost);
//        //        thingRequest.AddParameter("__EVENTVALIDATION", eventValidationElement.Attributes["value"].Value, ParameterType.GetOrPost);
//        //        thingRequest.AddParameter("ctl00$MainContent$addressSearch$ddlAddress", something, ParameterType.GetOrPost);
//        //        thingRequest.AddParameter("ctl00$toastQueue", "");

//        //        response = client.Post(thingRequest);


//        //        // Final request to get the data.
//        //        doc.LoadHtml(response.Content);

//        //        viewStateElement = doc.GetElementbyId("__VIEWSTATE");
//        //        viewStateGeneratorElement = doc.GetElementbyId("__VIEWSTATEGENERATOR");
//        //        eventValidationElement = doc.GetElementbyId("__EVENTVALIDATION");

//        //        var finalRequest = new RestRequest(url, Method.POST);

//        //        finalRequest.AddParameter("__EVENTTARGET", "", ParameterType.GetOrPost);
//        //        finalRequest.AddParameter("__EVENTARGUMENT", "", ParameterType.GetOrPost);
//        //        finalRequest.AddParameter("__VIEWSTATE", viewStateElement.Attributes["value"].Value, ParameterType.GetOrPost);
//        //        finalRequest.AddParameter("__VIEWSTATEGENERATOR", viewStateGeneratorElement.Attributes["value"].Value, ParameterType.GetOrPost);
//        //        finalRequest.AddParameter("__EVENTVALIDATION", eventValidationElement.Attributes["value"].Value, ParameterType.GetOrPost);
//        //        finalRequest.AddParameter("ctl00$MainContent$btnSearch", "Search", ParameterType.GetOrPost);
//        //        finalRequest.AddParameter("ctl00$toastQueue", "");

//        //        response = client.Post(finalRequest);
//        //    }

//        //}
//    }
//}