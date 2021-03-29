namespace WhatBins.Types
{
    using System;
    using System.Text.RegularExpressions;

    public struct Uprn
    {
        private static readonly Regex UprnRegex = new Regex("UPRN\\d+");
        private readonly string value;

        public Uprn(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!UprnRegex.IsMatch(value))
            {
                throw new ArgumentException("Invalid UPRN", nameof(value));
            }

            this.value = value;
        }

        public static implicit operator string(Uprn uprn) => uprn.value;

        public static bool operator ==(Uprn left, Uprn right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Uprn left, Uprn right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Uprn uprn)
            {
                return this.Equals(uprn);
            }

            return this.value.Equals(obj);
        }

        public bool Equals(Uprn other)
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
