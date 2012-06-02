using System;
using System.Net;
using System.Linq;
using System.Threading;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using TwiBo.Components.Model;
using TwiBo.Components;
using System.Diagnostics;
using TweetSharp;

namespace TwiBo.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) => { configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)); });

            var userClient = new Components.TableStorageClient<User>(User.TableName);
            var userQueryClient = new Components.TableStorageClient<UserQuery>(UserQuery.TableName);

            while (true)
            {
                foreach (var user in userClient.CreateQuery())
                {
                    Trace.WriteLine(String.Format("Processing user: {0}", user.RowKey));

                    var userQueries = userQueryClient.CreateQuery().Where(o => o.PartitionKey == user.RowKey);
                    foreach (var userQuery in userQueries)
                    {
                        Trace.WriteLine(String.Format("  Processing query: {0} - {1}", userQuery.Accounts, userQuery.Hashtags));
                        SaveTweets(userQuery);

                    }
                }

                Thread.Sleep(1000);
            }
        }

        private void SaveTweets(UserQuery query)
        {
            if (!String.IsNullOrWhiteSpace(query.Accounts) && !String.IsNullOrWhiteSpace(query.Hashtags))
            {
                var tweetHistoryClient = new Components.TableStorageClient<TweetHistory>(TweetHistory.TableName);
                string users = "from:@" + query.Accounts.Replace(" ", " OR from:@");
                string hashtags = "#" + query.Hashtags.Replace(" ", " OR #");

                TwitterService service = new TwitterService();

                var tweetsearch = service.Search(users + " AND " + hashtags);

                int counter = 1;
                foreach (var tweet in tweetsearch.Statuses)
                {
                    Trace.WriteLine(String.Format("Processing message {0}: {1}", tweet.Id, tweet.Text));
                    tweetHistoryClient.Upsert(new TweetHistoryLimited
                    {
                        Author = tweet.Author.ScreenName,
                        Created = tweet.CreatedDate,
                        PartitionKey = TweetHistory.GetPartitionKey(query.RowKey),
                        RowKey = tweet.Id.ToString(),
                        Text = tweet.Text,
                        TextAsHtml = tweet.TextAsHtml
                    });

                    if (counter % 100 == 0)
                        tweetHistoryClient.SaveChanges();
                }

                tweetHistoryClient.SaveChanges();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
