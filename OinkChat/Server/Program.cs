using System;
using Shared;
using Server.Controllers;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Controllers.Server server = new Controllers.Server(Constants.Hostname, Constants.Port);
            server.Start();
        }
    }
}
