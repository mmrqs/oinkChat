using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Messages
{
    /// <summary>
    /// The message sent by the client
    /// </summary>
    [Serializable]
    public class ClientMessage : Message
    {
        public string KeyWord { get; set; }
        protected string[] Words;

        /// <summary>
        /// Constructor of the class
        ///
        /// Initializes :
        ///
        /// string Keyword: The first word of the message (can be post, mp etc...)
        /// string [] Words : the rest of the message
        /// </summary>
        /// <param name="text"> Text sent by the client </param>
        public ClientMessage(string text)
        {
            string[] vs = text.Split(' ');
            KeyWord = vs[0];
            Words = vs.Skip(1).ToArray();
        }

        /// <summary>
        /// Used by CommandMessage
        /// </summary>
        protected ClientMessage() { }
        
        /// <summary>
        /// allow us to access the content of the tab Words
        /// </summary>
        /// <param name="index"></param>
        public string this[int index]
        {
            get { return Words[index]; }
        }

        /// <summary>
        /// return the length of the content of the message sent by the client minus the keyword
        /// </summary>
        public int Length { get { return Words.Length; } }

        /// <summary>
        /// override the Text method of message
        /// return the text sent by the client minus the keyword
        /// </summary>
        public override string Text
        {
            get { return String.Join(" ", Words); }
        }
    }
}
