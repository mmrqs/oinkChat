
using System;

namespace Shared.Messages
{
    [Serializable]
    public class ChatMessage : Message
    {
        private string _content;

        public string Content
        {
            get { return _content; }
        }
        
        public ChatMessage(string p)
        {
            _content = p;
        }

        public override string ToString()
        {
            return _content;
        }
    }
}