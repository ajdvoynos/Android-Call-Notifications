using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using EOW.Hubs;
using Microsoft.Extensions.Configuration;

namespace EOW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<MyHub> _hubContext;
        private readonly IConfiguration Configuration;

        public HomeController(ILogger<HomeController> logger, IHubContext<MyHub> hubContext, IConfiguration configuration)
        {
            _logger = logger;
            _hubContext = hubContext;
            Configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Send(string pw)
        {
            if (pw != Configuration["HubPassword"])
                return StatusCode(500, "Sneaky ;)");

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", Configuration["SuperSecretMessage"]);
            return Ok();

        }
    }
}
