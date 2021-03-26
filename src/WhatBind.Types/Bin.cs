namespace WhatBins.Types
{
    public struct Bin
    {
        public Bin(BinColour colour)
        {
            Colour = colour;
        }

        public BinColour Colour { get; }
    }
}
