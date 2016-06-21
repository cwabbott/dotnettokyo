using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using DotNetTokyo.Web.Helpers;
using DotNetTokyo.Web.Services;

namespace DotNetTokyo.Web.Controllers
{
    public class HomeController : LocalizedController
    {
        private IMeetupService meetupService;

        public HomeController(IMeetupService meetupService)
        {
            this.meetupService = meetupService;
        }

        [OutputCache(VaryByCustom = "url", Duration = 3600)]
        public async Task<ActionResult> Index()
        {
            return View(await meetupService.GetUpcomingEvents());
        }
    }
}