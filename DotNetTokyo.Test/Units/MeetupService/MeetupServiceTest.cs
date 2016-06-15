using DotNetTokyo.Test.Infra;
using DotNetTokyo.Web.Models;
using DotNetTokyo.Web.Services;
using FakeItEasy;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DotNetTokyo.Test.Units
{
    public class MeetupServiceTest
    {
        private const string ApiDomain = "api.domain.com";
        private const string EventGroupName = "event-group-name";

        private IMeetupConfiguration _meetupConfig;

        public MeetupServiceTest()
        {
            
            _meetupConfig = A.Fake<IMeetupConfiguration>();
            A.CallTo(() => _meetupConfig.ApiDomain).Returns(ApiDomain);
            A.CallTo(() => _meetupConfig.EventGroupName).Returns(EventGroupName);
        }

        [Theory]
        [MemberData("EventTestData")]
        public async Task ShouldReturnUpcomingEvents(string eventJson)
        {
            // Arrange
            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddFakeResponse(
                new Uri(string.Format("http://{0}/{1}/{2}", ApiDomain, EventGroupName, "events")),
                new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent(eventJson)
                }
              );

            // Act
            var httpClient = new HttpClient(fakeResponseHandler);
            var meetupSvc = new MeetupService(_meetupConfig, httpClient);
            var upcomingEvents = await meetupSvc.GetUpcomingEvents();

            var expUpcomingEvents = JsonConvert.DeserializeObject<IEnumerable<Event>>(eventJson);
            var compareLogic = new CompareLogic();
            var compareResult = compareLogic.Compare(upcomingEvents, expUpcomingEvents);
            compareResult.AreEqual.ShouldBe(true, compareResult.DifferencesString);
        }

        [Theory]
        [MemberData("ApiUriTestData")]
        public void ShouldReturnApiUri(string apiMethod, IEnumerable<KeyValuePair<string, string>> keyValues, string expApiUri)
        {
            var meetupSvc = new MeetupService(_meetupConfig, new HttpClient());
            var apiUri = meetupSvc.ConstructApiUri(apiMethod, keyValues);
            apiUri.ShouldBe(expApiUri);
        }

        public static IEnumerable<object[]> EventTestData
        {
            get 
            {
                // No event
                yield return new object[] { "[]" };
                // One event
                yield return new object[] { "[{\"created\":1465281615000,\"id\":\"231709906\",\"name\":\"Tokyo .NET Developers Meetup #12 - 2016-6-21\",\"status\":\"upcoming\",\"time\":1466503200000,\"updated\":1465281615000,\"utc_offset\":32400000,\"waitlist_count\":0,\"yes_rsvp_count\":10,\"venue\":{\"id\":23880758,\"name\":\"JP Tower 29F, Medidata Office\",\"lat\":0.0,\"lon\":0.0,\"repinned\":false,\"address_1\":\"2-7-2 Marunouchi, Chiyoda, Tokyo 100-0005\",\"city\":\"Tokyo\",\"country\":\"jp\",\"localized_country_name\":\"Japan\"},\"group\":{\"created\":1433397718000,\"name\":\"Tokyo .NET Developers Meetup\",\"id\":18647945,\"join_mode\":\"open\",\"lat\":35.66999816894531,\"lon\":139.77000427246094,\"urlname\":\"Tokyo-NET-Developers-Meetup\",\"who\":\"Engineers\"},\"link\":\"http://www.meetup.com/Tokyo-NET-Developers-Meetup/events/231709906/\",\"description\":\"<p>Tentative schedule:</p> <p><br/>19:00 Doors Open</p> <p>- food, drinks, networking</p> <p>19:30 Presentations (starting presentations earlier)</p> <p>- TBD call for presenters!</p> <p>20:50 wrap up </p> <p>\n\nWe'll fit in two or three ~10 minute presentations and leave about 45 minutes at the end for discussion, chit-chat, networking</p> \",\"visibility\":\"public\"}]" };
                // Multiple events
                yield return new object[] { "[{\"created\":1462787056000,\"id\":\"230984712\",\"name\":\"Tokyo .NET Developers Meetup #11 - 2016-5-17\",\"status\":\"past\",\"time\":1463479200000,\"updated\":1463490952000,\"utc_offset\":32400000,\"waitlist_count\":0,\"yes_rsvp_count\":17,\"venue\":{\"id\":23880758,\"name\":\"JP Tower 29F, Medidata Office\",\"lat\":0.0,\"lon\":0.0,\"repinned\":false,\"address_1\":\"2-7-2 Marunouchi, Chiyoda, Tokyo 100-0005\",\"city\":\"Tokyo\",\"country\":\"jp\",\"localized_country_name\":\"Japan\"},\"group\":{\"created\":1433397718000,\"name\":\"Tokyo .NET Developers Meetup\",\"id\":18647945,\"join_mode\":\"open\",\"lat\":35.66999816894531,\"lon\":139.77000427246094,\"urlname\":\"Tokyo-NET-Developers-Meetup\",\"who\":\"Engineers\"},\"link\":\"http://www.meetup.com/Tokyo-NET-Developers-Meetup/events/230984712/\",\"description\":\"<p>Tentative schedule:</p> <p><br/>19:00 Doors Open</p> <p>- food, drinks, networking</p> <p>19:30 Presentations (starting presentations earlier)</p> <p>- Bot Framework</p> <p>- </p> <p>20:50 wrap up  </p> \",\"visibility\":\"public\"},{\"created\":1465281615000,\"id\":\"231709906\",\"name\":\"Tokyo .NET Developers Meetup #12 - 2016-6-21\",\"status\":\"upcoming\",\"time\":1466503200000,\"updated\":1465281615000,\"utc_offset\":32400000,\"waitlist_count\":0,\"yes_rsvp_count\":10,\"venue\":{\"id\":23880758,\"name\":\"JP Tower 29F, Medidata Office\",\"lat\":0.0,\"lon\":0.0,\"repinned\":false,\"address_1\":\"2-7-2 Marunouchi, Chiyoda, Tokyo 100-0005\",\"city\":\"Tokyo\",\"country\":\"jp\",\"localized_country_name\":\"Japan\"},\"group\":{\"created\":1433397718000,\"name\":\"Tokyo .NET Developers Meetup\",\"id\":18647945,\"join_mode\":\"open\",\"lat\":35.66999816894531,\"lon\":139.77000427246094,\"urlname\":\"Tokyo-NET-Developers-Meetup\",\"who\":\"Engineers\"},\"link\":\"http://www.meetup.com/Tokyo-NET-Developers-Meetup/events/231709906/\",\"description\":\"<p>Tentative schedule:</p> <p><br/>19:00 Doors Open</p> <p>- food, drinks, networking</p> <p>19:30 Presentations (starting presentations earlier)</p> <p>- TBD call for presenters!</p> <p>20:50 wrap up </p> <p>\n\nWe'll fit in two or three ~10 minute presentations and leave about 45 minutes at the end for discussion, chit-chat, networking</p> \",\"visibility\":\"public\"}]" };
            }

        }

        public static IEnumerable<object[]> ApiUriTestData
        {
            get
            {
                var apiMethod = "api-method";

                yield return new object[] { 
                    apiMethod, 
                    Enumerable.Empty<KeyValuePair<string, string>>(), 
                    string.Format("http://{0}/{1}/{2}", ApiDomain, EventGroupName, apiMethod) };
                yield return new object[] { 
                    apiMethod, 
                    new KeyValuePair<string, string>[] { 
                        new KeyValuePair<string, string>("q1", "v1") }, 
                    string.Format("http://{0}/{1}/{2}?q1=v1", ApiDomain, EventGroupName, apiMethod) };
                yield return new object[] { 
                    apiMethod, 
                    new KeyValuePair<string, string>[] { 
                        new KeyValuePair<string, string>("q1", "v1"), 
                        new KeyValuePair<string, string>("q2", "v2") }, 
                    string.Format("http://{0}/{1}/{2}?q1=v1&q2=v2", ApiDomain, EventGroupName, apiMethod) };
            }
        }
    }
}
