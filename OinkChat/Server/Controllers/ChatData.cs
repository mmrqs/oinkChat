using System;
using Shared.Models;
using Server.Utilities;
using System.Collections.Generic;
using System.Threading;
using Shared.Messages;
using System.Linq;
using Shared.Messagers;

namespace Server.Controllers
{
    class ChatData
    {
        private Semaphore _usersSemaphore;
        private Semaphore _topicSemaphore;
        private Semaphore _usersOnlineSemaphore;

        private Backer<List<User>> _ub;
        private Backer<List<Topic>> _tb;
        
        private List<Tuple<String, Sender>> _usersOnline;
        
        public ChatData()
        {
            _ub = new Backer<List<User>>("users", new List<User>());
            _tb = new Backer<List<Topic>>("topics", new List<Topic>());
            _usersOnline = new List<Tuple<string, Sender>>();
            
            _usersSemaphore = new Semaphore(1, 1);
            _usersOnlineSemaphore = new Semaphore(1, 1);
            _topicSemaphore = new Semaphore(1, 1);
        }

        public bool AuthUser(User user)
        {
            _usersSemaphore.WaitOne();
            User result = _ub.Subject.Find(u => u.Equals(user));
            _usersSemaphore.Release();
            return result != null;
        }

        public bool AddUser(User user)
        {
            _usersSemaphore.WaitOne();
            bool inexists = _ub.Subject.Find(u => u.Pseudo.Equals(user.Pseudo)) == null;
            if(inexists)
            {
                _ub.Subject.Add(user);
                _ub.Backup();
            }
            _usersSemaphore.Release();
            return inexists;
        }
        public bool AddTopic(Topic topic)
        {
            _topicSemaphore.WaitOne();
            bool inexists = _tb.Subject.Find(t => t.Title.Equals(topic.Title)) == null;
            if(inexists)
            {
                _tb.Subject.Add(topic);
                _tb.Backup();
            }
            _topicSemaphore.Release();
            return inexists;
        }
        public string GetTopicList()
        {
            _topicSemaphore.WaitOne();

            string list = _tb.Subject.Count > 0 ?
                string.Join(Environment.NewLine, _tb.Subject.Select(t => t.Title).ToArray()) : 
                "No avalaible topics.";

            _topicSemaphore.Release();
            return list;
        }
        public Topic GetTopicByTitle(string name)
        {
            _topicSemaphore.WaitOne();
            Topic res = _tb.Subject.Find(t => t.Title.Equals(name));
            _topicSemaphore.Release();
            return res;
        }
        public void AddUserOnline(String pseudo, Sender sender)
        {
            _usersOnlineSemaphore.WaitOne();
            _usersOnline.Add(new Tuple<string, Sender>(pseudo, sender));
            _usersOnlineSemaphore.Release();
        }
        public String GetUsersOnline()
        {
           String listUsersOnline = "";

            _usersOnlineSemaphore.WaitOne();

            foreach(Tuple<String, Sender> t in _usersOnline)
            {
                listUsersOnline += t.Item1 + Environment.NewLine;
            }

            _usersOnlineSemaphore.Release();

            return listUsersOnline;
        }
        
        public void DeleteUserOnline(Sender receiver)
        {
            _usersOnlineSemaphore.WaitOne();
            _usersOnline.Remove( _usersOnline.Find(u => u.Item2 == receiver));
            _usersOnlineSemaphore.Release();
        }

        public Sender getSenderUser(String pseudo)
        {
            _usersOnlineSemaphore.WaitOne();
            Sender s = _usersOnline.Find(u => u.Item1.Equals(pseudo)).Item2;
            _usersOnlineSemaphore.Release();
            return s;
        }
    }
}