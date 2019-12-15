using System;
using Shared.Messages;

namespace Shared.Models
{
    public delegate void MessageSenderEventHandler(object sender, Message e);

    [Serializable]
    public class Topic
    {
        [field: NonSerialized]
        private event MessageSenderEventHandler MessageSenderEvent;

        public string Title { get; set; }

        public Topic(string title)
        {
            Title = title;
        }

        public override string ToString()
        {
            return Title;
        }

        public override bool Equals(object obj)
        { 
            return obj is Topic topic &&
                   Title == topic.Title;
        }
        
        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
        
        public void Subscription(MessageSenderEventHandler method)
        {
            MessageSenderEvent += method;
        }

        public void Unsubscription(MessageSenderEventHandler method)
        {
            MessageSenderEvent -= method;
        }
        
        public void SendEventMessage(Message message)
        {
            MessageSenderEvent?.Invoke(this, message);
        }
    }
}