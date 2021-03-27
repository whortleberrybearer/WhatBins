namespace WhatBins.Tests.Fakes
{
    using Bogus;
    using WhatBins.Types;

    public class BinFaker : Faker<Bin>
    {
        public BinFaker()
        {
            this.StrictMode(true);

            this.CustomInstantiator(faker => new Bin(faker.Random.Enum<BinColour>()));

            this.AssertConfigurationIsValid();
        }
    }
}
