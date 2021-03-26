namespace WhatBins.UnitTests.Fakes
{
    using Bogus;
    using NodaTime;
    using System.Collections.Generic;
    using WhatBins.Types;

    public class CollectionFaker
    {
        private readonly Faker faker = new Faker();
        private readonly BinFaker binFaker = new BinFaker();

        public IEnumerable<Collection> Generate(int count)
        {
            List<Collection> collections = new List<Collection>();

            for (int i = 0; i < count; i++)
            {
                collections.Add(new Collection(
                    LocalDate.FromDateTime(this.faker.Date.Soon()),
                    this.binFaker.Generate(this.faker.Random.Number(1, 5))));
            }

            return collections;
        }
    }
}
