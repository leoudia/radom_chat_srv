using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Random.Infra.Socket
{
    public class StreamWriteAsync
    {
        private WebSocket webSocket;

        public StreamWriteAsync(WebSocket webSocket)
        {
            this.webSocket = webSocket;
        }

        public async Task WritedAsync(string msg)
        {
            var buffer = Encoding.UTF8.GetBytes(msg);

            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
