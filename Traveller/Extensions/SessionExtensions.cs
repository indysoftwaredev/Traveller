using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Traveller.Infrastructure.Json.Converters;

namespace Traveller.Extensions
{
    public static class SessionExtensions
    {
        private static readonly JsonSerializerOptions _options;

        static SessionExtensions()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new ShipComponentConverterFactory());
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value, _options));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value, _options);
        }
    }
}
