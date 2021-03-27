namespace WhatBins.Types
{
    using System;
    using System.Text.RegularExpressions;

    public struct PostCode
    {
        private static readonly Regex postCodeRegex = new Regex("([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\\s?[0-9][A-Za-z]{2})");
        private readonly string value;

        public PostCode(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!postCodeRegex.IsMatch(value))
            {
                throw new ArgumentException("Invalid postcode", nameof(value));
            }

            this.value = value;

            string[] parts = this.value.Split(' ');

            this.Outcode = parts[0];
            this.Incode = parts[1];
        }

        public static implicit operator string(PostCode postCode) => postCode.value;

        public static bool operator ==(PostCode left, PostCode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PostCode left, PostCode right)
        {
            return !(left == right);
        }


        public string Outcode { get; }

        public string Incode { get; }

        public override bool Equals(object? obj)
        {
            if (obj is PostCode postCode)
            {
                return this.Equals(postCode);
            }

            return this.value.Equals(obj);
        }

        public bool Equals(PostCode other)
        {
            return this.value.Equals(other.value);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
