using Microsoft.AspNetCore.Mvc;
using EstateViewWebAPIServer.Models;
using EstateViewWebAPIServer.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EstateViewWebAPIServer.Controllers.Scenario
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsController : ControllerBase
    {
        private readonly IScenario _scenario;
        public DetailsController(IScenario scenario)
        {
            _scenario = scenario;
        }
        // POST api/<DetailsController>
        [HttpPost]
        public IActionResult Post([FromBody] RequestFromClient request)
        {
            return Ok(_scenario.GetDetails(request));
        }
    }
}
