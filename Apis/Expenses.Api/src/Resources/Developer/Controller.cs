using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Resources.Developer
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperController : ControllerBase
    {
        [HttpGet]
        [Route("/api/v1/developer")]
        [SwaggerOperation("CheckAuthentication")]
        [SwaggerResponse(statusCode: 200, description: "Successful operation")]
        public virtual async Task<IActionResult> CheckAuth(CancellationToken cancellationToken)
        {
            return Ok("You are in!");
        }
    }
}
