using Server.Handlers;
using Shared.Messagers;
using Shared.Messages;

namespace Server.Controllers
{
    public delegate void MessageEventHandler(object sender, Message e);

    class ServerMailer : Mailer
    {
        private ChatData _data;
        private DispatchSession _session;
        private HandlerFactory _factory;
        
        public ServerMailer(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
            _factory = new HandlerFactory();
        }

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