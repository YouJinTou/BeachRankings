using BR.Core.Models;
using System.Collections.Generic;

namespace BR.IndexService.Abstractions
{
    public interface IIndexEntryPreprocessor
    {
        IEnumerable<IndexEntry> PreprocessToken(string token);

        IEnumerable<IndexEntry> PreprocessTokens(IEnumerable<string> tokens);
    }
}
