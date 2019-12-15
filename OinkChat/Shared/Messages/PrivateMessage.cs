using System;

namespace Shared.Messages
{
    [Serializable]
    public class PrivateMessage : Message
    {
        private String _sender;
        private String _content;

        public PrivateMessage(String text, String pseudo)
        {
            _sender = pseudo;
            _content = text;
        }

        public override string Text
        {
            get { return _sender + " send you a new message" + Environment.NewLine 
                    +_sender +" : "+ _content; }
        }

    }
}
