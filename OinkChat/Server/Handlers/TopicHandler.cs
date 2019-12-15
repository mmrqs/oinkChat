using System;
using Server.Controllers;
using Shared.Models;
using Shared.Messages;

namespace Server.Handlers
{
    class TopicHandler : IHandler
    {
        private ChatData _data;
        private DispatchSession _session;

        public TopicHandler(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
        }
        
        public Message Handle(ClientMessage input)
        {
            return (input.KeyWord) switch
            {
                "create" => CreateTopic(new Topic(input[0])),
                "display" => DisplayTopics(),
                "join" => JoinTopic(input[0]),
                "help" => HelpMessage(),
                _ => HelpMessage(),
            };
        }

        private Message CreateTopic(Topic topic)
        {
            return _data.AddTopic(topic) ? 
                new DumbMessage("A new topic named " + topic.Title + " is created. ") : 
                new DumbMessage("The topic " + topic.Title + " already exists.");
        }

        private Message DisplayTopics()
        {
            return new DumbMessage(_data.GetTopicList());
        }

        private Message JoinTopic(string name)
        {
            _session.TopicJoined = _data.GetTopicByTitle(name);
            if (_session.TopicJoined == null) 
                return new DumbMessage("Topic " + name + " doesn't exist");

            _session.TopicJoined.Subscription(_session.Sender.ReceiveMessage);
            return new DumbMessage("You joined the topic " + name);
        }

        private Message HelpMessage()
        {
            return new DumbMessage("You can :", 
                "Create a topic : create <topic name>", 
                "Display a list of every topic : display", 
                "Join a topic : join <topic name>");
        }
        
        
    }
}