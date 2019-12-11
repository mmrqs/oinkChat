using System;
using Shared;
using Server.Controllers;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatServer server = new ChatServer(Constants.Hostname, Constants.Port);
            server.Start();
        }
    }
}
