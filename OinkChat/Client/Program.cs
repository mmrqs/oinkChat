using Shared;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client(Constants.Hostname, Constants.Port);
            client.Run();
        }
    }
}
