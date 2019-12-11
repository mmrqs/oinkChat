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

        
        
        public IMessage Handle(IMessage input)
        {
            string[] answer = input.ToString().Split(" ");
            return (answer[0]) switch
            {
                "CreateTopic" => CreateTopic(new Topic(answer[1])),
                "DisplayTopics" => DisplayTopics(),
                _ => new DumbMessage("The correct syntax is <CreateTopic|DisplayTopics|JoinTopic>"),
            };
        }

        private IMessage CreateTopic(Topic  topic)
        {
            return _data.AddTopic(topic) ? new DumbMessage("A new topic named"+topic.Title+"is created. ") : new DumbMessage("The topic " + topic.Title + " already exists.");
        }

        private IMessage DisplayTopics()
        {
            return _data.GetAllTopics();
        }
    }
}