using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Messages
{
    public class ClientChatMessage : ClientMessage
    {
        public string Target { get; }
        public ClientChatMessage(ClientMessage cm) : base(cm.KeyWord)
        {
            string[] vs = cm.Text.Split(' ');
            Target = vs[0];
            Words = vs.Skip(1).ToArray();
            PayLoad = string.Join(" ", Words);
        }
    }
}
