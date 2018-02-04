using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Linq;

namespace Zeiss.MachineStreaming.Tests
{
    [TestClass]
    public class DeviceStatusMessageJsonSerializerTest
    {
        [TestMethod]
        public void TestMethod()
        {
            int count = 10;
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            var jsonTextWriter = new JsonTextWriter(stringWriter);
            DeviceStatusMessageJsonSerializer.SerializeEnumerable(jsonTextWriter, GetTestEnumerable(count));

            var stringReader = new StringReader(stringBuilder.ToString());
            var jsonTextReader = new JsonTextReader(stringReader);
            var collection = DeviceStatusMessageJsonSerializer.DeserializeEnumerable(jsonTextReader);
            Assert.AreEqual(count, collection.Count());
        }

        IEnumerable<DeviceStatusMessage> GetTestEnumerable(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new DeviceStatusMessage
                {
                    MessageId = Guid.NewGuid(),
                    MachineId = Guid.Empty,
                    Timestamp = DateTimeOffset.UtcNow,
                    Status = DeviceStatus.Running,
                };
            }
        }
    }
}
