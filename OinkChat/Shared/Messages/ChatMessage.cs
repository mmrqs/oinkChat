
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

        public ChatMessage(User sender, string content)
        {
            Content = content;
            Sender = sender;
            Date = DateTime.Now;
        }
        public ChatMessage(DateTime date, User sender, string content)
        {
            Content = content;
            Sender = sender;
            Date = date;
        }

        public override string Text()
        {
            return string.Join(" ", Date.ToString("g"), Sender, "said :", Content);
        }
    }
}