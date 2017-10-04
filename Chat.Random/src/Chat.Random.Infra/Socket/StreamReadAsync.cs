using Chat.Random.Domain.Entities;
using Chat.Random.Infra.Parameter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Random.Infra.Socket
{
    public class StreamReadAsync
    {
        private WebSocket webSocket;

        public StreamReadAsync(WebSocket webSocket)
        {
            this.webSocket = webSocket;
        }

        public bool IsRead()
        {
            return webSocket.State.Equals(WebSocketState.Open);
        }

        public async Task<string> ReadAsync()
        {
            var buffer = new byte[ChatParameters.BufferSize];

            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (!result.CloseStatus.HasValue)
                throw new IOException(Messages.INVALID_SOCKET_MSG);
            
            return Encoding.UTF8.GetString(buffer);
        }

        public async Task CloseAsync()
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, Messages.CLOSE_SOCKET_MSG, CancellationToken.None);
        }
    }
}
