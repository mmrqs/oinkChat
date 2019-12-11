using System;
using System.Reflection;

namespace Server.Models
{
    [Serializable]
    public class Topic
    {
        public string Title { get; set; }

        public Topic(string title)
        {
            Title = title;
        }

        public override bool Equals(object obj)
        { 
            return obj is Topic topic &&
                   Title == topic.Title;
        }
        
        public override int GetHashCode()
        {
                return HashCode.Combine(Title);
        }
    }
}