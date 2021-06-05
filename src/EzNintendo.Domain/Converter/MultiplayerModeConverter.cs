using System;
using EzNintendo.Domain.Nintendo;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Converter
{
    internal sealed class MultiplayerModeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var s = serializer.Deserialize<string>(reader);

            if (s.ToLower().Contains("simultaneous"))
            {
                return MultiplayerMode.Simultaneous;
            }

            if (s.ToLower().Contains("alternating"))
            {
                return MultiplayerMode.Alternating;
            }

            return MultiplayerMode.None;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}