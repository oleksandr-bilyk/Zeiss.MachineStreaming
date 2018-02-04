using System;

namespace Zeiss.MachineStreaming
{
    /// <summary>
    /// Device reports about its status.
    /// </summary>
    public sealed class DeviceStatusMessage
    {
        public Guid MessageId { get; set; }
        public Guid MachineId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public DeviceStatus Status { get; set; }
    }
}
