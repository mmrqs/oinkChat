using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public abstract class Message : EventArgs
    {
        public abstract new string ToString();
    }
}
