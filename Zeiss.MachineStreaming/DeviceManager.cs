using System;
using System.Reactive.Subjects;

namespace Zeiss.MachineStreaming
{
    public class DeviceManager
    {
        private readonly Subject<DeviceStatusMessage> subject;
        public DeviceManager()
        {
            subject = new Subject<DeviceStatusMessage>();
        }

        /// <summary>
        /// Used for all devices messages observations by multiple clients.
        /// </summary>
        public IObservable<DeviceStatusMessage> DeviceStatusObservable => subject;

        public void ProcessMessages(DeviceStatusMessage message)
        {
            PersistMessage(message);

            // TODO: do some message filtration.

            NorifyAllSubscriptions(message);
        }

        private void NorifyAllSubscriptions(DeviceStatusMessage message)
        {
            subject.OnNext(message);
        }

        private void PersistMessage(DeviceStatusMessage message)
        {
            //TODO:
        }
    }
}
