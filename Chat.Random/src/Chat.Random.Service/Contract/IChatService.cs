using Chat.Random.Infra.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Random.Service.Contract
{
    public interface IChatService
    {
        void ArrivedChat(Session session);
    }
}
