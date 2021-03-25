using System;
using System.Collections.Generic;
using System.Text;

namespace WhatBins.Extractors.ChorleyCouncil
{
    class Requestor
    {
    }
}


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