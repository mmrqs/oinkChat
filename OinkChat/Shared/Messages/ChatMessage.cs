
using Shared.Models;
using System;

namespace Shared.Messages
{
    [Serializable]
    public class ChatMessage : Message
    {
        private string Content { get; }
        
        private User Sender { get; }
        private DateTime Date { get; }

        private Topic Topic { get; }

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