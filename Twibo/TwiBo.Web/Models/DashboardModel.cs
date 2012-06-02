using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwiBo.Components.Model;

namespace TwiBo.Web.Models
{
    public class DashboardModel
    {
        public string Name { get; set; }
        public ICollection<TweetHistory> TweetHistory { get; set; }
    }
}