using NUnit.Framework;

namespace DeReader.UnitTests
{
    [TestFixture]
    public class RssFetcher
    {
        [Test]
        public void CanGetRssStreamFromAtom10Stream()
        {
            var fetcher = new DeReader.RssFetcher();
            var articles = fetcher.Fetch("http://feeds.feedburner.com/codingly");
            Assert.IsTrue(articles.Count > 0);
        }

        [Test]
        public void CanGetRssStreamFromRss2Stream()
        {
            var fetcher = new DeReader.RssFetcher();
            var articles = fetcher.Fetch("http://rss.lemonde.fr/c/205/f/3050/index.rss");
            Assert.IsTrue(articles.Count > 0);
        }
    }
}
