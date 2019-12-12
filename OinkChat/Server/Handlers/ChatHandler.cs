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
            
            if (input.ToString().Equals("exit"))
            {
                return new TopicHandler(_data,_session).Handle(input);
            }
            else
            {
                _session.TopicJoined.SendEventMessage(answer);
            }
            
            return null;
        }
    }
}