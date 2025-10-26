using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetDemos.API
{
    public class RecipeId
    {
        public string Id { get; private set; }
        public RecipeId(string id)
        {
            if(id.Length < 5)
            {
                throw new ArgumentException("RecipeId must be at least 5 characters long.", nameof(id));
            }

            Id = id;
        }
    }

    public class RecipeIdConverter : JsonConverter<RecipeId>
    {
        public override RecipeId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string id = reader.GetString()!;
                try
                {
                    return new RecipeId(id);
                }
                catch (ArgumentException)
                {
                    // Return null instead of throwing so the object can be created
                    // and model validation will add a property-level error for Id.
                    return null;
                }
            }

            // Non-string tokens are not valid for Id. Return null so validation will catch it.
            return null;
        }
        public override void Write(Utf8JsonWriter writer, RecipeId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Id);
        }
    }
}