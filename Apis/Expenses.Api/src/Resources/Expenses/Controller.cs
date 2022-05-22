using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using MyAssistant.Apis.Expenses.Api.Attributes;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{ 
    [ApiController]
    public class ExpensesController : ControllerBase
    { 
        private readonly ILogger<ExpensesController> _logger;
        private readonly IExpensesService _service;

        public ExpensesController(IExpensesService service, ILogger<ExpensesController> logger) 
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [Route("/api/v1/expense")]
        [ValidateModelState]
        [SwaggerOperation("AddExpense")]
        public virtual async Task<IActionResult> AddExpense([FromBody]Request body, CancellationToken cancellationToken)
        { 
            var expense = new Expense {
                Timestamp = body.Timestamp ?? DateTime.UtcNow,
                Category = body.Category,
                Name = body.Name,
                Amount = body.Amount,
                Currency = body.Currency ?? "EUR"
            };
            await _service.AddAsync(expense, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("/api/v1/expense/{id}")]
        [ValidateModelState]
        [SwaggerOperation("DelExpense")]
        public virtual async Task<IActionResult> DelExpense([FromRoute][Required]int id, CancellationToken cancellationToken)
        { 

            await _service.DelAsync(id, cancellationToken);
            return Ok();
        }

        [HttpGet]
        [Route("/api/v1/expenses")]
        [ValidateModelState]
        [SwaggerOperation("GetAllExpenses")]
        [SwaggerResponse(statusCode: 200, type: typeof(Response), description: "Successful operation")]
        public virtual async Task<IActionResult> GetAllExpenses(CancellationToken cancellationToken)
        { 
            var expenses = await _service.GetAsync(cancellationToken);
            return Ok(expenses);
        }
    }
}
