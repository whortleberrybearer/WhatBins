namespace WhatBins.Types.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public class UprnTests
    {
        public static IEnumerable<object[]> ValidUprns { get; } = new List<object[]>()
            {
                new object[] { "UPRN100023336956" },
                new object[] { "UPRN000000199356" },
            };

        public static IEnumerable<object[]> GenerateNonEqualUprns()
        {
            return ValidUprns.Skip(1).Select((uprn, i) => new object[] { uprn[0], ValidUprns.ElementAt(i)[0] });
        }

        public class ConstructorTests
        {
            [Fact]
            public void ShouldThrowArgumentNullWhenValueNull()
            {
                Action a = () => new Uprn(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [InlineData("")]
            [InlineData("UPRN")]
            [InlineData("0")]
            [InlineData("UPNR0")]
            [InlineData("QWER094")]
            public void ShouldThrowArguemntWhenValueInvalid(string value)
            {
                Action a = () => new Uprn(value);

                a.Should().Throw<ArgumentException>();
            }
        }

        public class EqualsOperatorTests
        {
            [Theory]
            [MemberData(nameof(ValidUprns), MemberType = typeof(UprnTests))]
            public void ObjectsShouldBeEqual(string value)
            {
                Uprn uprn1 = new Uprn(value);
                Uprn uprn2 = new Uprn(value);

                bool result = uprn1 == uprn2;

                result.Should().BeTrue();
            }

            [Theory]
            [MemberData(nameof(GenerateNonEqualUprns), MemberType = typeof(UprnTests))]
            public void ObjectsShouldNotBeEqual(string value1, string value2)
            {
                Uprn uprn1 = new Uprn(value1);
                Uprn uprn2 = new Uprn(value2);

                bool result = uprn1 == uprn2;

                result.Should().BeFalse();
            }
        }

        public class NotEqualsOperatorTests
        {
            [Theory]
            [MemberData(nameof(ValidUprns), MemberType = typeof(UprnTests))]
            public void ObectsShouldBeNotEqual(string value)
            {
                Uprn uprn1 = new Uprn(value);
                Uprn uprn2 = new Uprn(value);

                bool result = uprn1 != uprn2;

                result.Should().BeFalse();
            }

            [Theory]
            [MemberData(nameof(GenerateNonEqualUprns), MemberType = typeof(UprnTests))]
            public void ObjectsShouldNotBeEqual(string value1, string value2)
            {
                Uprn uprn1 = new Uprn(value1);
                Uprn uprn2 = new Uprn(value2);

                bool result = uprn1 != uprn2;

                result.Should().BeTrue();
            }
        }

        public class EqualsObjectTests
        {
            [Theory]
            [MemberData(nameof(ValidUprns), MemberType = typeof(UprnTests))]
            public void ShouldBeTrueWhenMatchingObjects(string value)
            {
                Uprn sut = new Uprn(value);
                object obj = new Uprn(value);

                bool result = sut.Equals(obj);

                result.Should().BeTrue();
            }

            [Theory]
            [MemberData(nameof(ValidUprns), MemberType = typeof(UprnTests))]
            public void ShouldBeFaleWhenNull(string value)
            {
                Uprn sut = new Uprn(value);

                bool result = sut.Equals(null);

                result.Should().BeFalse();
            }

            [Theory]
            [MemberData(nameof(ValidUprns), MemberType = typeof(UprnTests))]
            public void ShouldBeFaleWhenDifferentObject(string value)
            {
                Uprn sut = new Uprn(value);

                bool result = sut.Equals(new object());

                result.Should().BeFalse();
            }
        }

        public class StringOperatorTests
        {
            [Theory]
            [MemberData(nameof(ValidUprns), MemberType = typeof(UprnTests))]
            public void ObjectShouldActLikeAString(string value)
            {
                Uprn sut = new Uprn(value);

                sut.Should().Be(value);
            }
        }

        public class ToStringTests
        {
            [Theory]
            [MemberData(nameof(ValidUprns), MemberType = typeof(UprnTests))]
            public void ShouldReturnOriginalValue(string value)
            {
                Uprn sut = new Uprn(value);

                string result = sut.ToString();

                result.Should().Be(value);
            }
        }

        public class GetHashCodeTests
        {
            [Theory]
            [MemberData(nameof(ValidUprns), MemberType = typeof(UprnTests))]
            public void ShouldReturnOriginalValueHashCode(string value)
            {
                Uprn sut = new Uprn(value);

                int result = sut.GetHashCode();

                result.Should().Be(value.GetHashCode());
            }
        }
    }
}
