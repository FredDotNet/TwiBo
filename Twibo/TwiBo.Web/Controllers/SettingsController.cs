using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwiBo.Components;
using TwiBo.Web.Models;
using TwiBo.Web.Tools;

namespace TwiBo.Web.Controllers
{
    public class SettingsController : Controller
    {
        private readonly string partitionKey;
        private TableStorageClient<UserQuery> storageClient;
        public SettingsController()
        {
            var identity = AcsIdentity.TryGet();
            partitionKey= UserQuery.GetPartitionKey(identity.IdentityProvider, identity.Name);
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

        public ActionResult Delete(string id)
        {
            var queryToDelete = storageClient.CreateQuery().ToList().Single(q => q.RowKey == id);
            var model = new Models.SettingsModel()
            {
                Accounts = queryToDelete.Accounts,
                Name = queryToDelete.Name,
                HashTags = queryToDelete.Hashtags,
                RowKey = queryToDelete.RowKey,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(SettingsModel model)
        {
            var queryToDelete = storageClient.CreateQuery().ToList().Single(q => q.RowKey == model.RowKey);
            storageClient.Delete(queryToDelete);
            storageClient.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
