using Server.Controllers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Handlers
{
    /// <summary>
    /// Constrains the handler to implement the method Handler that takes an clientMessage and returns a Message 
    /// </summary>
    interface IHandler
    {
        Message Handle(ClientMessage input);
    }
}
