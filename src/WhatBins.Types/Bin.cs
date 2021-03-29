namespace WhatBins.Types
{
    public class Bin
    {
        public Bin(BinColour colour)
        {
            this.Colour = colour;
        }

        public BinColour Colour { get; }
    }
}
