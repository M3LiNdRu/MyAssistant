using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MyAssistant.Apis.Expenses.Api.Attributes;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.AspNetCore.Authorization;

namespace MyAssistant.Apis.Expenses.Api.Resources.Appointments
{
    [Authorize]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IAppointmentsService _service;

        public AppointmentsController(IAppointmentsService service, ILogger<AppointmentsController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [Route("/api/v1/appointment")]
        [ValidateModelState]
        [SwaggerOperation("AddAppointment")]
        public virtual async Task<IActionResult> AddAppointment([FromBody] Request body, CancellationToken cancellationToken)
        {
            var appointment = new Appointment
            {
                Title = body.Title,
                Description = body.Description,
                DateTime = body.DateTime,
                Timestamp = body.Timestamp ?? DateTime.UtcNow
            };
            await _service.AddAsync(appointment, cancellationToken);
            return Ok();
        }

        [HttpGet]
        [Route("/api/v1/appointments")]
        [ValidateModelState]
        [SwaggerOperation("GetUpcomingAppointments")]
        [SwaggerResponse(statusCode: 200, type: typeof(Response), description: "Successful operation")]
        public virtual async Task<IActionResult> GetUpcomingAppointments(CancellationToken cancellationToken)
        {
            var appointments = await _service.GetUpcomingAsync(cancellationToken);
            return Ok(appointments);
        }
    }
}
