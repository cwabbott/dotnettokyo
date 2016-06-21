using System.Configuration;

namespace DotNetTokyo.Web.Services
{
    public class MeetupConfiguration : IMeetupConfiguration
    {
        public string ApiDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiDomain"];
            }
        }

        public string EventGroupName
        {
            get
            {
                return ConfigurationManager.AppSettings["EventGroupName"];
            }
        }
    }
}