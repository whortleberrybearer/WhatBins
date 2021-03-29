namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;
    using HtmlAgilityPack;

    public class RequestResult
    {
        private RequestResult(bool success, string? html = null)
        {
            this.Success = success;

            if (!string.IsNullOrEmpty(html))
            {
                this.HtmlDocument = new HtmlDocument();
                this.HtmlDocument.LoadHtml(html);
            }
            else
            {
                this.HtmlDocument = null;
            }
        }

        public static RequestResult Failed { get; } = new RequestResult(false);

        public bool Success { get; }

        public HtmlDocument? HtmlDocument { get; }

        public static RequestResult Succeeded(string html)
        {
            if (html is null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            return new RequestResult(true, html);
        }
    }
}
