using System.Collections.Generic;

namespace DeReader
{
    public interface IRssFetcher
    {
        Dictionary<string, Article> Fetch(string uri);
    }
}