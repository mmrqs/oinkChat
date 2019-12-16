using System;
using System.Net.Sockets;
using System.Threading;
using Server.Handlers;
using Shared.Messagers;
using Shared.Messages;

namespace Server.Controllers
{

    /// <summary>
    /// Will handle the message received from the Receiver, create an output and send it to the Sender
    /// </summary>
    class ServerMailer : Mailer
    {
        private ChatData _data;
        private DispatchSession _session;
        private HandlerFactory _factory;
        
        /// <summary>
        /// Constructor
        ///
        /// It initializes :
        ///
        /// - data : all the server data
        /// - session : all the informations related to the client
        /// - factory : object that allows the dispatching in the corresponding handler
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="session"></param>
        public ServerMailer(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
            _factory = new HandlerFactory();
        }


        /// <summary>
        /// 1 - Receives the message from the Receiver.
        /// 2 - Creates an output by getting the Handler for the message and calling the corresponding Handle method in the Handler.
        /// 3 - if the output isn't null, it raises an event that will be caught by the Sender transmitting the output.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message">The message send by the Receiver</param>
        public override void OnMessageReceived(object sender, Message message)
        {
            ClientMessage input = (ClientMessage)message;

            Message output = _factory
                .GetHandler(_data, _session, input.KeyWord)
                .Handle(input);
            
            if (output != null)
                RaiseEvent(this, output);
        }
    }
}