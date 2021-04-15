namespace WhatBins.Types
{
    using FluentResults;

    public interface ICollectionExtractor
    {
        Result<Collection> Extract(PostCode postCode);

        Result<bool> CanExtract(PostCode postCode);
    }
}