using Chat.Random.Domain.Entities.Chat;
using Chat.Random.Infra.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Chat.Random.Domain.Entities.Chat.Received;
using Chat.Random.Service.Match;
using Chat.Random.Service.Channel;

namespace Chat.Random.Service.Process
{
    public class UserProcessEvent
    {
        private StreamReadAsync streamRead;
        private volatile bool stop = false;
        private ILogger logger;
        private ChatMatchManager manager;
        private UserChannel channel;

        public UserProcessEvent(StreamReadAsync streamRead, ILogger logger, ChatMatchManager manager, UserChannel channel)
        {
            this.streamRead = streamRead;
            this.logger = logger;
            this.manager = manager;
            this.channel = channel;
        }

        public async Task ReadMessagesAsync()
        {
            if (streamRead.IsRead())
            {
                while (isRead())
                {
                    try
                    {
                        string msg = await streamRead.ReadAsync();
                        await ProcessMessage(msg);
                    }catch(Exception e)
                    {
                        logger.LogError("ReadMessagesAsync", e);
                        StopProcessMessge();
                    }
                }
            }
        }

        private void StopProcessMessge()
        {
            throw new NotImplementedException();
        }

        private async Task ProcessMessage(string msg)
        {
            try
            {
                ChatMessageProtocol messageProtocol = ParseMessage(msg);
                switch (messageProtocol.Command)
                {
                    case ChatCommandType.NEXT:

                        await manager.Next(channel);
                        break;

                    case ChatCommandType.STOP:

                        await manager.Stop(channel);
                        break;
                }
            }
            catch (Exception e)
            {
                logger.LogError("ParseMessage", e);
            }
        }

        private ChatMessageProtocol ParseMessage(string msg)
        {
            return JsonConvert.DeserializeObject<ChatMessageProtocol>(msg);
        }

        public bool isRead ()
        {
            return !stop && streamRead.IsRead();
        }

        public void Stop()
        {
            stop = true;
        }
    }
}
