using Server.Controllers;
using Shared.Models;
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

        public Message Handle(ClientMessage input)
        {
            if (input.Length != 2)
                return Help();

            User claim = new User(input[0], input[1]);

            return (input.KeyWord) switch
            {
                "signin" => Signin(claim),
                "signup" => Signup(claim),
                _ => Help()
            };
        }

        private Message Signin(User claim)
        {
            if (_data.AuthUser(claim))
            {
                _session.IsLogged = true;
                _session.User = claim;

                _data.AddUserOnline(_session.User.Pseudo, _session.Sender);
                return new DumbMessage("You have successfully been logged ; welcome, " + claim);
            } else
            {
                return new DumbMessage("Incorrect pseudo and / or password");
            }
        }

        private Message Signup(User claim)
        {
             return _data.AddUser(claim) ? 
                new DumbMessage("An account was created for user " + claim.Pseudo) :
                new DumbMessage("The user " + claim.Pseudo + " already exists"); ;
        }

        private Message Help()
        {
            return new HelpMessage("Sign in : signin <pseudo> <password>",
                "Sign up : signup <pseudo> <password>");
        }
    }
}
