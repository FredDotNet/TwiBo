using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwiBo.Web.Models;
using TwiBo.Components;
using TwiBo.Web.Tools;
using TwiBo.Components.Model;

namespace TwiBo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string partitionKey;
        private TableStorageClient<TweetHistory> tweetStorageClient;
        private TableStorageClient<UserQuery> queryStorageClient;
        public HomeController()
        {
            var identity = AcsIdentity.TryGet();
            partitionKey= UserQuery.GetPartitionKey(identity.IdentityProvider, identity.Name);
            tweetStorageClient = new TableStorageClient<TweetHistory>("TweetHistory");
            queryStorageClient = new TableStorageClient<UserQuery>("UserQuery");
        }

        //GET /Dashboard/
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Twibo!";

            var models = new List<DashboardModel>();

            var queries = queryStorageClient.CreateQuery().Where(q => q.PartitionKey == partitionKey).ToList();
            foreach (var query in queries)
            {
                var tweets = tweetStorageClient.CreateQuery().Where(q => q.PartitionKey == query.RowKey).ToList();
                models.Add(new DashboardModel()
                {
                    Name = query.Name,
                    TweetHistory = tweets,
                });
            }

            return View(models);
        }

        //Get /Dashboard/About
        public ActionResult About()
        {
            return View();
        }
    }
}
