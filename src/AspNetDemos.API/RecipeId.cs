using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetDemos.API
{
    public class RecipeId
    {
        public string Value { get; }

        private RecipeId(string value)
        {
            Value = value;
        }

        public static bool TryCreate(string? raw, out RecipeId? result, out string? error)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                result = null;
                error = "RecipeId cannot be null or empty.";
                return false;
            }

            if (raw.Length < 5)
            {
                result = null;
                error = "RecipeId must be at least 5 characters long.";
                return false;
            }

            result = new RecipeId(raw);
            error = null;
            return true;
        }

        public override string ToString() => Value;
    }

    public class RecipeIdConverter : JsonConverter<RecipeId>
    {
        public override RecipeId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("RecipeId must be a string.");
            }

            string? raw = reader.GetString();
            if(!RecipeId.TryCreate(raw, out RecipeId? valueObject, out string? error))
            {
                throw new JsonException(error);
            }

            return valueObject;
        }
        public override void Write(Utf8JsonWriter writer, RecipeId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}