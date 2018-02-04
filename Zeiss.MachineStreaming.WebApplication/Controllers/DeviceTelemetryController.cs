using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Zeiss.MachineStreaming.WebApplication.Models;

namespace Zeiss.MachineStreaming.WebApplication.Controllers
{
    /// <summary>
    /// Devices send its telemetry to this service.
    /// </summary>
    [Route("api/[controller]")]
    public class DeviceTelemetryController : Controller
    {
        private readonly DeviceService devicesService;
        public DeviceTelemetryController(DeviceService devicesService)
        {
            this.devicesService = devicesService;
        }

        // GET: api/DeviceTelemetry
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        // POST: api/DeviceTelemetry
        [HttpPost]
        public void Post()
        {
            devicesService.ProcessDataFromDevice(HttpContext.Request.Body);
        }
    }
}
