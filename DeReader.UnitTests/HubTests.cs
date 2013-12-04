using DeReader.Api;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;
using Moq;

namespace DeReader.UnitTests
{
    public class HubTests
    {
        [Test]
        public void TestCanInsertNewUser()
        {
            const string name = "Adina";
            const string password = "Password2BeCh@nged";
            
            // instanciate database mock
            var database = new Mock<IDatabaseClient>();
            database.Setup(e => e.InsertUser(name, password));

            // create testing browser
            var browser = new Browser(with => with.Module(new Hub(database.Object)));
            var response = browser.Post("/newuser/", with =>
            {
                with.HttpRequest();
                with.FormValue("Name", name); // post values into the http message
                with.FormValue("Password", password);
            });

            // verifications 
            database.VerifyAll();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void TestCanGetUser()
        {
            const string name = "Adina";
            const string password = "Password2BeCh@nged";

            // instanciate database mock
            var database = new Mock<IDatabaseClient>();
            database.Setup(e => e.GetUser(name)).Returns(new User(name, password));

            // create testing browser
            var browser = new Browser(with => with.Module(new Hub(database.Object)));
            string uri = string.Format("/user/{0}", name);

            // action : get user from database
            var response = browser.Get(uri, with => with.HttpRequest());
            
            // verifications
            var expectedReturnMessage = string.Format("User added : {0}", name);
            Assert.IsNotNull(expectedReturnMessage, response.Body.AsString());
            database.VerifyAll();
        }

        [Test]
        public void TestCannotAddStreamForUnknownUser()
        {
            const string user = "NotExistingUser";
            var database = new Mock<IDatabaseClient>();
            database.Setup(e => e.GetUser(user)); // return null

            var browser = new Browser(with => with.Module(new Hub(database.Object)));
            var response = browser.Post("/user/stream/subscription/", with =>
            {
                with.HttpRequest();
                with.FormValue("Name", user);
                with.FormValue("Stream", "https://news.ycombinator.com/rss");
            });

            response.ShouldHaveRedirectedTo("/error/unknownuser/");
        }

        [Test]
        public void TestCanAddStreamForSubscriber()
        {
            var database = new Mock<IDatabaseClient>();

            var browser = new Browser(with => with.Module(new Hub(database.Object)));
            browser.Post("/user/stream/subscription/", with =>
                {
                    with.HttpRequest();
                    with.FormValue("Name", "KnwownUser");
                    with.FormValue("Stream", "https://news.ycombinator.com/rss");
                });
        }
    }
}

