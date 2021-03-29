namespace WhatBins.Types.Fakes
{
    using Bogus;
    using WhatBins.Types;

    public class UprnFaker
    {
        private readonly Faker faker = new Faker();

        public Uprn Generate()
        {
            return new Uprn($"UPRN{this.faker.Random.ReplaceNumbers("##############")}");
        }
    }
}
