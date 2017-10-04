using Chat.Random.Domain.Entities.Chat.Received;
using Chat.Random.Infra.Socket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Random.Service.Channel
{
    public class UserChannel : IChatAnonymousChannel, IUserChannelHandle
    {
        private StreamWriteAsync streamWrite;
        private IChatAnonymousChannel anonymousChannel;

        public UserChannel(StreamWriteAsync streamWrite)
        {
            this.streamWrite = streamWrite;
        }

        public async Task ArrivedMessage(ChatMessageProtocol message)
        {
            if(anonymousChannel != null)
            {
                lock(anonymousChannel)
                    await anonymousChannel.PostAsync(message);
            }
        }

        public void EstablishedConnection(IChatAnonymousChannel anonymousChannel)
        {
            if(anonymousChannel == null)
            {
                lock (anonymousChannel)
                    this.anonymousChannel = anonymousChannel;
            }else
            {
                this.anonymousChannel = anonymousChannel;
            }
        }

        public async Task PostAsync(ChatMessageProtocol message)
        {
            await streamWrite.WritedAsync(ToJson(message));
        }

        private string ToJson(ChatMessageProtocol msg)
        {
            return JsonConvert.SerializeObject(msg);
        }

        public void StopConnection()
        {
            if (anonymousChannel == null)
            {
                lock (anonymousChannel)
                {
                    anonymousChannel.Stop();
                    anonymousChannel = null;
                }
            }
        }

        public void Stop()
        {
            if (anonymousChannel == null)
            {
                lock (anonymousChannel)
                {
                    anonymousChannel = null;
                }
            }
        }
    }
}
