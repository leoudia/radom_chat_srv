using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Random.Infra.Parameter
{
    public static class ChatParameters
    {
        private static int bufferSize = 1024 * 4;
        private static TimeSpan keepAliveIntervalTime = TimeSpan.FromSeconds(120);

        public static TimeSpan KeepAliveIntervalTime
        {
            get { return keepAliveIntervalTime; }
        }

        public static int BufferSize
        {
            get
            {
                return bufferSize;
            }
        }
    }
}
