namespace WhatBins.UnitTests.Fakes
{
    using Bogus;
    using WhatBins.Types;

    public class ExtractResultFaker
    {
        private readonly Faker faker = new Faker();
        private readonly CollectionFaker collectionFaker = new CollectionFaker();

        public ExtractResult Generate()
        {
            return new ExtractResult(
                this.faker.Random.Enum<CollectionState>(),
                this.collectionFaker.Generate(this.faker.Random.Number(1, 5)));
        }
    }
}
