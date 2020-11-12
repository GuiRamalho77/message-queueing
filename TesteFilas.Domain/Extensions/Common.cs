using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Queue.Domain.Extensions
{
    public static class Common
    {
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrWhiteSpace(value);
        public static T DeserializeMessage<T>(ReadOnlyMemory<byte> body) where T : class => JsonSerializer.Deserialize<T>(body.ToArray());

    }
}
