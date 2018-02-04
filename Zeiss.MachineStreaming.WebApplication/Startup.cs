using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.IO;

namespace Zeiss.MachineStreaming.WebApplication
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            DependencyInjectionCompositionRoot(services);
        }

        private static void DependencyInjectionCompositionRoot(IServiceCollection services)
        {
            services.AddSingleton<DeviceManager>();
            services.AddTransient<DeviceService>();
            services.AddSingleton<MockDeviceMessageGenerator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseWebSockets(
                new WebSocketOptions()
                {
                    KeepAliveInterval = TimeSpan.FromSeconds(60),
                    ReceiveBufferSize = 4 * 1024,
                }
            );
            app.Use(WebSocketMiddlewareExtension);
        }

        private async Task WebSocketMiddlewareExtension(HttpContext context, Func<Task> next)
        {
            if (context.Request.Path == "/websocket")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await WebSocketEcho(context, webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await next();
            }
        }

        private async Task WebSocketEcho(HttpContext context, WebSocket webSocket)
        {
            var deviceManager = context.RequestServices.GetRequiredService<DeviceManager>();
            foreach (var message in deviceManager.DeviceStatusObservable.ToEnumerable())
            {
                byte[] messageData = SerializeMessage(message);
                await webSocket.SendAsync(
                    messageData, WebSocketMessageType.Text, endOfMessage: true, cancellationToken: CancellationToken.None
                );
            }

            await webSocket.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
        }

        private byte[] SerializeMessage(DeviceStatusMessage message)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            new JsonSerializer().Serialize(streamWriter, message);
            return memoryStream.ToArray();
        }
    }
}
