using System;

namespace DotNetTokyo.Web.Models
{
    public class Meta
    {
        public string lon { get; set; }
        public int count { get; set; }
        public string link { get; set; }
        public string next { get; set; }
        public int total_count { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public long updated { get; set; }
        public string description { get; set; }
        public string method { get; set; }
        public string lat { get; set; }
    }

    public class Event
    {
        public long created { get; set; }
        public int duration { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public long time { get; set; }
        public long updated { get; set; }
        public int utc_offset { get; set; }
        public int waitlist_count { get; set; }
        public int yes_rsvp_count { get; set; }
        public Venue venue { get; set; }
        public Group group { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string visibility { get; set; }

        public DateTime DateTimeUtc
        {
            get
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
                return epoch.AddMilliseconds(time);
            }
        }

        public DateTime DateTimeLocal
        {
            get { return DateTimeUtc.AddMilliseconds(utc_offset); }
        }
    }

    public class Venue
    {
        public int id { get; set; }
        public string name { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public bool repinned { get; set; }
        public string address_1 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string localized_country_name { get; set; }
    }

    public class Group
    {
        public int id { get; set; }
        public float group_lat { get; set; }
        public string name { get; set; }
        public float group_lon { get; set; }
        public string join_mode { get; set; }
        public string urlname { get; set; }
        public string who { get; set; }
    }
}