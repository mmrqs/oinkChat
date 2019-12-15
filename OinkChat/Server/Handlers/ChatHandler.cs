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
            CommandMessage cm = new CommandMessage(input);
            if (cm.Target.Equals(""))
                return Help();

            return (input.KeyWord) switch
            {
                "exit" => ExitTopic(cm.Target),
                "post" => Post(cm),
                _ => null
            };
         }

        public Message Post(CommandMessage message)
        {
            Topic selectedTopic = _data.GetTopicByTitle(message.Target);
            
            if (selectedTopic == null)
                return new DumbMessage("The topic " + message.Target + " doesn't exist.");

            selectedTopic.SendEventMessage(new ChatMessage(_session.User,message.Text, selectedTopic));
            return _session.TopicsJoined.Contains(selectedTopic) ?
                null : new DumbMessage("WARNING !",
                "You posted in a topic you have not joined",
                "You won’t get any answer from the other users");
        }

        public Message ExitTopic(string name)
        {
            Topic selectedTopic = _data.GetTopicByTitle(name);

            if (selectedTopic == null)
                return new DumbMessage("The topic " + name + " doesn't exist.");

            if (!_session.TopicsJoined.Contains(selectedTopic))
                return new DumbMessage("You have not joined the topic " + name + " yet.");

            _session.TopicsJoined.Remove(selectedTopic);
            selectedTopic.Unsubscription(_session.Sender.ReceiveMessage);
            return new DumbMessage("You exited the topic " + name);
        }

        private Message Help()
        {
            return new HelpMessage("Post a message : post <topic> <message>",
                "Exit the topic : exit <topic>");
        }
    }
}