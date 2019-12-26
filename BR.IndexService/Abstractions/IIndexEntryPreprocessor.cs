using BR.Core.Models;
using System.Collections.Generic;

namespace BR.IndexService.Abstractions
{
    public interface IIndexEntryPreprocessor
    {
        IEnumerable<IndexEntry> PreprocessToken(string token, params string[] ids);

        IEnumerable<IndexEntry> PreprocessTokens(IEnumerable<string> tokens);
    }
}
