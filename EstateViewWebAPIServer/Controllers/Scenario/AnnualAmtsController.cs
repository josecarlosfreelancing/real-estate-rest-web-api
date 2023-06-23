using Microsoft.AspNetCore.Mvc;
using EstateViewWebAPIServer.Models;
using EstateViewWebAPIServer.Models.Scenarios;
using EstateViewWebAPIServer.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EstateViewWebAPIServer.Controllers.Scenario
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnualAmtsController : ControllerBase
    {
        private readonly IScenario _scenario;
        public AnnualAmtsController(IScenario scenario)
        {
            _scenario = scenario;
        }
        // POST api/<AnnualAmtsController>
        [HttpPost]
        public IActionResult Post([FromBody] RequestFromClient request)
        {
            return Ok(_scenario.GetAccounts(request));
        }
    }
}
