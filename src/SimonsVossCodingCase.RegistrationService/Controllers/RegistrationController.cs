using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimonsVossCodingCase.RegistrationService.Models;
using SimonsVossCodingCase.RegistrationService.Services;

namespace SimonsVossCodingCase.RegistrationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRemoteSignService _remoteSignService;

        public RegistrationController(IRemoteSignService remoteSignService)
        {
            _remoteSignService = remoteSignService;
        }

        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get(RegistrationModel model, CancellationToken cancellationToken = default)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await _remoteSignService.SignJson(JsonSerializer.Serialize(model), cancellationToken));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}