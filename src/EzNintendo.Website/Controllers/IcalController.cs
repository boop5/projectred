using System.Threading.Tasks;
using EzNintendo.Website.Services.Web;
using Microsoft.AspNetCore.Mvc;

namespace EzNintendo.Website.Controllers
{
    [ApiController]
    public sealed class IcalController : ControllerBase
    {
        private readonly CalendarService _calendarService;

        public IcalController(CalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpGet("calendar/games.ical")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _calendarService.GetGamesICal());
        }
    }
}