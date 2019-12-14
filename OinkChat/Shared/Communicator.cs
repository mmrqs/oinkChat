using Shared.Messages;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Shared
{
    public delegate void ClientDecoEventHandler(object sender, Message e);

    public class Communicator
    {
        private event ClientDecoEventHandler ClientDecoEvent;
        
        public void Send(Stream s, Message message)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(s, message);
        }

        public Message Receive(Stream s)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Message m = null;
            try
            {
                m = (Message) binaryFormatter.Deserialize(s);
                return m;
            }
            catch (Exception e)
            {
                ClientDecoEvent?.Invoke(this, new DumbMessage("An existing connection was forcibly closed by the remote host"));
                return null;
            }
        }
        
        public void Subscription(ClientDecoEventHandler method)
        {
            ClientDecoEvent += method;
        }
    }
}
