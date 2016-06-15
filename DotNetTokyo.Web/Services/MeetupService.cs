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
            var apiUri = ConstructApiUri("events");
            var events = Enumerable.Empty<Event>();

            var result = await CallApi(HttpVerbs.Get, apiUri);
            if (result.IsSuccessStatusCode)
                events = JsonConvert.DeserializeObject<IEnumerable<Event>>(result.Content.ReadAsStringAsync().Result);

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

        internal virtual string ConstructApiUri(string apiMethod)
        {
            return ConstructApiUri(apiMethod, Enumerable.Empty<KeyValuePair<string, string>>());
        }
        internal virtual string ConstructApiUri(
            string apiMethod, IEnumerable<KeyValuePair<string, string>> queryStringKeyValues)
        {
            var queryString = 
                string.Join("&", queryStringKeyValues.Select(kv => kv.Key + "=" + HttpUtility.UrlEncode(kv.Value)));
            if (!string.IsNullOrEmpty(queryString))
                queryString = "?" + queryString;

            return string.Format("http://{0}/{1}/{2}{3}",
                _meetupConfig.ApiDomain, _meetupConfig.EventGroupName, apiMethod, string.Join("&", queryString));
        }
    }
}