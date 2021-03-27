namespace WhatBins.Extractors.ChorleyCouncil.UnitTests
{
    using System;
    using Bogus;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using Xunit;

    public class RequestStateTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenViewStateIsNull()
            {
                Faker faker = new Faker();
                string eventValidation = faker.Random.String();
                string viewStateGenerator = faker.Random.String();

                Action a = () => new RequestState(null!, viewStateGenerator, eventValidation);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenViewStateGeneratorIsNull()
            {
                Faker faker = new Faker();
                string eventValidation = faker.Random.String();
                string viewState = faker.Random.String();

                Action a = () => new RequestState(viewState, null!, eventValidation);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenEventValidationIsNull()
            {
                Faker faker = new Faker();
                string viewState = faker.Random.String();
                string viewStateGenerator = faker.Random.String();

                Action a = () => new RequestState(viewState, viewStateGenerator, null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldSetAllProperties()
            {
                Faker faker = new Faker();
                string eventValidation = faker.Random.String();
                string viewState = faker.Random.String();
                string viewStateGenerator = faker.Random.String();

                RequestState sut = new RequestState(viewState, viewStateGenerator, eventValidation);

                using (new AssertionScope())
                {
                    sut.EventValidation.Should().Be(eventValidation);
                    sut.ViewState.Should().Be(viewState);
                    sut.ViewStateGenerator.Should().Be(viewStateGenerator);
                }
            }
        }
    }
}
