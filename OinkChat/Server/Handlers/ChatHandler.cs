using System;
using Server.Controllers;
using Server.Models;
using Shared.Messages;

namespace Server.Handlers
{
    class ChatHandler : IHandler
    {
        private ChatData _data;
        private DispatchSession _session;

        public ChatHandler(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
        }
        
        public Message Handle(Message input)
        {
            string answer = DateTime.Now.ToString("g") + "  " + _session.PseudoClient + "  ";
            answer += input.ToString();
            string topic = _session.TopicJoined.Title;
            
            if (!input.ToString().Equals("exit"))
            {
                _session.TopicJoined.SendEventMessage(answer);
                return null;
            }

            _session.TopicJoined.Unsubscription(_session.Sender.ReceiveMessage);
            _session.TopicJoined = null;
            return new DumbMessage("You exited the topic " + topic);
        
         }
    }
}