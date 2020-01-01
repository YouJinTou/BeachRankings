using BR.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace BR.Core.Models
{
    public class IndexPosting : IEquatable<IndexPosting>, IDbModel
    {
        public string Place { get; set; }

        public IEnumerable<string> BeachIds { get; set; }

        public string Type { get; set; }

        public bool Equals(IndexPosting other)
        {
            return this.Place == other.Place && this.Type == other.Type;
        }

        public override string ToString()
        {
            return $"{this.Place}|{this.Type}";
        }
    }
}
