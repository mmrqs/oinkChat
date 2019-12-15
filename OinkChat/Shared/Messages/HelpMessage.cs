using System;

namespace Shared.Messages
{
    [Serializable]
    public class HelpMessage : DumbMessage
    {
        public HelpMessage(params string[] lines) : base(lines) { }

        public override string Text
        {
            get 
            {
                return String.Join(Environment.NewLine, 
                    "HELP - You can :",
                    String.Join(Environment.NewLine, _lines),
                    "And remember, all the commands are case-sensitive !");
            }
        }
    }
}