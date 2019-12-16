using Server.Controllers;
using System;

namespace Server.Handlers
{
    /// <summary>
    /// Dispatch to a corresponding handler
    /// </summary>
    class HandlerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"> all the server data </param>
        /// <param name="session"> all the client data </param>
        /// <param name="keyword"> a keyword that represents an action to perform</param>
        /// <returns>return an handler </returns>
        /// <exception cref="NotImplementedException"></exception>
        public IHandler GetHandler(ChatData data, DispatchSession session, string keyword)
        {
            if(!session.IsLogged)
            {
                return new LogHandler(data, session);
            }
            else if (keyword.Equals("post") || keyword.Equals("exit"))
            {
                return new ChatHandler(data, session);
            }
            else if (keyword.Equals("mp") || keyword.Equals("displayUsers"))
            {
                return new PrivateMessagesHandler(data, session);
            }
            else
            {
                return new TopicHandler(data, session);
            }
            
            throw new NotImplementedException();
        }
    }
}
