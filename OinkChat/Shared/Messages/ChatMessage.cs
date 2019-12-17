
using Shared.Models;
using System;

namespace Shared.Messages
{
    /// <summary>
    /// Messages used to post in a topic
    /// </summary>
    [Serializable]
    public class ChatMessage : Message
    {
        /// <summary>
        /// The content of the message
        /// </summary>
        private string Content { get; }
        
        /// <summary>
        /// The Sender of the message
        /// </summary>
        private User Sender { get; }
        
        /// <summary>
        /// The date when the message has been sent
        /// </summary>
        private DateTime Date { get; }

        /// <summary>
        /// The topic of the message
        /// </summary>
        private Topic Topic { get; }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="sender"> The sender of the message </param>
        /// <param name="content"> The message content</param>
        /// <param name="topic">The topic of the message</param>
        public ChatMessage(User sender, string content, Topic topic)
        {
            Content = content;
            Sender = sender;
            Topic = topic;
            Date = DateTime.Now;
        }

        public override string Text
        {
            get { return string.Join(" ", Date.ToString("g"),"[", Topic.Title,"]", Sender, "said :", Content); }
        }
    }
}