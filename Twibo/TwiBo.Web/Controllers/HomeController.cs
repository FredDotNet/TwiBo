using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwiBo.Web.Models;
using TwiBo.Components;

namespace TwiBo.Web.Controllers
{
    public class HomeController : Controller
    {
        //GET /Dashboard/
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        //Get /Dashboard/About
        public ActionResult About()
        {
            return View();
        }
    }
}
