using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AspNetDemos.API;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
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
    [StringLength(maximumLength: 50, MinimumLength = 3)]
    public required string Name { get; set; }
}

