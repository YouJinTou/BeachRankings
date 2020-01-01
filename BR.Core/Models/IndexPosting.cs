using BR.Core.Abstractions;
using System.Collections.Generic;

namespace BR.Core.Models
{
    public class IndexPosting : IDbModel
    {
        public string Place { get; set; }

        public IEnumerable<string> BeachIds { get; set; }

        public string Type { get; set; }

        public override string ToString()
        {
            return $"{this.Place}|{this.Type}";
        }
    }
}
