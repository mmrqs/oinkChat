using Server.Controllers;
using Shared.Messages;

namespace Server.Handlers
{
    class MessageHandler : IHandler
    {
        private ChatData _data;
        private DispatchSession _session;

        public MessageHandler(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
        }
        
        public IMessage Handle(IMessage input)
        {
            string answer = input.ToString();
            _session.JoinTopic.SendEventMessage(answer);
            return new DumbMessage("");
        }
    }
}