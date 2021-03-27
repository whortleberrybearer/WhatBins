namespace WhatBins.Types.Fakes
{
    using Bogus;
    using NodaTime;
    using WhatBins.Types;

    public class CollectionFaker : Faker<Collection>
    {
        private readonly BinFaker binFaker = new BinFaker();

        public CollectionFaker()
        {
            this.StrictMode(true);

            this.CustomInstantiator(faker => new Collection(LocalDate.FromDateTime(faker.Date.Soon()), this.binFaker.Generate(3)));

            this.AssertConfigurationIsValid();
        }
    }
}
