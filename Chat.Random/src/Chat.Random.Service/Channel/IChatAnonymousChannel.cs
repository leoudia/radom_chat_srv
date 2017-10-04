using Chat.Random.Domain.Entities.Chat.Received;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Random.Service.Channel
{
    public interface IChatAnonymousChannel
    {
        Task PostAsync(ChatMessageProtocol message);
        void Stop();
    }
}
