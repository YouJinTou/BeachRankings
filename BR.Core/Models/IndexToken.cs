using BR.Core.Tools;
using System;

namespace BR.Core.Models
{
    public class IndexToken : IEquatable<IndexToken>
    {
        public IndexToken(string token, PlaceType type)
        {
            this.Token = token;
            this.Type = type;
        }

        public string Token { get; }

        public PlaceType Type { get; }

        public bool Equals(IndexToken other)
        {
            return this.Token == other.Token && this.Type == other.Type;
        }
    }
}
