using DotNetTokyo.Web.Services;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;

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