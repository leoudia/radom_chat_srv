using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Random.Domain.Entities.Chat.Send
{
    public class ChatMessage
    {
        public ChatMessage(string body)
        {
            Body = body;
        }

        public string Body { get; }
    }
}
