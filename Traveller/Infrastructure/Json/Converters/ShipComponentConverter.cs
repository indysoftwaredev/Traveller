using System.Text.Json.Serialization;
using System.Text.Json;
using Traveller.Models;

namespace Traveller.Infrastructure.Json.Converters
{
    public class ShipComponentConverter<T> : JsonConverter<T> where T : ShipComponent
    {
        private readonly Dictionary<string, Type> _typeMap;

        public ShipComponentConverter(List<Type> derivedTypes)
        {
            _typeMap = derivedTypes.ToDictionary(t => t.Name);
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            // Create new options without this converter to avoid recursion
            var newOptions = new JsonSerializerOptions();
            foreach (var converter in options.Converters)
            {
                if (!(converter is ShipComponentConverterFactory))
                {
                    newOptions.Converters.Add(converter);
                }
            }

            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var jsonObject = jsonDoc.RootElement;

            // Try to get the type from the discriminator
            if (jsonObject.TryGetProperty("$type", out var typeProperty))
            {
                var typeName = typeProperty.GetString();
                if (typeName != null && _typeMap.TryGetValue(typeName, out var type))
                {
                    return (T)JsonSerializer.Deserialize(jsonObject.GetRawText(), type, newOptions)!;
                }
            }

            // Fallback to base type if no matching derived type
            return JsonSerializer.Deserialize<T>(jsonObject.GetRawText(), newOptions);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // Create new options without this converter to avoid recursion
            var newOptions = new JsonSerializerOptions();
            foreach (var converter in options.Converters)
            {
                if (!(converter is ShipComponentConverterFactory))
                {
                    newOptions.Converters.Add(converter);
                }
            }

            var baseJson = JsonSerializer.SerializeToNode(value, value.GetType(), newOptions);
            if (baseJson != null)
            {
                baseJson["$type"] = value.GetType().Name;
                baseJson.WriteTo(writer);
            }
        }
    }
}
