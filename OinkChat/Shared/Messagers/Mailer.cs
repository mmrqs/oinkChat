using Shared.Messages;
using System.Threading;

namespace Shared.Messagers
{
    
    /// <summary>
    /// The delegate of the event MessageEvent.
    /// It delegates to the sender method : receiveMessage
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    public delegate void MessageEventHandler(object sender, Message message);

    public abstract class Mailer
    {
        /// <summary>
        /// Event raised when the mailer finished handling the message
        /// </summary>
        protected event MessageEventHandler MessageEvent;

        /// <summary>
        /// It enters in an infinite loop (while the cancelation isn't requested) where it performs the CoreTask.
        /// </summary>
        public void Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                CoreTask();
            }
        }

        /// <summary>
        /// Thread.Sleep(3000) avoid the permanent solicitation of the cpu (it's for performance issues)
        /// </summary>
        protected virtual void CoreTask()
        {
            Thread.Sleep(3000);
        }

        /// <summary>
        /// It receives the message from the Receiver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message">Message received</param>
        public abstract void OnMessageReceived(object sender, Message message);
        
        /// <summary>
        /// It allows the Sender subscription to the Mailer.
        /// </summary>
        /// <param name="method"></param>
        public void Subscription(MessageEventHandler method)
        {
            MessageEvent += method;
        }

        /// <summary>
        /// Raises an event to send a message to the Sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        protected void RaiseEvent(object sender, Message message)
        {
            MessageEvent?.Invoke(sender, message);
        }
    }
}
