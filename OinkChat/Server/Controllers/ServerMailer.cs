using System;
using System.Net.Sockets;
using System.Threading;
using Server.Handlers;
using Shared.Messages;

namespace Server.Controllers
{
    /// <summary>
    /// The delegate of the event MessageEvent.
    /// It delegates to the sender method : receiveMessage
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MessageProcessedEventHandler(object sender, Message e);

    /// <summary>
    /// Will handle the message received from the Receiver, create an output and send it to the Sender
    /// </summary>
    class ServerMailer
    {
        /// <summary>
        /// The event corresponding to the input processing
        /// </summary>
        private event MessageProcessedEventHandler InputProcessedEvent;
        
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
        /// Run the thread while the client is online
        /// </summary>
        /// <param name="token"></param>
        public void Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested) 
            {
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// 1 - Receives the message from the Receiver.
        /// 2 - Creates an output by getting the Handler for the message and calling the corresponding Handle method in the Handler.
        /// 3 - if the output isn't null, it raises an event that will be caught by the Sender transmitting the output.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message">The message send by the Receiver</param>
        public void Action(object sender, Message message)
        {
            ClientMessage input = (ClientMessage)message;

            Message output = _factory
                .GetHandler(_data, _session, input.KeyWord)
                .Handle(input);
            
            //Invoke : execute the delegate
            if (output != null)
                InputProcessedEvent?.Invoke(this, output);
        }
        
        /// <summary>
        /// Allows the subscription to the InputProcessEvent.
        /// </summary>
        /// <param name="method"></param>
        public void Subscription(MessageProcessedEventHandler method)
        {
            InputProcessedEvent += method;
        }
    }
}