using System.Collections.Generic;

namespace BR.Core.Models
{
    public class IndexBeachEqualityComparer : IEqualityComparer<IndexBeach>
    {
        public bool Equals(IndexBeach x, IndexBeach y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(IndexBeach obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
