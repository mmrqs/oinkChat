using Shared.Messagers;
using Shared.Messages;
using System;

namespace Client
{
    public class ClientMailer : Mailer
    {
        /// <summary>
        /// This method will listens what the user enters in the keyboard and then raises an event that will notify the Sender of the client.
        /// </summary>
        protected override void CoreTask()
        {
            RaiseEvent(this, new ClientMessage(Console.ReadLine()));
        }

        /// <summary>
        /// It display the received message from Receiver of the client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"> the message received from Receiver </param>
        public override void OnMessageReceived(object sender, Message message)
        {
            Console.WriteLine("$ " + message.Text);
        }
    }
}