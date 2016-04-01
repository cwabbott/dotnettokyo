using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DotNetTokyo.Web.Models;
using Newtonsoft.Json;

namespace DotNetTokyo.Web.Services
{
    public class MeetupService : IMeetupService
    {
        public virtual string UpcomingEventsApiUrl
        {
            get { return ConfigurationManager.AppSettings["MeetupUpcomingEventsUrl"]; }
        }

        private HttpClient client;

        public MeetupService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<Event>> GetUpcomingEvents()
        {
            var result = await client.GetStringAsync(UpcomingEventsApiUrl);
            var upcoming = JsonConvert.DeserializeObject<IList<Event>>(result);
            return upcoming ?? Enumerable.Empty<Event>();
        }
    }
}