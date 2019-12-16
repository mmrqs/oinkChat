using Server.Controllers;
using Shared.Messages;
using System;

namespace Server.Handlers
{
    class PrivateMessagesHandler : IHandler
    {
        private ChatData _data;
        private DispatchSession _session;

        /// <summary>
        /// Class constructor
        ///
        /// It initializes :
        ///
        /// - data : all the server data
        /// - session : all the data related to the client
        /// </summary>
        /// <param name="data"></param>
        /// <param name="session"></param>
        public PrivateMessagesHandler(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
        }

        /// <summary>
        /// It handle the message sent by the client
        /// </summary>
        /// <param name="input">The message sent by the client</param>
        /// <returns>a Message</returns>
        public Message Handle(ClientMessage input)
        {
            CommandMessage cm = new CommandMessage(input);
            return input.KeyWord switch
            {
                "mp" => SendPrivateMessage(cm),
                "displayUsers" => DisplayOnlineUsers(),
                _ => null
            };
        }

        /// <summary>
        /// Allows the client to send private messages
        ///
        /// </summary>
        /// <param name="message">CommandMessage</param>
        /// <returns></returns>
        public Message SendPrivateMessage(CommandMessage message)
        {
            if (message.Target.Equals("") || message.Length == 0 || message[0].Equals(""))
                return Help();

            try
            {
                //We get the Sender related to the user and we transmit the message to the ReceiveMessage
                _data.GetSenderUser(message.Target).ReceiveMessage(null, new PrivateMessage(message.Text, _session.User));
            }
            catch (NullReferenceException)
            {
                return new DumbMessage("The user " + message.Target + " doesn't exist or is offline");
            }           
            return new DumbMessage("Private message has been successfully sent to " + message.Target);
        }

        /// <summary>
        /// Allows to display all the online users pseudo
        /// </summary>
        /// <returns>A Message with all the online users pseudo</returns>
        public Message DisplayOnlineUsers()
        {
            return new DumbMessage("Online users are :", _data.GetUsersOnline());
        }

        /// <summary>
        /// Helper corresponding to the ChatHandler class
        /// </summary>
        /// <returns> an help message </returns>
        public Message Help()
        {
            return new HelpMessage("Send a private message : mp <pseudo> <message>",
                "Display the online users : displayUsers");
        }
    }
}
