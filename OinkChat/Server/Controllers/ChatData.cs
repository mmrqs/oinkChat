using System;
using Server.Models;
using Server.Utilities;
using System.Collections.Generic;
using System.Threading;
using Shared.Messages;

namespace Server.Controllers
{
    class ChatData
    {
        private List<User> _users;
        private List<Topic> _topics;
        private Semaphore _usersSemaphore;
        private Semaphore _topicSemaphore;
        private Backer<List<User>> _userBacker;
        private Backer<List<Topic>> _topicBacker;
        
        public ChatData()
        {
            _userBacker = new Backer<List<User>>("users");
            _topicBacker = new Backer<List<Topic>>("topics");
            
            _users = _userBacker.HasData() ?
                _userBacker.Read() : new List<User>();
            _userBacker.SetSubject(_users);
            
            _topics = _topicBacker.HasData() ?
                _topicBacker.Read() : new List<Topic>();
            
            _topicBacker.SetSubject(_topics);
            _usersSemaphore = new Semaphore(1, 1);
            _topicSemaphore = new Semaphore(1, 1);
        }

        public bool AuthUser(User user)
        {
            _usersSemaphore.WaitOne();
            User result = _users.Find(u => u.Equals(user));
            _usersSemaphore.Release();
            return result != null;
        }

        public bool AddUser(User user)
        {
            _usersSemaphore.WaitOne();
            bool inexists = (_users.Find(u => u.Pseudo.Equals(user.Pseudo)) == null);
            if(inexists)
            {
                _users.Add(user);
                _userBacker.Backup();
            }
            _usersSemaphore.Release();
            return inexists;
        }
        public bool AddTopic(Topic topic)
        {
            
           _topicSemaphore.WaitOne();
            bool inexists = (_topics.Find(u => u.Title.Equals(topic.Title)) == null);
            if(inexists)
            {
                _topics.Add(topic);
            }
            _topicSemaphore.Release();
            return inexists;
        }
 

        public IMessage GetAllTopics()
        {
            string allTopics = "";
            _topicSemaphore.WaitOne();
            foreach (Topic t in _topics)
            {
                allTopics += t.Title + Environment.NewLine;
            }
            _topicSemaphore.Release();
            return new DumbMessage(allTopics);
        }
    }
}