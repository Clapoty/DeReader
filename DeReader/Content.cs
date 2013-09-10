namespace DeReader
{
    public class Content
    {
        public Content(string text, string type)
        {
            Type = type;
            Text = text;
        }

        public string Text { get; private set; }
        public string Type { get; private set; }

        private bool Equals(Content content)
        {
            return string.Equals(content.Text, Text) && string.Equals(content.Type, Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var content = obj as Content;
            return content != null && Equals(content);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Text != null ? Text.GetHashCode() : 0)*397 ^ (Type != null ? Text.GetHashCode() : 0);
            }
        }
    }
}