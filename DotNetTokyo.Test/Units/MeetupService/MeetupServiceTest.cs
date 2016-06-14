using DotNetTokyo.Test.Infra;
using DotNetTokyo.Web.Models;
using DotNetTokyo.Web.Services;
using FakeItEasy;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DotNetTokyo.Test.Units
{
    public class MeetupServiceTest
    {
        [Theory]
        [MemberData("NoEvent")]
        [MemberData("OneEvent")]
        [MemberData("MultipleEvents")]
        public async Task GetUpcomingEvents_ShouldReturnEvents(string eventJson)
        {
            // Arrange
            var apiDomain = "api.domain.com";
            var eventGroupName = "event-group-name";
            var apiMethod = "events";

            var meetupConfig = A.Fake<IMeetupConfiguration>();
            A.CallTo(() => meetupConfig.ApiDomain).Returns(apiDomain);
            A.CallTo(() => meetupConfig.EventGroupName).Returns(eventGroupName);

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddFakeResponse(
                new Uri(string.Format("http://{0}/{1}/{2}?scroll=next_upcoming", apiDomain, eventGroupName, apiMethod)),
                new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent(eventJson)
                }
              );

            // Act
            var httpClient = new HttpClient(fakeResponseHandler);
            var meetupSvc = new MeetupService(meetupConfig, httpClient);
            var upcomingEvents = await meetupSvc.GetUpcomingEvents();

            var expUpcomingEvents = CreateExpectedEvents(eventJson);
            var compareLogic = new CompareLogic();
            var compareResult = compareLogic.Compare(upcomingEvents, expUpcomingEvents);
            compareResult.AreEqual.ShouldBe(true, compareResult.DifferencesString);
        }

        public static IEnumerable<object[]> NoEvent 
        {
            get { yield return new object[] { "{ }" }; }
        }

        public static IEnumerable<object[]> OneEvent
        {
            get { yield return new object[] { "{  \"0\": {    \"created\": 1465281615000,    \"id\": \"231709906\",    \"name\": \"Tokyo .NET Developers Meetup #12 - 2016-6-21\",    \"status\": \"upcoming\",    \"time\": 1466503200000,    \"updated\": 1465281615000,    \"utc_offset\": 32400000,    \"waitlist_count\": 0,    \"yes_rsvp_count\": 7,    \"venue\": {      \"id\": 23880758,      \"name\": \"JP Tower 29F, Medidata Office\",      \"lat\": 0,      \"lon\": 0,      \"repinned\": false,      \"address_1\": \"2-7-2 Marunouchi, Chiyoda, Tokyo 100-0005\",      \"city\": \"Tokyo\",      \"country\": \"jp\",      \"localized_country_name\": \"Japan\"    },    \"group\": {      \"created\": 1433397718000,      \"name\": \"Tokyo .NET Developers Meetup\",      \"id\": 18647945,      \"join_mode\": \"open\",      \"lat\": 35.66999816894531,      \"lon\": 139.77000427246094,      \"urlname\": \"Tokyo-NET-Developers-Meetup\",      \"who\": \"Engineers\"    },    \"link\": \"http://www.meetup.com/Tokyo-NET-Developers-Meetup/events/231709906/\",    \"description\": \"<p>Tentative schedule:</p> <p><br/>19:00 Doors Open</p> <p>- food, drinks, networking</p> <p>19:30 Presentations (starting presentations earlier)</p> <p>- TBD call for presenters!</p> <p>20:50 wrap up </p> <p>\n\nWe'll fit in two or three ~10 minute presentations and leave about 45 minutes at the end for discussion, chit-chat, networking</p> \",    \"visibility\": \"public\"  }}" }; }
        }

        public static IEnumerable<object[]> MultipleEvents
        {
            get { yield return new object[] { "{  \"0\": {    \"created\": 1462787056000,    \"id\": \"230984712\",    \"name\": \"Tokyo .NET Developers Meetup #11 - 2016-5-17\",    \"status\": \"past\",    \"time\": 1463479200000,    \"updated\": 1463490952000,    \"utc_offset\": 32400000,    \"waitlist_count\": 0,    \"yes_rsvp_count\": 17,    \"venue\": {      \"id\": 23880758,      \"name\": \"JP Tower 29F, Medidata Office\",      \"lat\": 0,      \"lon\": 0,      \"repinned\": false,      \"address_1\": \"2-7-2 Marunouchi, Chiyoda, Tokyo 100-0005\",      \"city\": \"Tokyo\",      \"country\": \"jp\",      \"localized_country_name\": \"Japan\"    },    \"group\": {      \"created\": 1433397718000,      \"name\": \"Tokyo .NET Developers Meetup\",      \"id\": 18647945,      \"join_mode\": \"open\",      \"lat\": 35.66999816894531,      \"lon\": 139.77000427246094,      \"urlname\": \"Tokyo-NET-Developers-Meetup\",      \"who\": \"Engineers\"    },    \"link\": \"http://www.meetup.com/Tokyo-NET-Developers-Meetup/events/230984712/\",    \"description\": \"<p>Tentative schedule:</p> <p><br/>19:00 Doors Open</p> <p>- food, drinks, networking</p> <p>19:30 Presentations (starting presentations earlier)</p> <p>- Bot Framework</p> <p>- </p> <p>20:50 wrap up  </p> \",    \"visibility\": \"public\"  },  \"1\": {    \"created\": 1465281615000,    \"id\": \"231709906\",    \"name\": \"Tokyo .NET Developers Meetup #12 - 2016-6-21\",    \"status\": \"upcoming\",    \"time\": 1466503200000,    \"updated\": 1465281615000,    \"utc_offset\": 32400000,    \"waitlist_count\": 0,    \"yes_rsvp_count\": 7,    \"venue\": {      \"id\": 23880758,      \"name\": \"JP Tower 29F, Medidata Office\",      \"lat\": 0,      \"lon\": 0,      \"repinned\": false,      \"address_1\": \"2-7-2 Marunouchi, Chiyoda, Tokyo 100-0005\",      \"city\": \"Tokyo\",      \"country\": \"jp\",      \"localized_country_name\": \"Japan\"    },    \"group\": {      \"created\": 1433397718000,      \"name\": \"Tokyo .NET Developers Meetup\",      \"id\": 18647945,      \"join_mode\": \"open\",      \"lat\": 35.66999816894531,      \"lon\": 139.77000427246094,      \"urlname\": \"Tokyo-NET-Developers-Meetup\",      \"who\": \"Engineers\"    },    \"link\": \"http://www.meetup.com/Tokyo-NET-Developers-Meetup/events/231709906/\",    \"description\": \"<p>Tentative schedule:</p> <p><br/>19:00 Doors Open</p> <p>- food, drinks, networking</p> <p>19:30 Presentations (starting presentations earlier)</p> <p>- TBD call for presenters!</p> <p>20:50 wrap up </p> <p>nnWe'll fit in two or three ~10 minute presentations and leave about 45 minutes at the end for discussion, chit-chat, networking</p> \",    \"visibility\": \"public\"  }}" }; }
        }

        private IEnumerable<Event> CreateExpectedEvents(string eventJson)
        {
            var events = new List<Event>();
            var eventObjects = JObject.Parse(eventJson);
            foreach (var eventObject in eventObjects)
                events.Add(eventObject.Value.ToObject<Event>());

            return events;
        }
    }
}
