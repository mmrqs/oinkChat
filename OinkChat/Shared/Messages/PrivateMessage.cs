using Shared.Models;
using System;

namespace Shared.Messages
{
    [Serializable]
    public class PrivateMessage : Message
    {
        private User _sender;
        private string _content;

        private DateTime Date;

        public PrivateMessage(string text, User sender)
        {
            _sender = sender;
            _content = text;
            Date = DateTime.Now;
        }

        public override string Text
        {
            get 
            {
                return String.Join(" ", _sender, "sent you a new message", 
                    Environment.NewLine, Date.ToString("g"), _sender, ":", _content);
            }
        }

    }
}
