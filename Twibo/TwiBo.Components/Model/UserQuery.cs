using System;
using Microsoft.WindowsAzure.StorageClient;

namespace TwiBo.Components
{
    public class UserQuery : TableServiceEntity
    {
        public const string TableName = "UserQuery";

        public static string GetPartitionKey(string identityProvider, string name)
        {
            return identityProvider.Replace("\\", "-").Replace("/", "-").Replace("#", "-").Replace("?", "-") 
                + "-" + name.Replace("\\", "-").Replace("/", "-").Replace("#", "-").Replace("?", "-");
        }

        /// <summary>
        /// Space delimited.
        /// </summary>
        public string Accounts { get; set; }

        /// <summary>
        /// Space delimited.
        /// </summary>
        public string Hashtags { get; set; }
    }
}
