namespace WhatBins.Types
{
    public struct Uprn
    {
        private string value;

        public Uprn(string value)
            : this()
        {
            // TOCO CHeck if valid
            this.value = value;
        }

        public static implicit operator string(Uprn uprn) => uprn.value;
    }
}