namespace DeReader
{
    public class Author
    {
        public Author(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }

        protected bool Equals(Author other)
        {
            bool @equals = string.Equals(Email, other.Email) && string.Equals(Name, other.Name);
            return @equals;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Author && Equals((Author) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Email != null ? Email.GetHashCode() : 0)*397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}