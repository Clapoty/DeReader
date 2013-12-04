using System;
using System.Collections.Generic;

namespace DeReader.Api
{
    public interface IDatabaseClient : IDisposable
    {
        void InsertUser(string name, string password);
        User GetUser(string name);

        void InsertArticle(Article article);
        IEnumerable<Article> GetArticles(string uri);
    }
}