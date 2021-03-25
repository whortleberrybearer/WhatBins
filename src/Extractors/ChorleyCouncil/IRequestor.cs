using WhatBins.Types;

namespace WhatBins.Extractors.ChorleyCouncil
{
    public interface IRequestor
    {
        void DoRequest1(PostCode postCode);
    }
}