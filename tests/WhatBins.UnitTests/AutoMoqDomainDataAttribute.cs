namespace WhatBins.UnitTests
{
    using System;
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoFixture.Xunit2;

    [AttributeUsage(AttributeTargets.Method)]
    public class AutoMoqDomainDataAttribute : AutoDataAttribute
    {
        public AutoMoqDomainDataAttribute()
            : base(() => CreateFixture())
        {
        }

        private static IFixture CreateFixture()
        {
            return new Fixture().Customize(new AutoMoqCustomization() { ConfigureMembers = true });
        }
    }
}