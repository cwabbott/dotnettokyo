using DotNetTokyo.Test.Extensions;
using DotNetTokyo.Test.Infra;
using DotNetTokyo.Web.Controllers;
using DotNetTokyo.Web.Models;
using DotNetTokyo.Web.Services;
using FakeItEasy;
using KellermanSoftware.CompareNetObjects;
using Ploeh.AutoFixture;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace DotNetTokyo.Test.Functionals
{
    public class HomeControllerTest : TestController
    {
        private HomeController _homeController;
        private MeetupService _meetupSvc;
        private Fixture _fixture;

        public HomeControllerTest()
        {
            _meetupSvc = A.Fake<MeetupService>();
            _homeController = new HomeController(_meetupSvc);
            _homeController.ControllerContext = controllerContext;
            _fixture = new Fixture();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        public async Task ShouldReturnEvents(int eventCount)
        {
            // Arrange
            var upcomingEvents = _fixture.CreateMany<Event>(eventCount);
            A.CallTo(() => _meetupSvc.GetUpcomingEvents()).Returns(upcomingEvents);
            var expUpcomingEvents = upcomingEvents.DeepClone();

            // Act
            var result = await _homeController.Index();

            // Assert
            result.ShouldBeOfType<ViewResult>();
            var viewModel = (result as ViewResult).Model as IEnumerable<Event>;

            var compareLogic = new CompareLogic();
            compareLogic.Config.IgnoreObjectTypes = true;
            var compareResult = compareLogic.Compare(viewModel, expUpcomingEvents);
            compareResult.AreEqual.ShouldBe(true, compareResult.DifferencesString);
        }
    }
}
