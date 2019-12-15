using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    [Serializable]
    public abstract class Message : EventArgs
    {
        public abstract string Text();
    }
}
