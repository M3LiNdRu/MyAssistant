using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAssistant.Apis.Expenses.Api.Attributes;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace MyAssistant.Apis.Expenses.Api.Resources.Categories
{
    [Authorize]
    [ApiController]
    public class CategoriesController : ControllerBase
    { 
        private readonly ICategoriesService _service;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoriesService service, ILogger<CategoriesController> logger) 
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/v1/category")]
        [ValidateModelState]
        [SwaggerOperation("AddCategory")]
        [SwaggerResponse(statusCode: 200, type: typeof(Response), description: "Successful operation")]
        public virtual async Task<IActionResult> AddCategory([FromBody]Request body, CancellationToken cancellationToken)
        { 
            var category = new Category {
                Name = body.Name,
                Description = body.Description
            };

            await _service.AddAsync(category, cancellationToken);

            return Ok();
        }

        [HttpGet]
        [Route("/api/v1/categories")]
        [ValidateModelState]
        [SwaggerOperation("GetAllCategories")]
        [SwaggerResponse(statusCode: 200, type: typeof(Response), description: "Successful operation")]
        public virtual async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
        { 
            var response = await _service.GetAsync(cancellationToken);

            return Ok(response);
        }

        [HttpDelete]
        [Route("/api/v1/category/{name}")]
        [ValidateModelState]
        [SwaggerOperation("RemoveCategory")]
        public virtual async Task<IActionResult> RemoveCategory([FromRoute][Required]string name, CancellationToken cancellationToken)
        { 
            await _service.DelAsync(name, cancellationToken);
            return Ok();
        }
    }
}
