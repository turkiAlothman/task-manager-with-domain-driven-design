using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Application.Errors;

namespace TaskManager.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult ErrorHandler()
        {
            Debug.WriteLine("errrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrroooooooooooooooooooooooooooooooooooooooooooooooooooooor");

            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            //var (StatusCode, Description) = exception switch
            //{
            //    IServiceException ServiceException => ((int)ServiceException.StatusCode, ServiceException.Message)
            //    _=> (500, "error occurred during processing the request")
            //};


            // return Problem(title: Description);    

            return Ok();
        }
    }
}
