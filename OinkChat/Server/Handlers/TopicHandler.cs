using System;
using Server.Controllers;
using Server.Models;
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
        
        public Message Handle(Message input)
        {
            string[] answer = input.ToString().Split(" ");
            return (answer[0]) switch
            {
                "create" => CreateTopic(new Topic(answer[1])),
                "display" => DisplayTopics(),
                "join" => JoinTopic(answer[1]),
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
            return new DumbMessage("You can :\n" +
                "Create a topic : <create> <topic name>\n" +
                "Display a list of every topic : <display>\n" +
                "Join a topic : <join> <topic name>");
        }
    }
}