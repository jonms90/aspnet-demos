using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        // If we reach here, the model is valid and can be processed.
        // For simplicity, we just return a Created response with the recipe details.
        return CreatedAtRoute(null, new
        {
            id = request.Id.Value,
            name = request.Name
        });
    }
}

public class NewRecipeRequest
{
    [Required]
    public RecipeId Id { get; set; }

    [StringLength(maximumLength: 50, MinimumLength = 3)]
    [Required]
    public string Name { get; set; }
}

