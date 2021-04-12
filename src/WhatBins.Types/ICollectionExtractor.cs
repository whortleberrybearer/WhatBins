namespace WhatBins.Types
{
    using FluentResults;

    public interface ICollectionExtractor
    {
        Result<CollectionExtraction> Extract(PostCode postCode);

        Result<bool> CanExtract(PostCode postCode);
    }
}