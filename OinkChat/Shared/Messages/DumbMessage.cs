using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    [Serializable]
    public class DumbMessage : Message
    {
        private string[] _lines;

        public DumbMessage(params string[] text)
        {
            _lines = text;
        }

        public override string Text()
        {
            return string.Join("\n", _lines);
        }
    }
}
