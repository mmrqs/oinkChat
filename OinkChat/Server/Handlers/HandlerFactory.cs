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

            if (session.JoinTopic == null)
            {
                return new TopicHandler(data, session);
            }
            else
            {
                return new MessageHandler(data, session);
            }
            
            throw new NotImplementedException();
        }
    }
}
