using BR.Core.Models;
using System.Collections.Generic;

namespace BR.IndexService.Processors
{
    public class IndexEntryEqualityComparer : IEqualityComparer<IndexEntry>
    {
        public bool Equals(IndexEntry x, IndexEntry y)
        {
            return x.ToString() == y.ToString();
        }

        public int GetHashCode(IndexEntry obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
