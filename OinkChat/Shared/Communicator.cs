using Shared.Messages;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Shared
{
    /// <summary>
    /// delegate of the event ClientDecoEvent
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ClientDecoEventHandler(object sender, Message e);

    public class Communicator
    {
        /// <summary>
        /// Event corresponding to a client deconnexion
        /// </summary>
        private event ClientDecoEventHandler ClientDecoEvent;

        private BinaryFormatter _binaryFormatter = new BinaryFormatter();

        /// <summary>
        /// Serializes a message through the TcpClient Stream
        /// </summary>
        /// <param name="s"></param>
        /// <param name="message"></param>
        public void Send(Stream s, Message message)
        {
            _binaryFormatter.Serialize(s, message);
        }

        /// <summary>
        /// Deserializes a message through the TcpClient Stream
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Subscription to the Event ClientDecoEvent
        /// </summary>
        /// <param name="method"></param>
        public void Subscription(ClientDecoEventHandler method)
        {
            ClientDecoEvent += method;
        }
    }
}
