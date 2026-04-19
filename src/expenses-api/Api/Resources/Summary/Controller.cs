using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAssistant.Apis.Expenses.Api.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace MyAssistant.Apis.Expenses.Api.Resources.Summary
{
    [Authorize]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private readonly ILogger<SummaryController> _logger;
        private readonly ISummaryService _service;

        public SummaryController(ISummaryService service, ILogger<SummaryController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("/api/v1/summary/current")]
        [ValidateModelState]
        [SwaggerOperation("GetSummary")]
        [SwaggerResponse(statusCode: 200, type: typeof(CurrentSummary), description: "Successful operation")]
        public virtual async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
        {
            var summary = await _service.GetCurrentSummaryAsync(cancellationToken);

            return Ok(summary);
        }

        [HttpGet]
        [Route("/api/v1/summary/monthly/{year}/{month}")]
        [ValidateModelState]
        [SwaggerOperation("GetMonthlySummary")]
        [SwaggerResponse(statusCode: 200, type: typeof(CompleteSummary), description: "Successful operation")]
        public virtual async Task<IActionResult> GetSummary(int year, int month, CancellationToken cancellationToken)
        {
            var summary = await _service.GetCompleteSummaryAsync(year, month, cancellationToken);

            return Ok(summary);
        }
    }
}
