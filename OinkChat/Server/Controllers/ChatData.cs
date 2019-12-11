using Server.Models;
using Server.Utilities;
using System.Collections.Generic;
using System.Threading;

namespace Server.Controllers
{
    class ChatData
    {
        private List<User> _users;
        private Semaphore _usersSemaphore;
        private Backer<List<User>> _userBacker;

        public ChatData()
        {
            _usersSemaphore = new Semaphore(1, 1);

            _userBacker = new Backer<List<User>>("users");

            _users = _userBacker.HasData() ?
                _userBacker.Read() : new List<User>();
            _userBacker.SetSubject(_users);
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
    }
}