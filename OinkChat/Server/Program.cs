using System;
using Shared;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(Constants.Hostname, Constants.Port);
            server.Start();
        }
    }
}
