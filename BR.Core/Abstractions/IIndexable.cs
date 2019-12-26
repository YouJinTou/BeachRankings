using System.Collections.Generic;

namespace BR.Core.Abstractions
{
    public interface IIndexable
    {
        IEnumerable<string> GetTokens();
    }
}
