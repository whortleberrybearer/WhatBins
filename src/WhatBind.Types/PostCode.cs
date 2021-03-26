namespace WhatBins.Types
{
    public struct PostCode
    {
        private string value;

        public PostCode(string value)
            : this()
        {
            // TOCO CHeck if valid
            this.value = value;

            
            string[] parts = this.value.Split(' ');

            this.Outcode = parts[0];
            this.Incode = parts[1];
        }

        public string Outcode { get; }  // First nit

        public string Incode { get; } // Second bit
    }
}
