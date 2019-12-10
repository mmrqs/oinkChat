using Shared.Messages;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Shared
{
    public class Communicator
    {
        public static void Send(Stream s, IMessage message)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(s, message);
        }

        public static IMessage Receive(Stream s)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter(); 
            return (IMessage)binaryFormatter.Deserialize(s);
        }
    }
}
