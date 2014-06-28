using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetTokyo.Web.Models;

namespace DotNetTokyo.Web.Services
{
    public interface IMeetupService
    {
        Task<IEnumerable<Event>> GetUpcomingEvents();
    }
}