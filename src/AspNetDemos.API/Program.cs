using Asp.Versioning;
using AspNetDemos.API;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(options => {
    // Suppress implicit required attribute for non-nullable reference types
    // This prevents ASP.NET Core from adding [Required] to such properties automatically
    // This prevents "request field is required" errors for complex types that have custom validation.
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;

})
    .ConfigureApiBehaviorOptions(options => {
        // Standardize ValidationProblemDetails output for model validation errors
        options.InvalidModelStateResponseFactory = context =>
    {
        var modelState = context.ModelState;
        // Normalize JSON path keys like $.RecipeId to RecipeId
        var pathKeys = modelState.Keys.Where(k => k.StartsWith("$.")).ToArray();
        foreach (var key in pathKeys)
        {
            if (!modelState.TryGetValue(key, out var entry) || entry == null)
            {
                continue;
            }

            var normalizedKey = key[2..]; // Remove "$."
            if (!string.IsNullOrWhiteSpace(normalizedKey))
            {
                if (modelState.TryGetValue(normalizedKey, out var normalizedEntry) && normalizedEntry is not null)
                {
                    foreach (var error in entry.Errors)
                    {
                        normalizedEntry.Errors.Add(error);
                    }
                }
                else
                {
                    foreach (var error in entry.Errors)
                    {
                        modelState.AddModelError(normalizedKey, error.ErrorMessage);
                    }
                }
            }

            // Remove the original key to avoid duplication
            if (key != "$.") // Normalization of the root key would result in empty string key and not duplication.
            {
                modelState.Remove(key);
            }
        }

        var problemDetails = new ValidationProblemDetails(modelState)
        {
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
        };

        return new BadRequestObjectResult(problemDetails)
        {
            ContentTypes = { "application/problem+json" }
        };
    };
    }).AddJsonOptions(options => {
    // Register the RecipeIdConverter for RecipeId value object serialization
    options.JsonSerializerOptions.Converters.Add(new RecipeIdJsonConverter());

    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddApiVersioning(options => {
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc()
.AddApiExplorer(options => { 
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseAuthorization();
app.MapControllers();

app.Run();
