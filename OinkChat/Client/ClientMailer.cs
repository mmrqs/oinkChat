using Shared.Messagers;
using Shared.Messages;
using System;

namespace Client
{
    public class ClientMailer : Mailer
    {
        protected override void CoreTask()
        {
            string tamer = Console.ReadLine();
            RaiseEvent(this, new ClientMessage(tamer));
        }

        /// <summary>
        /// It display the received message from Receiver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public override void OnMessageReceived(object sender, Message message)
        {
            Console.WriteLine("$ " + message.Text);
        }
    }
}