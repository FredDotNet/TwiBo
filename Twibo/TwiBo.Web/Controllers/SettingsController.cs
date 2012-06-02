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
        private const string partitionKey = "koen";
        private TableStorageClient<UserQuery> storageClient;
        public SettingsController()
        {
            storageClient= new TableStorageClient<UserQuery>("UserQuery");
        }

        //
        // GET: /Settings/

        public ActionResult Index()
        {
            var queries = storageClient.CreateQuery().Where(q => q.PartitionKey == partitionKey).ToList();
            var model = new SettingsModel()
            {
                Queries = queries,
            };

            return View(model);
        }


        [HttpPost]
        public ActionResult Index(SettingsModel model)
        {
            storageClient.Insert(new UserQuery()
            {
                Name = model.Name,
                Accounts = model.Accounts,
                Hashtags = model.HashTags,
                PartitionKey = partitionKey,
                RowKey = Guid.NewGuid().ToString(),
            });
            storageClient.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var model = new SettingsModel();

            return View(model);
        }
    }
}
