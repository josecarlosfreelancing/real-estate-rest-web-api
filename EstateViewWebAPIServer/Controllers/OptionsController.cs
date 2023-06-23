using Microsoft.AspNetCore.Mvc;
using EstateViewWebAPIServer.Models;
using EstateViewWebAPIServer.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EstateViewWebAPIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly IOption _options;
        public OptionsController(IOption options)
        {
            _options = options;
        }

        // GET: api/<OptionsController>
        [HttpGet]
        public IActionResult GetNewOptions()
        {
            // EstateProjectionOptions options = EstateProjectionOptions.CreateEmptyOptions();
            return Ok(_options.GetInitialOptions());
        }
    }
}
