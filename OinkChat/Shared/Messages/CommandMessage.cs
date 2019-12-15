using System.Linq;

namespace Shared.Messages
{
    public class CommandMessage : ClientMessage
    {
        public string Target { get; }
        public CommandMessage(ClientMessage cm) : base()
        {
            string[] vs = cm.Text.Split(' ');
            KeyWord = cm.KeyWord;
            Target = vs[0];
            Words = vs.Skip(1).ToArray();
        }
    }
}
