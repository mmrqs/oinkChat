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

        public ChatData()
        {
            _users = LaunchUserBackup();
            _topics = LaunchTopicBackup();
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
        //A CHANGER
        private static List<User> LaunchUserBackup()
        {
            Backer<List<User>> userBacker = new Backer<List<User>>("users", 5000);

            List<User> users = userBacker.HasData() ?
                userBacker.Read() : new List<User>();

            userBacker.SetSubject(users);

            new Thread(userBacker.Backup).Start();

            return users;
        }
        
        private static List<Topic> LaunchTopicBackup()
        {
            Backer<List<Topic>> topicBacker = new Backer<List<Topic>>("topics", 5000);

            List<Topic> topics = topicBacker.HasData() ?
                topicBacker.Read() : new List<Topic>();

            topicBacker.SetSubject(topics);

            new Thread(topicBacker.Backup).Start();

            return topics;
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
