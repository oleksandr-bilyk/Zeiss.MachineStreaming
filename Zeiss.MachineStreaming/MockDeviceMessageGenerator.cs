using System;
using System.Threading;

namespace Zeiss.MachineStreaming
{
    public sealed class MockDeviceMessageGenerator
    {
        private readonly DeviceManager deviceManager;
        public MockDeviceMessageGenerator(DeviceManager deviceManager)
        {
            this.deviceManager = deviceManager;
        }

        public void GenerateMessage(int count)
        {
            for (int i = 0; i < count; i++)
            {
                deviceManager.ProcessMessages(
                    new DeviceStatusMessage
                    {
                        MessageId = Guid.Empty,
                        MachineId = Guid.Empty,
                        Status = DeviceStatus.Idle,
                        Timestamp = DateTimeOffset.Now,
                    }
                ); 
            }
        }
    }
}
