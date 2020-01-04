using BR.Core.Models;
using System.Collections.Generic;

namespace BR.Core.Abstractions
{
    public interface IIndexEntryPreprocessor
    {
        IEnumerable<IndexEntry> PreprocessToken(IndexToken token, IndexBeach beach);
    }
}
