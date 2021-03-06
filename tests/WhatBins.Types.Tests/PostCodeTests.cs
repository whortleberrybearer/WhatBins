namespace WhatBins.Types.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using Xunit;

    public class PostCodeTests
    {
        public static IEnumerable<object[]> ValidPostCodes { get; } = new List<object[]>()
            {
                new object[] { "PR1 1AA" },
                new object[] { "W1 1AA" },
            };

        public static IEnumerable<object[]> GenerateNonEqualPostCodes()
        {
            return ValidPostCodes.Skip(1).Select((postCode, i) => new object[] { postCode[0], ValidPostCodes.ElementAt(i)[0] });
        }

        public class ConstructorTests
        {
            public static IEnumerable<object[]> SplitValidPostCodes()
            {
                return ValidPostCodes.Select(postCode => ((string)postCode[0]).Split(' '));
            }

            [Fact]
            public void ShouldThrowArgumentNullWhenValueNull()
            {
                Action a = () => new PostCode(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [InlineData("")]
            [InlineData("1W")]
            public void ShouldThrowArguemntWhenValueInvalid(string value)
            {
                Action a = () => new PostCode(value);

                a.Should().Throw<ArgumentException>();
            }

            [Theory]
            [MemberData(nameof(SplitValidPostCodes))]
            public void ShouldSetIncodeAndOutcode(string outcode, string incode)
            {
                PostCode sut = new PostCode($"{outcode} {incode}");

                using (new AssertionScope())
                {
                    sut.Incode.Should().Be(incode);
                    sut.Outcode.Should().Be(outcode);
                }
            }
        }

        public class EqualsOperatorTests
        {
            [Theory]
            [MemberData(nameof(ValidPostCodes), MemberType = typeof(PostCodeTests))]
            public void ObjectsShouldBeEqual(string value)
            {
                PostCode postCode1 = new PostCode(value);
                PostCode postCode2 = new PostCode(value);

                bool result = postCode1 == postCode2;

                result.Should().BeTrue();
            }

            [Theory]
            [MemberData(nameof(GenerateNonEqualPostCodes), MemberType = typeof(PostCodeTests))]
            public void ObjectsShouldNotBeEqual(string value1, string value2)
            {
                PostCode postCode1 = new PostCode(value1);
                PostCode postCode2 = new PostCode(value2);

                bool result = postCode1 == postCode2;

                result.Should().BeFalse();
            }
        }

        public class NotEqualsOperatorTests
        {
            [Theory]
            [MemberData(nameof(ValidPostCodes), MemberType = typeof(PostCodeTests))]
            public void ObectsShouldBeNotEqual(string value)
            {
                PostCode postCode1 = new PostCode(value);
                PostCode postCode2 = new PostCode(value);

                bool result = postCode1 != postCode2;

                result.Should().BeFalse();
            }

            [Theory]
            [MemberData(nameof(GenerateNonEqualPostCodes), MemberType = typeof(PostCodeTests))]
            public void ObjectsShouldNotBeEqual(string value1, string value2)
            {
                PostCode postCode1 = new PostCode(value1);
                PostCode postCode2 = new PostCode(value2);

                bool result = postCode1 != postCode2;

                result.Should().BeTrue();
            }
        }

        public class EqualsObjectTests
        {
            [Theory]
            [MemberData(nameof(ValidPostCodes), MemberType = typeof(PostCodeTests))]
            public void ShouldBeTrueWhenMatchingObjects(string value)
            {
                PostCode sut = new PostCode(value);
                object obj = new PostCode(value);

                bool result = sut.Equals(obj);

                result.Should().BeTrue();
            }

            [Theory]
            [MemberData(nameof(ValidPostCodes), MemberType = typeof(PostCodeTests))]
            public void ShouldBeFaleWhenNull(string value)
            {
                PostCode sut = new PostCode(value);

                bool result = sut.Equals(null);

                result.Should().BeFalse();
            }

            [Theory]
            [MemberData(nameof(ValidPostCodes), MemberType = typeof(PostCodeTests))]
            public void ShouldBeFaleWhenDifferentObject(string value)
            {
                PostCode sut = new PostCode(value);

                bool result = sut.Equals(new object());

                result.Should().BeFalse();
            }
        }

        public class StringOperatorTests
        {
            [Theory]
            [MemberData(nameof(ValidPostCodes), MemberType = typeof(PostCodeTests))]
            public void ObjectShouldActLikeAString(string value)
            {
                PostCode sut = new PostCode(value);

                sut.Should().Be(value);
            }
        }

        public class ToStringTests
        {
            [Theory]
            [MemberData(nameof(ValidPostCodes), MemberType = typeof(PostCodeTests))]
            public void ShouldReturnOriginalValue(string value)
            {
                PostCode sut = new PostCode(value);

                string result = sut.ToString();

                result.Should().Be(value);
            }
        }

        public class GetHashCodeTests
        {
            [Theory]
            [MemberData(nameof(ValidPostCodes), MemberType = typeof(PostCodeTests))]
            public void ShouldReturnOriginalValueHashCode(string value)
            {
                PostCode sut = new PostCode(value);

                int result = sut.GetHashCode();

                result.Should().Be(value.GetHashCode());
            }
        }
    }
}
