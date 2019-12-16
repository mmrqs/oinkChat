using Shared;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        /// <summary>
        /// Starts a new client with an hostname, a port and run it
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            new Client(new TcpClient(Constants.Hostname, Constants.Port)).Run();
        }
    }
}
