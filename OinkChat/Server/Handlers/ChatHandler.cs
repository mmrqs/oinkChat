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
                "exit" => Exit(),
                "post" => Send(input.Text()),
                _ => Help()
            };
         }

        public Message Send(string message)
        {
            _session.TopicJoined.SendEventMessage(new ChatMessage(_session.User, message));
            return null;
        }

        public Message Help()
        {
            return new DumbMessage("You can :",
                "Post a message : post <message>",
                "Exit the topic : exit");
        }

        public Message Exit()
        {
            _session.TopicJoined.Unsubscription(_session.Sender.ReceiveMessage);
            _session.TopicJoined = null;
            return new DumbMessage("You exited the topic " + _session.TopicJoined.Title);
        }
    }
}