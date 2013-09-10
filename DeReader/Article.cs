using System;
using System.Collections.Generic;

namespace DeReader
{
    public sealed class Article
    {
        public Article(string id, string source, string subject, string title, string summary, List<Author> authors, DateTime lastUpdateTime, DateTime publishDate, Content content)
        {
            Id = id;
            Source = source;
            Title = title;
            Authors = authors;
            Content = content;
            Summary = summary;
            Subject = subject;
            PublishDate = publishDate.ToUniversalTime();
            LastUpdateTime = lastUpdateTime.ToUniversalTime();
        }

        public string Id { get; private set; }
        public string Source { get; private set; }
        public string Subject { get; private set; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public List<Author> Authors { get; private set; }
        public DateTime LastUpdateTime { get; private set; }
        public DateTime PublishDate { get; private set; }
        public Content Content { get; private set; }

        private bool Equals(Article other)
        {
            bool authorsEquals = Authors.Count == other.Authors.Count;
            if (!authorsEquals) return false;

            for(int i=0; i<Authors.Count; i++)
            {
                authorsEquals = Authors[i].Equals(other.Authors[i]);
                if (!authorsEquals) return false;
            }

            return string.Equals(Id, other.Id) && string.Equals(Source, other.Source) && string.Equals(Subject, other.Subject)
                && string.Equals(Title, other.Title) && PublishDate.Equals(other.PublishDate) && LastUpdateTime.Equals(other.LastUpdateTime) 
                && Content.Equals(other.Content) && authorsEquals && string.Equals(Summary, other.Summary);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Article && Equals((Article)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Subject != null ? Subject.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Source != null ? Source.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ PublishDate.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format("Id: {0}\n Source:{5}\n Title: {1}\n Subject: {2}\n Summary: {3}\n Content: {4}\n", Id, Title, Subject, Summary, Content, Source);
        }

    }
}