using BR.Core.Abstractions;
using System.Collections.Generic;

namespace BR.Core.Models
{
    public class IndexPosting : IDbModel
    {
        public string Id { get; set; }

        public string Place { get; set; }

        public IEnumerable<IndexBeach> Beaches { get; set; }

        public string Type { get; set; }

        public override string ToString()
        {
            return $"{this.Id}_{this.Place}_{this.Type}";
        }
    }
}
