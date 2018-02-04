using Newtonsoft.Json;
using System;
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
            using (var textReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(textReader))
            {
                foreach (DeviceStatusMessage message in DeviceStatusMessageJsonSerializer.DeserializeEnumerable(jsonTextReader))
                {
                    deviceManager.ProcessMessages(message);
                }
            }
        }

        /// <remarks>
        /// Client should get stream of JsonMessages about all devices.
        /// </remarks>
        public void WriteClientDataAboutAllDevices(Stream steam, int? maxMessagesCount = null)
        {
            IObservable<DeviceStatusMessage> observable = deviceManager.DeviceStatusObservable;
            if (maxMessagesCount.HasValue)
            {
                observable = observable.Take(maxMessagesCount.Value);
            }

            using (var streamWriter = new StreamWriter(steam))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                DeviceStatusMessageJsonSerializer.SerializeEnumerable(jsonTextWriter, observable.ToEnumerable());
            }
        }
    }
}
