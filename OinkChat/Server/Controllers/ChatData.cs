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

        public ChatData()
        {
            _users = LaunchUserBackup();
            _usersSemaphore = new Semaphore(1, 1);
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

        private static List<User> LaunchUserBackup()
        {
            Backer<List<User>> userBacker = new Backer<List<User>>("users", 5000);

            List<User> users = userBacker.HasData() ?
                userBacker.Read() : new List<User>();

            userBacker.SetSubject(users);

            new Thread(userBacker.Backup).Start();

            return users;
        }
    }
}
