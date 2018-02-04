using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Zeiss.MachineStreaming.WebApplication.Controllers
{
    /// <summary>
    /// Provides information for system center clients.
    /// </summary>
    [Produces("application/json")]
    [Route("api/SystemCenterTelemetry")]
    public class SystemCenterTelemetryController : Controller
    {
        private readonly DeviceService devicesService;
        private readonly MockDeviceMessageGenerator mockDeviceMessageGenerator;
        public SystemCenterTelemetryController(DeviceService devicesService, MockDeviceMessageGenerator mockDeviceMessageGenerator)
        {
            this.devicesService = devicesService;
            this.mockDeviceMessageGenerator = mockDeviceMessageGenerator;
        }

        [HttpGet]
        public string Get() => "SystemCenterTelemetry service.";

        // GET api/values/5
        [HttpGet("Mock-{id}")]
        public void GenerateMockDeviceMessages(int id)
        {
            mockDeviceMessageGenerator.GenerateMessage(id);
        }

        // GET: api/SystemCenterTelemetry
        [HttpPost]
        public void Post()
        {
            HttpContext.Response.ContentType = "application/json";
            devicesService.WriteClientDataAboutAllDevices(HttpContext.Response.Body, maxMessagesCount: 3);
        }
    }
}
