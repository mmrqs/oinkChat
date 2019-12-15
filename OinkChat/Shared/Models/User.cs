using System;

namespace Shared.Models
{
    [Serializable]
    public class User
    {
        public string Pseudo { get; set; }
        public string Password { get; set; }

        public User(string pseudo, string password)
        {
            Pseudo = pseudo;
            Password = password;
        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Pseudo == user.Pseudo &&
                   Password == user.Password;
        }

        public override int GetHashCode()
        {
            return (Pseudo + Password).GetHashCode();
        }

        public override string ToString()
        {
            return Pseudo;
        }
    }
}
