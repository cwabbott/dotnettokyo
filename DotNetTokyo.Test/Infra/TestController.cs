using FakeItEasy;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DotNetTokyo.Test.Infra
{
    public class TestController
    {
        protected ControllerContext controllerContext;
        protected NameValueCollection queryStringCollection = new NameValueCollection();

        public TestController()
        {
            controllerContext = CreateFakeControllerContext();
        }

        private ControllerContext CreateFakeControllerContext()
        {
            // Fake request
            var request = A.Fake<HttpRequestBase>();
            A.CallTo(() => request.Headers).Returns(new NameValueCollection());
            A.CallTo(() => request.QueryString).Returns(queryStringCollection);

            // Fake response
            var response = A.Fake<HttpResponseBase>();

            // Fake cookies
            var cookieCollection = new HttpCookieCollection();
            A.CallTo(() => request.Cookies).Returns(cookieCollection);
            A.CallTo(() => response.Cookies).Returns(cookieCollection);

            // Fake http context
            var httpContext = A.Fake<HttpContextBase>();
            A.CallTo(() => httpContext.Request).Returns(request);
            A.CallTo(() => httpContext.Response).Returns(response);

            // Fake controller context
            return
              new ControllerContext(httpContext, new RouteData(), A.Fake<ControllerBase>());
        }
    }
}
