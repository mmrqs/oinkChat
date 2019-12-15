using System;

namespace Shared.Messages
{
    [Serializable]
    public class DumbMessage : Message
    {
        protected string[] _lines;

        public DumbMessage(params string[] text)
        {
            _lines = text;
        }

        public override string Text
        {
            get { return string.Join(Environment.NewLine, _lines); }
        }
    }
}
