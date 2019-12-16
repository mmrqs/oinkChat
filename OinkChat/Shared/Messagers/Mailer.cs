using Shared.Messages;
using System.Threading;

namespace Shared.Messagers
{
    public delegate void MessageEventHandler(object sender, Message message);

    public abstract class Mailer
    {
        protected event MessageEventHandler MessageEvent;

        /// <summary>
        /// It runs the Init()
        /// It starts the threads Sender and Receiver
        /// It enters in an infinite loop (while the cancelation isn't requested) where it listens the keyboard.
        /// </summary>
        public void Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                CoreTask();
            }
        }

        protected virtual void CoreTask()
        {
            Thread.Sleep(3000);
        }

        public abstract void OnMessageReceived(object sender, Message message);
        
        /// <summary>
        /// It allows the Sender subscription to the Client.
        /// </summary>
        /// <param name="method"></param>
        public void Subscription(MessageEventHandler method)
        {
            MessageEvent += method;
        }

        /// <summary>
        /// Raises an event to send a message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        protected void RaiseEvent(object sender, Message message)
        {
            MessageEvent?.Invoke(sender, message);
        }
    }
}
