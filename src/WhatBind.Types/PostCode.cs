namespace WhatBins.Types
{
    public struct PostCode
    {
        private string value;

        public PostCode(string value)
            : this()
        {
            this.value = value;
        }

        public string Outcode { get; }  // First nit

        public string Incode { get; } // Second bit
    }
}
