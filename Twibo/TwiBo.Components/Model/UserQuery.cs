using System;
using Microsoft.WindowsAzure.StorageClient;

namespace TwiBo.Components
{
    public class UserQuery : TableServiceEntity
    {
        public static string GetPartitionKey(string identity)
        {
            return identity.Replace("\\", "-").Replace("/", "-").Replace("#", "-").Replace("?", "-");
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
