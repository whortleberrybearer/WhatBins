namespace WhatBins.Tests.Fakes
{
    using Bogus;
    using System.Collections.Generic;
    using WhatBins.Types;

    public class BinFaker
    {
        private readonly Faker faker = new Faker();

        public IEnumerable<Bin> Generate(int count)
        {
            List<Bin> bins = new List<Bin>();

            for (int i = 0; i < count; i++)
            {
                bins.Add(new Bin(this.faker.Random.Enum<BinColour>()));
            }

            return bins;
        }
    }
}
