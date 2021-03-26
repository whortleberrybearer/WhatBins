namespace WhatBins.Types
{
    public interface ICollectionExtractor
    {
        ExtractResult Extract(PostCode postCode);

        bool CanExtract(PostCode postCode);
    }
}