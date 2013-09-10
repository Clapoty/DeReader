
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DeReader.UnitTests
{
    [TestFixture]
    public class DatabaseClientTests
    {
        private DatabaseClient _database;
        
        [SetUp]
        public void Init()
        {
            _database = new DatabaseClient("mongodb://localhost");
        }

        [TearDown]
        public void Stop()
        {
            _database.Dispose();
        }

        [Test]
        public void ReturnsNullWhenUserIsNotFound()
        {
            var result = _database.GetUser("Rotten");
            Assert.IsNull(result);
        }

        [Test]
        public void TestCanInsertUser()
        {
            _database.InsertUser("Adina", "melody");
            User user = _database.GetUser("Adina");
            Assert.AreEqual("Adina", user.Name);
        }

        [Test]
        public void TestCanInsertArticle()
        {
            string id1 = "Id1" + Guid.NewGuid();
            string id2 = "Id2" + Guid.NewGuid();
            string source = "CassandraUri" + Guid.NewGuid();
            List<Author> authors = new List<Author>() { { new Author("Adi", "adi@gmail.fr") }, { new Author("Francky", "Francky@gmail.fr") } };
            
            Article article1 = new Article(id1, source, "Learn Cassandra For Dummies", "Cassandra in Depth",
                "Learn to use and setup a NoSQL database", authors, (new DateTime(2013, 3, 4)), (new DateTime(2012, 12, 10)), new Content("blablablablabla", "html"));

            Article article2 = new Article(id2, source, "Cassandra Cookbook", "Cassandra Cookbook",
               "Configure a NoSQL database for advanced users", authors, (new DateTime(2013, 5, 15)), (new DateTime(2013, 2, 3)), new Content("blablablablabla2", "html"));

            _database.InsertArticle(article1);
            _database.InsertArticle(article2);

            Article[] results = null;
            Assert.DoesNotThrow(() => results =_database.GetArticles(source).ToArray());
            
            Assert.AreEqual(2, results.Length);
            Assert.AreEqual(article1, results[0]);
            Assert.AreEqual(article2, results[1]);
        }
    }
}
