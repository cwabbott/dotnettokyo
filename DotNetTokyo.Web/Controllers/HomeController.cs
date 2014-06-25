using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetTokyo.Web.Helpers;

namespace DotNetTokyo.Web.Controllers
{
    public class HomeController : LocalizedController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}