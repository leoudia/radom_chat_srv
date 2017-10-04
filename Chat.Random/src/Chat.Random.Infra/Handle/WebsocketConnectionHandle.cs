using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Chat.Random.Infra.Parameter;

namespace Chat.Random.Infra.Handle
{
    public class WebsocketConnectionHandle
    {
        private ILogger<WebsocketConnectionHandle> logger;

        public WebsocketConnectionHandle(ILogger<WebsocketConnectionHandle> logger)
        {
            this.logger = logger;
        }

        public void Configuration(IApplicationBuilder app)
        {
            app.UseWebSockets();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = ChatParameters.KeepAliveIntervalTime,
                ReceiveBufferSize = ChatParameters.BufferSize
            };

            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        
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

            });
        }
    }
}
