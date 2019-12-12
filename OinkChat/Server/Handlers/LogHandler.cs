using Server.Controllers;
using Server.Models;
using Shared.Messages;

namespace Server.Handlers
{
    class LogHandler : IHandler
    {
        private ChatData _data;
        private DispatchSession _session;

        public LogHandler(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
        }

        public Message Handle(Message input)
        {
            string[] words = input.ToString().Split(" ");
            if(words.Length != 3)
            {
                return new DumbMessage("The correct syntax is <signin|signup> <pseudo> <password>");
            }

            User claim = new User(words[1], words[2]);

            return (words[0]) switch
            {
                "signin" => Signin(claim),
                "signup" => Signup(claim),
                _ => new DumbMessage("The correct syntax is <signin|signup> <pseudo> <password>"),
            };
        }

        private Message Signin(User claim)
        {
            if (_data.AuthUser(claim))
            {
                _session.IsLogged = true;
                return new DumbMessage("You have successfully been logged ; welcome, " + claim.Pseudo);
            } else
            {
                return new DumbMessage("Incorrect pseudo and / or password.");
            }
        }

        private Message Signup(User claim)
        {
             return _data.AddUser(claim) ? 
                new DumbMessage("An account was created for user " + claim.Pseudo) :
                new DumbMessage("The user " + claim.Pseudo + " already exists."); ;
        }
    }
}
