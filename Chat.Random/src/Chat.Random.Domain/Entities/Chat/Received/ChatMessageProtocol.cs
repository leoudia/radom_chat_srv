using Chat.Random.Domain.Entities.Chat.Send;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Random.Domain.Entities.Chat.Received
{
    public class ChatMessageProtocol
    {
        public ChatCommandType Command { get; set; }

        public ChatMessage ChatMessage { get; set; }
    }
}
