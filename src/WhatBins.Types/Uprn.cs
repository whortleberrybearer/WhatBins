
namespace WhatBins.Types
{
    public struct Uprn
    {
        private readonly string value;

        public Uprn(string value)
        {
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
