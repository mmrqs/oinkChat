using Server.Controllers;
using Shared.Models;
using Shared.Messages;

namespace Server.Handlers
{
    class TopicHandler : IHandler
    {
        private ChatData _data;
        private DispatchSession _session;

        /// <summary>
        /// Class constructor
        ///
        /// It initializes :
        ///
        /// - data : all the server data
        /// - session : all the data related to the client
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="session"></param>
        public TopicHandler(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
        }
        
        /// <summary>
        /// It handle the message sent by the client
        /// </summary>
        /// <param name="input">The message sent by the client</param>
        /// <returns>a Message</returns>
        public Message Handle(ClientMessage input)
        {
            return input.KeyWord switch
            {
                "create" => CreateTopic(new Topic(input[0])),
                "display" => DisplayTopics(),
                "join" => JoinTopic(input[0]),
                "help" => HelpMessage(),
                _ => HelpMessage()
            };
        }

        /// <summary>
        /// It allows the user to create a topic
        /// </summary>
        /// <param name="topic">The topic to create</param>
        /// <returns>A validation message if the topic has been created or an error message otherwise </returns>
        private Message CreateTopic(Topic topic)
        {
            return _data.AddTopic(topic) ? 
                new DumbMessage("A new topic named " + topic.Title + " has been created. ") : 
                new DumbMessage("The topic " + topic.Title + " already exists.");
        }

        /// <summary>
        /// Allows to display all the topics created
        /// </summary>
        /// <returns> A message containing all the topics created </returns>
        private Message DisplayTopics()
        {
            return new DumbMessage("The availaible topics are :", _data.GetTopicList());
        }

        /// <summary>
        /// Allows to join a topic
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Message JoinTopic(string name)
        {
            Topic selectedTopic = _data.GetTopicByTitle(name);

            if (selectedTopic == null) 
                return new DumbMessage("The topic " + name + " doesn't exist.");

            if(_session.TopicsJoined.Contains(selectedTopic))
                return new DumbMessage("You already joined the topic "+name);
            
            //We subscribe the Sender of the client to the topic 
            selectedTopic.Subscription(_session.Sender.ReceiveMessage);
            //We add the Topic to the list of topics joined by the client
            _session.TopicsJoined.Add(selectedTopic);

            return new DumbMessage("You joined the topic " + name);
        }

        /// <summary>
        /// Helper corresponding to the TopicHandler class
        /// </summary>
        /// <returns> an help message </returns>
        private Message HelpMessage()
        {
            return new HelpMessage("Create a topic : create <topic name>", 
                "Display a list of every topic : display", 
                "Join a topic : join <topic name>", 
                "Post a message : post <topic> <message>",
                "Exit the topic : exit <topic>",
                "Send a private message : mp <pseudo> <message>",
                "Display the online users : displayUsers");
        }
    }
}