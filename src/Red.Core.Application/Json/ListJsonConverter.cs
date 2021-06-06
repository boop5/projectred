using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Red.Core.Application.Json
{
    public sealed class ListJsonConverter<T> : JsonConverter<List<T>>
    {
        public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var list = new List<T>();

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Not an array");
            }

            reader.Read(); // skip startArray

            while (reader.TokenType is not JsonTokenType.EndArray)
            {
                try
                {
                    var value = reader.GetString();
                    var item = (T)Convert.ChangeType(value, typeof(T));
                    list.Add(item);
                }
                catch
                {
                    // ignored
                }

                reader.Read(); // read next item
            }

            return list;
        }

        public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
