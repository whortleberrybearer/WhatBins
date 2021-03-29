namespace WhatBins.Types.Fakes
{
    using Bogus;
    using WhatBins.Types;

    public class BinColourFaker
    {
        private readonly Faker faker = new Faker();

        public BinColour Generate()
        {
            return this.faker.Random.Enum<BinColour>();
        }
    }
}