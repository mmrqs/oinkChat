using System;
using System.Reflection;
using Shared.Messages;

namespace Server.Models
{
    public delegate void MessageSenderEventHandler(object sender, ChatMessage e);
    [Serializable]
    public class Topic
    {
        private event MessageSenderEventHandler MessageSenderEvent;
        public string Title { get; set; }

        public Topic(string title)
        {
            Title = title;
        }

        public override bool Equals(object obj)
        { 
            return obj is Topic topic &&
                   Title == topic.Title;
        }
        
        public override int GetHashCode()
        {
                return HashCode.Combine(Title);
        }
        
        public void Subscription(MessageSenderEventHandler method)
        {
            this.MessageSenderEvent += method;
        }
        
        public void SendEventMessage(String message)
        {
            if (this.MessageSenderEvent != null)
            {
                this.MessageSenderEvent(this, new ChatMessage(message));
            }
            
        }
    }
}