using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAssistant.Apis.Expenses.Api.Attributes;
using MyAssistant.Apis.Expenses.Api.Resources.Historigrams;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Resources.Historigrams
{
    //[Authorize]
    [ApiController]
    public class HistorigramController : ControllerBase
    {
        private readonly ILogger<HistorigramController> _logger;
        private readonly IHistorigramService _service;

        public HistorigramController(IHistorigramService service, ILogger<HistorigramController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("/api/v1/historigram/savings")]
        [ValidateModelState]
        [SwaggerOperation("GetSavingsHistorigram")]
        [SwaggerResponse(statusCode: 200, type: typeof(SavingsHistorigram), description: "Successful operation")]
        public virtual async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
        {
            var historigram = await _service.GetSavingsHistorigramAsync(cancellationToken);

            return Ok(historigram);
        }
    }
}
