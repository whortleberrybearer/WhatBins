
namespace WhatBins.Types
{
    public struct PostCode
    {
        private readonly string value;

        public PostCode(string value)
        {
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
