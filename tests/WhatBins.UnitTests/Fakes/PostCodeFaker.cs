namespace WhatBins.UnitTests.Fakes
{
    using Bogus;
    using WhatBins.Types;

    public class PostCodeFaker
    {
        private readonly Faker faker = new Faker();

        public PostCode Generate()
        {
            return new PostCode(this.faker.Random.Replace("??# #??"));
        }
    }
}
