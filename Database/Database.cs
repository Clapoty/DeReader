
using System;
using System.Collections.Generic;
using System.Linq;
using DeReader.Api;
using log4net;

namespace DeReader
{
    using MongoDB.Driver;
    public class DatabaseClient : IDatabaseClient
    {
        private readonly MongoDatabase _database;
        private readonly MongoCollection<Article> _collection;
        private readonly ILog _logger = LogManager.GetLogger(typeof(DatabaseClient));

        public DatabaseClient(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            _database = server.GetDatabase("Test");

            _collection = _database.GetCollection<Article>("ArticleCollection", new MongoCollectionSettings());
        }

        public void InsertUser(string name, string password)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException(name);
            if (string.IsNullOrEmpty(password)) throw new ArgumentException(password);

            var mongoUser = new MongoUser(name, new PasswordEvidence(password), false);
            _database.AddUser(mongoUser);
        }

        public User GetUser(string name)
        {
            var user = _database.FindUser(name);
            
            return user != null 
                ? new User(user.Username, user.PasswordHash) 
                : null;
        }

        public void InsertArticle(Article article)
        {
            WriteConcernResult writeConsernResult = _collection.Insert(article);
            
            if (!writeConsernResult.Ok)
            {
                throw new ApplicationException(string.Format("Cannot insert article because {0}", writeConsernResult.Response));
            }
        }

        public IEnumerable<Article> GetArticles(string uri)
        {
            try
            {
                var query = new QueryDocument("Source", uri);
                return _collection.Find(query).ToList();
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return new Article[0];
            }
        }

        public void Dispose()
        {
        }
    }
}
