namespace WhatBins.Extractors.ChorleyCouncil
{
    using WhatBins.Types;

    public interface IRequestor
    {
        RequestResult DoRequest1();

        RequestResult DoRequest2(PostCode postCode, RequestState requestState);

        RequestResult DoRequest3(Uprn uprn, RequestState requestState);

        RequestResult DoRequest4(RequestState requestState);
    }
}