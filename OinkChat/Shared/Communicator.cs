using Shared.Messages;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Shared
{
    public class Communicator
    {
        public static void Send(Stream s, Message message)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(s, message);
        }

        public static Message Receive(Stream s)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter(); 
            return (Message)binaryFormatter.Deserialize(s);
        }
    }
}
