namespace WhatBins.Types.Fakes
{
    using Bogus;
    using WhatBins.Types;

    public class BinFaker : Faker<Bin>
    {
        private readonly BinColourFaker binColourFaker = new BinColourFaker();

        public BinFaker()
        {
            this.StrictMode(true);

            this.CustomInstantiator(faker => new Bin(this.binColourFaker.Generate()));

            this.AssertConfigurationIsValid();
        }
    }
}
