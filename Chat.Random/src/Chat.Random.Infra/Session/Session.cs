using Chat.Random.Domain.Entities;
using Chat.Random.Infra.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Random.Infra.Session
{
    public class Session
    {
        public string Id { get; }

        public User User { get; }

        public StreamWriteAsync StreamWrite { get; }

        public StreamReadAsync StreamRead { get; }
    }
}
