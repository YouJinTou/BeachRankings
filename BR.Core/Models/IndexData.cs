using System.Collections.Generic;

namespace BR.Core.Models
{
    public class IndexData
    {
        public IEnumerable<string> Ids { get; set; }

        public IEnumerable<IndexToken> Tokens { get; set; }
    }
}
