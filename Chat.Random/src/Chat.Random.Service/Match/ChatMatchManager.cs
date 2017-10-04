using Chat.Random.Service.Channel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Random.Service.Match
{
    public class ChatMatchManager
    {
        private ConcurrentQueue<UserChannel> queue = new ConcurrentQueue<UserChannel>();

        public async Task<bool> Match(UserChannel channelStart, UserChannel channelTarget)
        {
            var msgStart = MessageFactory.MessageStart();

            channelStart.EstablishedConnection(channelTarget);
            channelTarget.EstablishedConnection(channelStart);

            await channelStart.ArrivedMessage(msgStart);
            await channelTarget.ArrivedMessage(msgStart);

            return true;
        }

        public async Task<bool> Stop(UserChannel channel)
        {
            var msg = MessageFactory.MessageStop();

            await channel.ArrivedMessage(msg);

            channel.StopConnection();

            return true;
        }

        public async Task<bool> Next(UserChannel channel)
        {
            UserChannel channelTarget;
            if (queue.Any())
            {
                while (queue.TryPeek(out channelTarget)) { }

                await Match(channel, channelTarget);

                return true;
            }
            else
            {
                queue.Enqueue(channel);

                return false;
            }
        }
    }
}
