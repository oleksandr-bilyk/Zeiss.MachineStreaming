using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Zeiss.MachineStreaming
{
    public static class DeviceStatusMessageJsonSerializer
    {
        public static void SerializeEnumerable(
            JsonTextWriter writer, IEnumerable<DeviceStatusMessage> collection
        )
        {
            var serializer = new JsonSerializer();
            writer.WriteStartArray();
            foreach (var message in collection)
            {
                serializer.Serialize(writer, message);
            }
            writer.WriteEndArray();
        }

        public static IEnumerable<DeviceStatusMessage> DeserializeEnumerable(JsonTextReader reader)
        {
            var serializer = JsonSerializer.Create();

            bool readOpeningArray = reader.Read();
            if (!readOpeningArray || reader.TokenType != JsonToken.StartArray)
                throw new SerializationException("StartArray expected.");

            while (true)
            {
                bool readNextItem = reader.Read();
                if (!readNextItem)
                    throw new SerializationException("Array item or end expected.");

                if (reader.TokenType == JsonToken.StartObject)
                {
                    var message = serializer.Deserialize<DeviceStatusMessage>(reader);
                    yield return message;
                }
                else if (reader.TokenType == JsonToken.EndArray) break;
                else throw new SerializationException("Unexpected token.");
            }
        }
    }
}
