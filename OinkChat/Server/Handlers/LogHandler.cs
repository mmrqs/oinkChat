using Server.Controllers;
using Shared.Models;
using Shared.Messages;

namespace Server.Handlers
{
    class LogHandler : IHandler
    {
        private ChatData _data;
        private DispatchSession _session;

        /// <summary>
        /// class constructor
        /// </summary>
        /// <param name="data"> all the server data </param>
        /// <param name="session"> all the client data </param>
        public LogHandler(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
        }

        /// <summary>
        /// It handles the message sent by the client
        /// </summary>
        /// <param name="input">ClientMessage</param>
        /// <returns> a Message</returns>
        public Message Handle(ClientMessage input)
        {
            //We verify if the user has correctly entered the pseudo and the password
            if (input.Length != 2)
                return Help();
            
            // input[0] : pseudo input[1] : password
            User claim = new User(input[0], input[1]);

            return (input.KeyWord) switch
            {
                "signin" => Signin(claim),
                "signup" => Signup(claim),
                _ => Help()
            };
        }

        /// <summary>
        /// allow the client to sign in
        /// </summary>
        /// <param name="claim">The User</param>
        /// <returns> A validation Message if he successfully signin or an error message otherwise </returns>
        private Message Signin(User claim)
        {
            //If the user pseudo and user password are correct
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

        /// <summary>
        /// If the user doesn't already exist, we add the User to our User list in the ChatData object
        /// </summary>
        /// <param name="claim">The User to register</param>
        /// <returns>A validation Message if he successfully sign up or an error message otherwise</returns>
        private Message Signup(User claim)
        {
             return _data.AddUser(claim) ? 
                new DumbMessage("An account was created for user " + claim.Pseudo) :
                new DumbMessage("The user " + claim.Pseudo + " already exists"); ;
        }

        /// <summary>
        /// Helper corresponding to the LogHandler class
        /// </summary>
        /// <returns> an help message </returns>
        private Message Help()
        {
            return new HelpMessage("Sign in : signin <pseudo> <password>",
                "Sign up : signup <pseudo> <password>");
        }
    }
}
