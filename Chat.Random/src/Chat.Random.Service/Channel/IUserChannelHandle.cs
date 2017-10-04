using Chat.Random.Domain.Entities.Chat.Received;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Random.Service.Channel
{
    public interface IUserChannelHandle
    {
        void EstablishedConnection(IChatAnonymousChannel anonymousChannel);

        Task ArrivedMessage(ChatMessageProtocol message);

        void StopConnection();
    }
}
