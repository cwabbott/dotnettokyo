using DotNetTokyo.Web.Helpers;
using System;
using System.Threading;
using System.Web.Mvc;

namespace DotNetTokyo.Web.Controllers
{
    public class LocalizedController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            var routeCulture = (string)RouteData.Values["culture"];
            if (routeCulture == "undefined")
            {
                // redirect to the correct route
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0
                    ? Request.UserLanguages[0]
                    : // obtain it from HTTP header AcceptLanguages
                    null;

                cultureName = CultureHelper.GetImplementedCulture(cultureName); // got a valid culture

                // redirect
                RouteData.Values["culture"] = cultureName;

                Response.RedirectToRoute(RouteData.Values);
            }
            else
            {
                // set the current page culture
                cultureName = routeCulture;
            }

            // Attempt to read the culture cookie from Request

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures           
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }
    }
}
