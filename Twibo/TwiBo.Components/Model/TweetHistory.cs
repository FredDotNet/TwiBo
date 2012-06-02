using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace TwiBo.Components.Model
{
    public class TweetHistory : TableServiceEntity
    {
        public const string TableName = "TweetHistory";

        public static string GetPartitionKey(string queryId)
        {
            return queryId.ToString().Replace("\\", "-").Replace("/", "-").Replace("#", "-").Replace("?", "-");
        }

        public string Author { get; set; }

        public DateTime Created { get; set; }

        public string Text { get; set; }

        public string TextAsHtml { get; set; }

        public bool Retweeted { get; set; }
    }

    public class TweetHistoryLimited : TableServiceEntity
    {
        public const string TableName = "TweetHistory";

        public static string GetPartitionKey(string queryId)
        {
            return queryId.ToString().Replace("\\", "-").Replace("/", "-").Replace("#", "-").Replace("?", "-");
        }

        public string Author { get; set; }

        public DateTime Created { get; set; }

        public string Text { get; set; }

        public string TextAsHtml { get; set; }
    }
}
