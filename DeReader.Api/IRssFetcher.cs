using System.Collections.Generic;

namespace DeReader.Api
{
    public interface IRssFetcher
    {
        Dictionary<string, Article> Fetch(string uri);
    }
}