using Chat.Random.Domain.Entities.Chat.Received;
using Chat.Random.Domain.Entities.Chat.Send;
using System;

namespace Chat.Random.Service.Match
{
    public static class MessageFactory
    {
        public static ChatMessageProtocol MessageStart()
        {
            ChatMessageProtocol msg = new ChatMessageProtocol()
            {
                Command = Domain.Entities.Chat.ChatCommandType.START,
                ChatMessage = new ChatMessage("Conexão estabelecida. Bom papo!")
            };

            return msg;
        }

        public static ChatMessageProtocol MessageStop()
        {
            ChatMessageProtocol msg = new ChatMessageProtocol()
            {
                Command = Domain.Entities.Chat.ChatCommandType.STOP,
                ChatMessage = new ChatMessage("Conexão perdida.")
            };

            return msg;
        }
    }
}