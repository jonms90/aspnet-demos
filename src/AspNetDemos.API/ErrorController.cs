using Microsoft.AspNetCore.Mvc;

namespace AspNetDemos.API
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError() 
        {
            return Problem("An unexpected error occurred.");
        }
    }
}
