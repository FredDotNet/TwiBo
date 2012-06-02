using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwiBo.Components;
using TwiBo.Web.Models;

namespace TwiBo.Web.Controllers
{
    public class SettingsController : Controller
    {
        //
        // GET: /Settings/

        public ActionResult Index()
        {
            var model = new SettingsModel()
            {
                Queries = new List<UserQuery>{ new UserQuery{ Accounts = "test 123 34", Hashtags = "WAS12"}}
            };

            return View(model);
        }


        [HttpPost]
        public ActionResult Index(SettingsModel model)
        {
            var storageClient = new TableStorageClient<UserQuery>("UserQuery");
            storageClient.Insert(new UserQuery()
            {
                Accounts = model.Accounts,
                Hashtags = model.HashTags,
                PartitionKey = "koen",
                RowKey = Guid.NewGuid().ToString(),
            });
            storageClient.SaveChanges();

            

            return RedirectToAction("Index");
        }
    }
}
