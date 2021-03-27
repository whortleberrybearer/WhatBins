namespace WhatBins.Tests.Fakes
{
    using Bogus;
    using WhatBins.Types;

    public class CollectionStateFaker
    {
        private readonly Faker faker = new Faker();

        public CollectionState Generate()
        {
            return this.faker.Random.Enum<CollectionState>();
        }
    }
}
