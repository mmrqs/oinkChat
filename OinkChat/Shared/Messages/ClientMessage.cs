using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Messages
{
    [Serializable]
    public class ClientMessage : Message, IEnumerable<string[]>
    {
        public string KeyWord { get; }
        protected string PayLoad;
        protected string[] Words;

        public ClientMessage(string text)
        {
            string[] vs = text.Split(' ');
            KeyWord = vs[0];
            Words = vs.Skip(1).ToArray();
            PayLoad = string.Join(" ", Words);
        }

        public ClientMessage(string keyword, string text)
        {
            KeyWord = keyword;
            PayLoad = text;
            Words = text.Split(' ');
        }

        protected ClientMessage() { }

        public string this[int index]
        {
            get { return Words[index]; }
        }

        public int Length
        {
            get
            {
                return Words.Length;
            }
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            return (IEnumerator<string[]>)Words.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string Text
        {
            get { return PayLoad; }
        }
    }
}
