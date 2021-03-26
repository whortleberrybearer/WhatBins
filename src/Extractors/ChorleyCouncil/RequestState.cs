namespace WhatBins.Extractors.ChorleyCouncil
{
    using System;

    public struct RequestState
    {
        public RequestState(string viewState, string viewStateGenerator, string eventValidation)
        {
            this.ViewState = viewState ?? throw new ArgumentNullException(nameof(viewState));
            this.ViewStateGenerator = viewStateGenerator ?? throw new ArgumentNullException(nameof(viewStateGenerator));
            this.EventValidation = eventValidation ?? throw new ArgumentNullException(nameof(eventValidation));
        }

        public string ViewState { get; }

        public string ViewStateGenerator { get; }

        public string EventValidation { get; }
    }
}