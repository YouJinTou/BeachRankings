using BR.Core.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BR.Seed
{
    internal class IndexTokenEqualityComparer : IEqualityComparer<IndexToken>
    {
        public bool Equals([AllowNull] IndexToken x, [AllowNull] IndexToken y)
        {
            return x.Token.Equals(y.Token);
        }

        public int GetHashCode([DisallowNull] IndexToken obj)
        {
            unchecked
            {
                return obj.Token.GetHashCode() + obj.Type.GetHashCode();
            }
        }
    }
}
