using EstateViewWebAPIServer.Interfaces;
using EstateViewWebAPIServer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EstateViewWebAPIServer.Controllers.Scenario
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogisticsController : ControllerBase
    {
        private readonly IScenario _scenario;
        public LogisticsController(IScenario scenario)
        {
            _scenario = scenario;
        }

        // POST api/<LogisticsController>
        [HttpPost]
        public IActionResult Post([FromBody] RequestFromClient request)
        {
            return Ok(_scenario.GetLogistics(request));
        }
    }
}
