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
        
        /// <summary>
        /// Class constructor
        ///
        /// It initializes :
        ///
        /// - data : all the server data
        /// - session : all the data related to the client
        /// </summary>
        /// <param name="data"></param>
        /// <param name="session"></param>
        public ChatHandler(ChatData data, DispatchSession session)
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
            CommandMessage cm = new CommandMessage(input);
            
            //if the content of the client Message is null, we return help
            if (cm.Target.Equals(""))
                return Help();
            
            return (input.KeyWord) switch
            {
                "exit" => ExitTopic(cm.Target),
                "post" => Post(cm),
                _ => null
            };
         }

        /// <summary>
        /// Allows the client to post in a topic
        ///
        /// 1 - We extract the name topic from the message and get its corresponding object
        /// 2 - If the topic isn't null, we send an event notifying all the users (more specificly their object Sender) who subscribed to our topic
        /// that a new message arrived
        /// </summary>
        /// <param name="message"> CommandMessage </param>
        /// <returns> a warning message if the user try to post in a topic that he didn't join null otherwise </returns>
        public Message Post(CommandMessage message)
        {
            Topic selectedTopic = _data.GetTopicByTitle(message.Target);
            
            if (selectedTopic == null)
                return new DumbMessage("The topic " + message.Target + " doesn't exist.");

            selectedTopic.SendEventMessage(new ChatMessage(_session.User,message.Text, selectedTopic));
            return _session.TopicsJoined.Contains(selectedTopic) ?
                null : new DumbMessage("WARNING !",
                "You posted in a topic you have not joined",
                "You won�t get any answer from the other users");
        }

        /// <summary>
        /// Allows the client to exit a topic
        ///
        /// 1 - We extract the name topic from the message and get its corresponding object
        /// 2 - If the topic isn't null and the client has joined the topic, we remove the topic from his list of topics joined in this session and
        /// we unsubscribe the event linked to the topic
        /// </summary>
        /// <param name="name"> Name of the topic the client want to exit </param>
        /// <returns> return a validation message or an error message </returns>
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

        /// <summary>
        /// Helper corresponding to the ChatHandler class
        /// </summary>
        /// <returns> an help message </returns>
        private Message Help()
        {
            return new HelpMessage("Post a message : post <topic> <message>",
                "Exit the topic : exit <topic>");
        }
    }
}