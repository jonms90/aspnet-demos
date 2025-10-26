using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace AspNetDemos.API;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public class RecipeController : ControllerBase
{
    // POST <RecipeController>
    [HttpPost]
    [Produces("application/problem+json")]
    public IActionResult Post([FromBody] NewRecipeRequest request)
    {
        // Process the valid recipe
        return Ok();
    }
}

public class NewRecipeRequest
{
    [JsonConverter(typeof(RecipeIdConverter))]
    public required RecipeId Id { get; set; }
    public required string Name { get; set; }
}

