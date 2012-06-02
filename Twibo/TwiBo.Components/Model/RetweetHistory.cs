using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace TwiBo.Components.Model
{
    public class RetweetHistory : TableServiceEntity
    {
        public const string TableName = "RetweetHistory";

        public static string GetPartitionKey(string user)
        {
            return user.ToString().Replace("\\", "-").Replace("/", "-").Replace("#", "-").Replace("?", "-");
        }
    }
}
