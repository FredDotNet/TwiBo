using System;

using Microsoft.WindowsAzure.StorageClient;

namespace TwiBo.Components.Model
{
    public class User : TableServiceEntity
    {
        public const string TableName = "User";

        public static string GetPartitionKey()
        {
            return "User";
        }

        public static string GetRowKey(string identityProvider, string name)
        {
            return identityProvider.Replace("\\", "-").Replace("/", "-").Replace("#", "-").Replace("?", "-")
                + "-" + name.Replace("\\", "-").Replace("/", "-").Replace("#", "-").Replace("?", "-");
        }
    }
}
