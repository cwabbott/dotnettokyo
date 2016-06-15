using DotNetTokyo.Web.Services;
using System.Threading.Tasks;
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

        public async Task<ActionResult> Index()
        {
            return View(await meetupService.GetUpcomingEvents());
        }
    }
}