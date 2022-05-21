using System;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using MyAssistant.Apis.Expenses.Api.Attributes;
using MyAssistant.Apis.Expenses.Api.Models;

namespace MyAssistant.Apis.Expenses.Api.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class ExpensesController : ControllerBase
    { 
        /// <summary>
        /// Add a new expense
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">BadRequest</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Route("/api/v1/expense")]
        [ValidateModelState]
        [SwaggerOperation("AddExpense")]
        public virtual IActionResult AddExpense([FromBody]Expense body)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete an expense
        /// </summary>
        /// <param name="id">Expense Id</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">BadRequest</response>
        /// <response code="401">Unauthorized</response>
        [HttpDelete]
        [Route("/api/v1/expense/{id}")]
        [ValidateModelState]
        [SwaggerOperation("DelExpense")]
        public virtual IActionResult DelExpense([FromRoute][Required]int? id)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all expenses
        /// </summary>
        /// <response code="200">Successful operation</response>
        [HttpGet]
        [Route("/api/v1/expenses")]
        [ValidateModelState]
        [SwaggerOperation("GetAllExpenses")]
        [SwaggerResponse(statusCode: 200, type: typeof(GetAllExpensesResponse), description: "Successful operation")]
        public virtual IActionResult GetAllExpenses()
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(GetAllExpensesResponse));
            string exampleJson = null;
            exampleJson = "{\n  \"expenses\" : [ {\n    \"amount\" : 56.9,\n    \"name\" : \"EL BRUCH\",\n    \"currency\" : \"EUR\",\n    \"id\" : 1,\n    \"category\" : \"Carburant\",\n    \"timestamp\" : \"2000-01-23T04:56:07.000+00:00\"\n  }, {\n    \"amount\" : 56.9,\n    \"name\" : \"EL BRUCH\",\n    \"currency\" : \"EUR\",\n    \"id\" : 1,\n    \"category\" : \"Carburant\",\n    \"timestamp\" : \"2000-01-23T04:56:07.000+00:00\"\n  } ]\n}";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<GetAllExpensesResponse>(exampleJson)
                        : default(GetAllExpensesResponse);            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
