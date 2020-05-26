using RemoteOrganControl.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RemoteOrganControl.Server.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SettingController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> logger;

        public SettingController(ILogger<WeatherForecastController> logger)
        {
            this.logger = logger;
        }
        
        [HttpGet]
        public IEnumerable<String> Get()
        {
            return new List<string>().Append("test");
        }
    }
}
