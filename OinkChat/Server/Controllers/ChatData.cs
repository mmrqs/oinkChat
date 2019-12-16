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
    /// <summary>
    /// It allows us to manipulate the topic list, user list and user online list
    /// </summary>
    class ChatData
    {
        private Semaphore _usersSemaphore;
        private Semaphore _topicSemaphore;
        private Semaphore _usersOnlineSemaphore;

        private Backer<List<User>> _ub;
        private Backer<List<Topic>> _tb;
        
        private List<Tuple<string, Sender>> _usersOnline;
        
        /// <summary>
        /// Constructor of the class ChatData
        /// It initializes :
        /// - The list of all created Users
        /// - The list of all created Topics
        /// - The list of all the online Users
        ///
        /// - The semaphores constraining the access to ressources
        /// </summary>
        public ChatData()
        {
            _ub = new Backer<List<User>>("users", new List<User>());
            _tb = new Backer<List<Topic>>("topics", new List<Topic>());
            _usersOnline = new List<Tuple<string, Sender>>();
            
            _usersSemaphore = new Semaphore(1, 1);
            _usersOnlineSemaphore = new Semaphore(1, 1);
            _topicSemaphore = new Semaphore(1, 1);
        }

        /// <summary>
        /// Authenticates the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true if the user is successfully authenticated</returns>
        public bool AuthUser(User user)
        {
            _usersSemaphore.WaitOne();
            User result = _ub.Subject.Find(u => u.Equals(user));
            _usersSemaphore.Release();
            return result != null;
        }

        /// <summary>
        /// Add an user into the list of existing users if it doesn't already exist
        /// </summary>
        /// <param name="user"></param>
        /// <returns>return true if the user doesn't exist and is added</returns>
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
        
        /// <summary>
        /// Add a Topic into the list of existing Topics if it doesn't already exist
        /// </summary>
        /// <param name="topic"></param>
        /// <returns>return true if the Topic doesn't exist and is added</returns>
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

        /// <summary>
        /// Get the list of existing topics
        /// </summary>
        /// <returns>The list of existing topics</returns>
        public string GetTopicList()
        {
            _topicSemaphore.WaitOne();

            string list = _tb.Subject.Count > 0 ?
                String.Join(Environment.NewLine, _tb.Subject.Select(t => t.Title).ToArray()) : 
                "No avalaible topics.";

            _topicSemaphore.Release();
            return list;
        }

        /// <summary>
        /// Get a Topic according to his title
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The topic</returns>
        public Topic GetTopicByTitle(string name)
        {
            _topicSemaphore.WaitOne();
            Topic res = _tb.Subject.Find(t => t.Title.Equals(name));
            _topicSemaphore.Release();
            return res;
        }

        /// <summary>
        /// Add an online user to the list of the online users
        /// </summary>
        /// <param name="pseudo"></param>
        /// <param name="sender">We add his sender in order to contact him through private messages</param>
        public void AddUserOnline(string pseudo, Sender sender)
        {
            _usersOnlineSemaphore.WaitOne();
            _usersOnline.Add(new Tuple<string, Sender>(pseudo, sender));
            _usersOnlineSemaphore.Release();
        }

        /// <summary>
        /// Get the list of the online users.
        /// </summary>
        /// <returns>A String containing all the users with new lines</returns>
        public string GetUsersOnline()
        {
            string listUsersOnline = "";

            _usersOnlineSemaphore.WaitOne();

            foreach(Tuple<string, Sender> t in _usersOnline)
            {
                listUsersOnline += t.Item1 + Environment.NewLine;
            }

            _usersOnlineSemaphore.Release();

            return listUsersOnline;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public void DeleteUserOnline(Sender receiver)
        {
            _usersOnlineSemaphore.WaitOne();
            _usersOnline.Remove(_usersOnline.Find(u => u.Item2 == receiver));
            _usersOnlineSemaphore.Release();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pseudo"></param>
        /// <returns>Sender</returns>
        /// <exception cref="NullReferenceException">If the user isn't in the online users list, we throw a nullReferenceException</exception>
        public Sender GetSenderUser(string pseudo)
        {
            Sender s;
            _usersOnlineSemaphore.WaitOne();
            try {
                 s = _usersOnline.Find(u => u.Item1.Equals(pseudo)).Item2;
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();              
            }
            finally
            {
                _usersOnlineSemaphore.Release();                
            }
            return s;
        }
    }
}