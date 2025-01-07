using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Traveller.Models;

namespace Traveller.Infrastructure.Json.Converters
{
    public class ShipComponentConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeToConvert == typeof(ShipComponent) || typeToConvert.IsSubclassOf(typeof(ShipComponent));

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var derivedTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ShipComponent)));

            var converterType = typeof(ShipComponentConverter<>).MakeGenericType(typeToConvert);
            var converter = (JsonConverter)Activator.CreateInstance(converterType, derivedTypes.ToList())!;

            return converter;
        }
    }
}
