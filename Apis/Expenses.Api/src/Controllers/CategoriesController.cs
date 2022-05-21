using System;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using MyAssistant.Apis.Expenses.Api.Models;
using MyAssistant.Apis.Expenses.Api.Attributes;


namespace MyAssistant.Apis.Expenses.Api.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class CategoriesController : ControllerBase
    { 
        /// <summary>
        /// Add a new category
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Successful operation</response>
        [HttpPost]
        [Route("/api/v1/category")]
        [ValidateModelState]
        [SwaggerOperation("AddCategory")]
        [SwaggerResponse(statusCode: 200, type: typeof(GetAllCategoriesResponse), description: "Successful operation")]
        public virtual IActionResult AddCategory([FromBody]Category body)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(GetAllCategoriesResponse));
            string exampleJson = null;
            exampleJson = "{\n  \"expenses\" : [ {\n    \"name\" : \"Carburant\",\n    \"description\" : \"Despeses de carburant\"\n  }, {\n    \"name\" : \"Carburant\",\n    \"description\" : \"Despeses de carburant\"\n  } ]\n}";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<GetAllCategoriesResponse>(exampleJson)
                        : default(GetAllCategoriesResponse);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <response code="200">Successful operation</response>
        [HttpGet]
        [Route("/api/v1/categories")]
        [ValidateModelState]
        [SwaggerOperation("GetAllCategories")]
        [SwaggerResponse(statusCode: 200, type: typeof(GetAllCategoriesResponse), description: "Successful operation")]
        public virtual IActionResult GetAllCategories()
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(GetAllCategoriesResponse));
            string exampleJson = null;
            exampleJson = "{\n  \"expenses\" : [ {\n    \"name\" : \"Carburant\",\n    \"description\" : \"Despeses de carburant\"\n  }, {\n    \"name\" : \"Carburant\",\n    \"description\" : \"Despeses de carburant\"\n  } ]\n}";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<GetAllCategoriesResponse>(exampleJson)
                        : default(GetAllCategoriesResponse);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// Rename a new category
        /// </summary>
        /// <param name="name">Category Name</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">BadRequest</response>
        /// <response code="401">Unauthorized</response>
        [HttpDelete]
        [Route("/api/v1/category/{name}")]
        [ValidateModelState]
        [SwaggerOperation("RemoveCategory")]
        public virtual IActionResult RemoveCategory([FromRoute][Required]string name)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            throw new NotImplementedException();
        }
    }
}
