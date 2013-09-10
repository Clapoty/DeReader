using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;

namespace DeReader
{
    public class RssFetcher : IRssFetcher
    {
        public Dictionary<string, Article> Fetch(string uri)
        {
            var xmlReader = XmlReader.Create(uri);
            SyndicationFeed feed = SyndicationFeed.Load(xmlReader);
            xmlReader.Close();

            if (feed == null)
            {
                return null;
            }

            var articles = new Dictionary<string, Article>();

            foreach (var item in feed.Items)
            {
                string subject = item.Title.Text;
                string summary = item.Summary.Text;
                List<Author> authors = new List<Author>(item.Authors.Count);
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

                var article = new Article(item.Id, uri, subject, item.Title.Text, summary, authors, lastUpdateTime, publishDate, content);
                string key = string.Format("{0}.{1}", uri, item.Id);
                articles.Add(key, article);
            }
            return articles;
        }
    }
}
