namespace WhatBins.Types.Fakes
{
    using Bogus;
    using NodaTime;
    using WhatBins.Types;

    public class CollectionDayFaker : Faker<CollectionDay>
    {
        private readonly BinFaker binFaker = new BinFaker();

        public CollectionDayFaker()
        {
            this.StrictMode(true);

            this.CustomInstantiator(faker => new CollectionDay(LocalDate.FromDateTime(faker.Date.Soon()), this.binFaker.Generate(3)));

            this.AssertConfigurationIsValid();
        }
    }
}
