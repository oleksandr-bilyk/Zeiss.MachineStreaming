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
            var messages = deviceManager.DeviceStatusObservable.ToEnumerable();
            DeviceStatusMessageJsonSerializer.SerializeEnumerable(writer, messages);
        }

        public IEnumerable<DeviceStatusMessage> ReadJsonStream(JsonTextReader reader)
        {
            return DeviceStatusMessageJsonSerializer.DeserializeEnumerable(reader);
        }
    }
}
