namespace Zeiss.MachineStreaming
{
    public enum DeviceStatus
    {
        Idle,
        Running,
        /// TODO: maybe consider to replace finished by iddle or waiting
        Finished,
        Errorre,
    }
}