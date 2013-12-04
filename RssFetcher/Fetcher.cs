using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using DeReader.Api;

namespace RssFetcher
{
    public class Fetcher : IRssFetcher
    {
        public Dictionary<string, Article> Fetch(string uri)
        {
            SyndicationFeed feed;
            using (var reader = XmlReader.Create(uri))
            {
                feed = SyndicationFeed.Load(reader);
            }

            if (feed == null)
            {
                return null;
            }

            var articles = new Dictionary<string, Article>();
            
            IObservable<SyndicationItem> obs = feed.Items.ToObservable();
            using (obs.Subscribe(x => AddItemTo(GetUriArticlePair(uri, x), articles)))
            {
                return articles;
            }
        }

        private void AddItemTo(KeyValuePair<string, Article> getUriArticlePair, Dictionary<string, Article> articles)
        {
            articles.Add(getUriArticlePair.Key, getUriArticlePair.Value);
        }

        private static KeyValuePair<string, Article> GetUriArticlePair(string uri, SyndicationItem item)
        {
            string subject = item.Title.Text;
            string summary = item.Summary.Text;
            var authors = new List<Author>(item.Authors.Count);
            authors.AddRange(item.Authors.Select(author => new Author(author.Name, author.Email)));

            string articleContent = "";
            foreach (SyndicationElementExtension ext in item.ElementExtensions)
            {
                if (ext.GetObject<XElement>().Name.LocalName == "encoded")
                {
                    articleContent = ext.GetObject<XElement>().Value;
                }
            }

            var content = new Content(articleContent, "Html");
            DateTime lastUpdateTime = item.LastUpdatedTime.DateTime;
            DateTime publishDate = item.PublishDate.DateTime;

            var article = new Article(item.Id, uri, subject, item.Title.Text, summary, authors, lastUpdateTime, publishDate,
                                      content);
            var key = string.Format("{0}.{1}", uri, item.Id);

            return new KeyValuePair<string, Article>(key, article);
        }
    }
}
