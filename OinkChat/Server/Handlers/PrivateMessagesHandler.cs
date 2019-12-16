using Server.Controllers;
using Shared.Messages;
using System;

namespace Server.Handlers
{
    class PrivateMessagesHandler : IHandler
    {
        private ChatData _data;
        private DispatchSession _session;

        public PrivateMessagesHandler(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
        }

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

        public Message SendPrivateMessage(CommandMessage message)
        {
            if (message.Target.Equals("") || message.Length == 0 || message[0].Equals(""))
                return Help();

            try
            {
                _data.GetSenderUser(message.Target).ReceiveMessage(null, new PrivateMessage(message.Text, _session.User));
            }
            catch (NullReferenceException)
            {
                return new DumbMessage("The user " + message.Target + " doesn't exist or is offline");
            }           
            return new DumbMessage("Private message has been successfully sent to " + message.Target);
        }

        public Message DisplayOnlineUsers()
        {
            return new DumbMessage("Online users are :", _data.GetUsersOnline());
        }

        public Message Help()
        {
            return new HelpMessage("Send a private message : mp <pseudo> <message>",
                "Display the online users : displayUsers");
        }
    }
}
