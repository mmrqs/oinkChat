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
            return (input.KeyWord) switch
            {
                "mp" => SendPrivateMessage(input[0], input.Text.Remove(0,input[0].Length)),
                "displayUsers" => DisplayOnlineUsers(),
                _ => Help()
            };
        }


        public Message SendPrivateMessage(String pseudo, String m )
        {
            try
            {
                _data.getSenderUser(pseudo).ReceiveMessage(null, new PrivateMessage(m, _session.User.Pseudo));
            }
            catch (NullReferenceException)
            {
                return new DumbMessage("The user " + pseudo + " doesn't exist or is offline");
            }           
            return new DumbMessage("Private message has been sent successfully to " + pseudo);
        }

        public Message DisplayOnlineUsers()
        {
            return new DumbMessage(_data.GetUsersOnline());
        }

        public Message Help()
        {
            return new DumbMessage("You can :",
                "Send a private message : mp <pseudo> <message>",
                "Display the online users : displayUsers");
        }
    }
}
