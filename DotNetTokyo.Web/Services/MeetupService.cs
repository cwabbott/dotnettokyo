using DotNetTokyo.Web.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DotNetTokyo.Web.Services
{
    public class MeetupService : IMeetupService
    {
        private IMeetupConfiguration _meetupConfig;
        private HttpClient _httpClient;

        public MeetupService() : this(new MeetupConfiguration(), new HttpClient()) { }
        public MeetupService(IMeetupConfiguration meetupConfig, HttpClient httpClient)
        {
            _meetupConfig = meetupConfig;
            _httpClient = httpClient;
        }

        public virtual async Task<IEnumerable<Event>> GetUpcomingEvents()
        {
            var apiUri = ConstructApiUri(
                "events",
                new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("scroll", "next_upcoming")
                });
            var events = new List<Event>();

            var result = await CallApi(HttpVerbs.Get, apiUri);
            if (result.IsSuccessStatusCode) {
                events = JsonConvert.DeserializeObject<IEnumerable<Event>>(result.Content.ReadAsStringAsync().Result).ToList();
            }

            return events;
        }

        internal virtual Task<HttpResponseMessage> CallApi(
            HttpVerbs httpVerb, string apiUrl, HttpContent httpContent = null)
        {
            switch (httpVerb) {
                case HttpVerbs.Get:
                    return _httpClient.GetAsync(apiUrl);
                default:
                    return null;
            }
        }

        internal virtual string ConstructApiUri(
            string apiMethod, IEnumerable<KeyValuePair<string, string>> queryStringKeyValues)
        {
            var queryString = new List<string>();

            foreach (var qr in queryStringKeyValues)
                queryString.Add(qr.Key + "=" + HttpUtility.UrlEncode(qr.Value));

            return string.Format("http://{0}/{1}/{2}?{3}",
                _meetupConfig.ApiDomain, _meetupConfig.EventGroupName, apiMethod, string.Join("&", queryString));
        }
    }
}