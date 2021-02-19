using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BookStore.Utility
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value) 
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
    }
}
