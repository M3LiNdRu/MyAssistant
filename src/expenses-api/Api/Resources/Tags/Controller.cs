using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAssistant.Apis.Expenses.Api.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace MyAssistant.Apis.Expenses.Api.Resources.Tags
{
    [Authorize]
    [ApiController]
    public class TagsController : ControllerBase
    { 
        private readonly ILogger<TagsController> _logger;
        private readonly ITagsService _service;

        public TagsController(ITagsService service, ILogger<TagsController> logger) 
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("/api/v1/tags")]
        [ValidateModelState]
        [SwaggerOperation("GetAllTags")]
        [SwaggerResponse(statusCode: 200, type: typeof(Response), description: "Successful operation")]
        public virtual async Task<IActionResult> GetAllTags(CancellationToken cancellationToken)
        { 
            var tags = await _service.GetTagNamesAsync(cancellationToken);
            return Ok(tags);
        }
    }
}
