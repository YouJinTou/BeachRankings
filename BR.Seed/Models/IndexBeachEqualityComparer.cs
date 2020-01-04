using BR.Core.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BR.Seed.Models
{
    internal class IndexBeachEqualityComparer : IEqualityComparer<IndexBeach>
    {
        public bool Equals([AllowNull] IndexBeach x, [AllowNull] IndexBeach y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode([DisallowNull] IndexBeach obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
