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

            if (!session.JoinTopic)
            {
                return new TopicHandler(data, session);
            }
            
            throw new NotImplementedException();
        }
    }
}
