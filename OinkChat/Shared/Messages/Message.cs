using System;

namespace Shared.Messages
{
    [Serializable]
    public abstract class Message : EventArgs
    {
        public abstract string Text();
    }
}
