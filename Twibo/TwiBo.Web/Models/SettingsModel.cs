using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwiBo.Components;

namespace TwiBo.Web.Models
{
    public class SettingsModel
    {
        public string RowKey { get; set; }
        public string Name { get; set; }
        public string Accounts { get; set; }
        public string HashTags { get; set; }

        public ICollection<UserQuery> Queries { get; set; }
    }
}