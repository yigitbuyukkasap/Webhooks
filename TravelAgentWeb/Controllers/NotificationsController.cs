using Microsoft.AspNetCore.Mvc;
using TravelAgentWeb.Data;

namespace TravelAgentWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController: ControllerBase
    {
        private readonly TravelAgentDbContext _context;

        public NotificationsController(TravelAgentDbContext context)
        {
            _context = context;
        }
    }
}