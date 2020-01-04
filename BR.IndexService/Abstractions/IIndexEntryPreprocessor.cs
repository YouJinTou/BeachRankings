using BR.Core.Models;
using System.Collections.Generic;

namespace BR.IndexService.Abstractions
{
    public interface IIndexEntryPreprocessor
    {
        IEnumerable<IndexEntry> PreprocessToken(IndexToken token, IndexBeach beach);
    }
}
