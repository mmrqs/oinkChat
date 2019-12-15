using Server.Controllers;
using System;

namespace Server.Handlers
{
    class HandlerFactory
    {
        public IHandler GetHandler(ChatData data, DispatchSession session, string keyword)
        {
            if(!session.IsLogged)
            {
                return new LogHandler(data, session);
            }
            else if (keyword.Equals("post"))
            {
                return new ChatHandler(data, session);
            }
            else
            {
                return new TopicHandler(data, session);
            }
            
            throw new NotImplementedException();
        }
    }
}
