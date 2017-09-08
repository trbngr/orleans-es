using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationsController : Controller
    {
        private readonly IClusterClient _client;

        public RegistrationsController(IClusterClient client) =>
            _client = client;

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRegistration command)
        {
            var registration = _client.GetGrain<IRegistration>(command.Email);
            var result = await registration.Create(command);
            return result.Match(
                Left: BadRequest,
                Right: e => (IActionResult)Ok(e)
            );
        }
    }
}