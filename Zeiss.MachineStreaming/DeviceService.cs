using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;

namespace Zeiss.MachineStreaming
{
    /// <summary>
    /// Works as web service.
    /// </summary>
    public sealed class DeviceService
    {
        private readonly DeviceManager deviceManager;
        public DeviceService(DeviceManager deviceManager)
        {
            this.deviceManager = deviceManager;
        }

        public void ProcessDataFromDevice(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                ProcessDataFromDevice(stream);
            }
        }

        public void ProcessDataFromDevice(TextReader textReader)
        {
            using (var jsonTextReader = new JsonTextReader(textReader))
            {
                ProcessDataFromDevice(jsonTextReader);
            }
        }

        public void ProcessDataFromDevice(JsonTextReader jsonTextReader)
        {
            foreach (DeviceStatusMessage message in ReadJsonStream(jsonTextReader))
            {
                deviceManager.ProcessMessages(message);
            }
        }

        public void WriteClientDataAboutAllDevices(Stream responseSteramWriteOnlyOutput)
        {
            using (var streamWriter = new StreamWriter(responseSteramWriteOnlyOutput))
            {
                WriteClientDataAboutAllDevices(streamWriter);
            }
        }

        public void WriteClientDataAboutAllDevices(TextWriter writer)
        {
            using (var jsonTextWriter = new JsonTextWriter(writer))
            {
                WriteClientDataAboutAllDevices(jsonTextWriter);
            }
        }

        /// <remarks>
        /// Client should get stream of JsonMessages about all devices.
        /// </remarks>
        public void WriteClientDataAboutAllDevices(JsonTextWriter writer)
        {
            var serializer = new JsonSerializer();
            writer.WriteStartArray();
            foreach (var message in deviceManager.DeviceStatusObservable.ToEnumerable())
            {
                serializer.Serialize(writer, message);
            }
            writer.WriteEndArray();
        }

        public IEnumerable<DeviceStatusMessage> ReadJsonStream(JsonTextReader jsonTextReader)
        {
            var serializer = new JsonSerializer();
            // Implement iterative deserialization; eading messages one by one.
            // I did it long time ago but don't have time to research NewtonJson library again now.
            throw new NotImplementedException();
        }
    }
}
