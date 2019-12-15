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

        private BinaryFormatter _binaryFormatter = new BinaryFormatter();
        public void Send(Stream s, Message message)
        {
            _binaryFormatter.Serialize(s, message);
        }

        public Message Receive(Stream s)
        {
            try
            {
                return (Message)_binaryFormatter.Deserialize(s);
            }
            catch
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
