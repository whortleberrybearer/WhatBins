namespace WhatBins.Extractors.ChorleyCouncil.UnitTests.Fakes
{
    using Bogus;

    public class RequestStateFaker
    {
        private readonly Faker faker = new Faker();

        public RequestState Generate()
        {
            return new RequestState(
                this.faker.Random.String(),
                this.faker.Random.String(),
                this.faker.Random.String());
        }
    }
}
