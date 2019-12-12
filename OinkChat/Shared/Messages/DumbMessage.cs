using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    [Serializable]
    public class DumbMessage : Message
    {
        private string _text;

        public DumbMessage(string text)
        {
            _text = text;
        }

        public override string ToString()
        {
            return _text;
        }
    }
}
