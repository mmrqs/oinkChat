using System;
using System.Net.Sockets;
using System.Threading;
using Server.Handlers;
using Shared.Messages;

namespace Server.Controllers
{
    public delegate void MessageEventHandler(object sender, Message e);
    class ServerMailer
    {
        private event MessageEventHandler MessageEvent;
        
        private ChatData _data;
        private DispatchSession _session;
        
        public ServerMailer(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
        }

        public void Run(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }
        public void Action(object sender, Message message)
        {
            ClientMessage input = (ClientMessage)message;

            Message output = new HandlerFactory()
                .GetHandler(_data, _session, input.KeyWord)
                .Handle(input);
            
            //Invoke : execute the delegate
            if (output != null)
                MessageEvent?.Invoke(this, output);
        }
        
        public void Subscription(MessageEventHandler method)
        {
            MessageEvent += method;
        }
    }
}