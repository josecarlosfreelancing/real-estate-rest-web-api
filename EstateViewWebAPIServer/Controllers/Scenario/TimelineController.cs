using Microsoft.AspNetCore.Mvc;
using EstateViewWebAPIServer.Models;
using EstateViewWebAPIServer.Interfaces;
using EstateViewWebAPIServer.Models.Scenarios;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EstateViewWebAPIServer.Controllers.Scenario
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly IScenario _scenario;
        public TimelineController(IScenario scenario)
        {
            _scenario = scenario;
        }
        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] RequestFromClient request)
        {
            return Ok(_scenario.GetTimelines(request));
        }
    }
}
