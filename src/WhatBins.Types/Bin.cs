namespace WhatBins.Types
{
    public class Bin
    {
        public Bin(BinColour colour)
        {
            Colour = colour;
        }

        public BinColour Colour { get; }
    }
}
