using Server.Controllers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Handlers
{
    interface IHandler
    {
        IMessage Handle(IMessage input);
    }
}
