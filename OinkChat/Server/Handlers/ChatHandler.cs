using System;
using Server.Controllers;
using Shared.Models;
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
        
        public Message Handle(ClientMessage input)
        {
            return (input.KeyWord) switch
            {
                "post" => Send(input),
                _ => Help()
            };
         }

        public Message Send(ClientMessage message)
        {
            Topic selectedTopic = _data.GetTopicByTitle(message[0]);
            
            if (selectedTopic == null)
                return new DumbMessage("The topic " + message[0] + " doesn't exist.");

            selectedTopic.SendEventMessage(new ChatMessage(_session.User, message.Text));
            return _session.TopicsJoined.Contains(selectedTopic) ?
                null : new DumbMessage("WARNING !",
                "You posted in a topic you have not joined",
                "You won’t get any answer from the other users");
        }

        public Message Help()
        {
            return new DumbMessage("You can :",
                "Post a message : post <message>");
        }
    }
}