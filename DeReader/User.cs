namespace DeReader
{
    public class User
    {
        public User() {}

        public User(string userName, string password)
        {
            Name = userName;
            Password = password;
        }

        public string Name { get; private set; }
        public string Password { get; private set; }
    }
}