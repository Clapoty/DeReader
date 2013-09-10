using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using log4net;

namespace DeReader
{
    public class Hub : NancyModule
    {
        private readonly IDatabaseClient _database;
        private readonly ILog _logger = LogManager.GetLogger(typeof (Hub));

        public Hub(IDatabaseClient database)
        {
            _database = database;

            Get["/user/{name}"] = parameters =>
                {
                    //TODO : see LoginAndRedirect in Nancy.Authentication.Forms 
                    try
                    {
                        dynamic createdUser = database.GetUser(parameters.name);
                        _logger.InfoFormat("User get : {0}", createdUser.Name);
                        return string.Format("user asked is {0} and password {1}", createdUser.Name,
                                             createdUser.Password);
                    }
                    catch (Exception e)
                    {
                        return string.Format("Error caught : {0}", e.Message);
                    }
                };


            Post["/newuser/"] = ctx =>
                {
                    dynamic name = Request.Form.Name.Value;
                    dynamic password = Request.Form.Password.Value;

                    _database.InsertUser(name, password);
                    dynamic message = string.Format("User added : {0}", name);
                    _logger.InfoFormat(message);

                    return message;
                };

            Post["/user/stream/subscription/"] = parameters =>
                {
                    string name = Request.Form.Name.Value;
                    string stream = Request.Form.Stream.Value;
                    _logger.InfoFormat("Try to add stream : {0} for user : {1}", stream, name);
                    string message;
                    try
                    {
                        string source = parameters.stream;
                        User user = database.GetUser(parameters.name);
                        // TODO : does the user have rights to do that operation ?
                        if (user == null)
                        {
                            return Response.AsRedirect("/error/unknownuser/");
                        }

                        _logger.DebugFormat("Data fetched for user : {0}", user.Name);

                        var fetcher = new RssFetcher();

                        // fetch all articles from source
                        Dictionary<string, Article> fetchedArticles = fetcher.Fetch(source);

                        _logger.DebugFormat("Fetched articles : {0}", fetchedArticles.Count);

                        // get all articles already inserted into database
                        IEnumerable<Article> articles = database.GetArticles(source);

                        var articlesArray = articles as Article[] ?? articles.ToArray();
                        _logger.DebugFormat("Fetched articles from database : {0}", articlesArray.Count());

                        string[] articleIds = articlesArray.Select(a => a.Id).ToArray();

                        // compare source articles ids and fetched ids values 
                        foreach (var article in fetchedArticles)
                        {
                            if (articles != null && !articleIds.Contains(article.Value.Id))
                            {
                                _logger.DebugFormat("Inserting article {0}", article.Value);
                                // insert new article
                                database.InsertArticle(article.Value);
                            }
                        }

                        message = string.Format("Stream added : {0} for user : {1}", parameters.stream, parameters.name);
                        _logger.InfoFormat(message);
                    }
                    catch (Exception e)
                    {
                        return string.Format("Error caught : {0}", e.Message);
                    }

                    return message;
                };
        }
    }
}