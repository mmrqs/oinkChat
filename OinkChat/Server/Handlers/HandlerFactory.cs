using Server.Controllers;
using System;

namespace Server.Handlers
{
    class HandlerFactory
    {
        public IHandler GetHandler(ChatData data, DispatchSession session)
        {
            if(!session.IsLogged)
            {
                return new LogHandler(data, session);
            } 
            else if (session.TopicJoined == null)
            {
                return new TopicHandler(data, session);
            }
            else
            {
                return new ChatHandler(data, session);
            }
            
            throw new NotImplementedException();
        }
    }
}
