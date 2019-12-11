using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Server.Utilities
{
    class Backer<T>
    {
        private T _subject;
        private string _backupName;
        private int _backupInterval;
        private BinaryFormatter _bf;
        private FileStream _fs;

        public Backer(string backupName, int backupInterval)
        {
            _backupName = backupName;
            _backupInterval = backupInterval;
            _bf = new BinaryFormatter();
            _fs = new FileStream(_backupName, FileMode.OpenOrCreate);
        }

        public bool HasData() 
        {
            return _fs.Length != 0; 
        }

        public void SetSubject(T subject)
        {
            _subject = subject;
        }

        public void Backup()
        {
            while (true)
            {
                _fs = new FileStream(_backupName, FileMode.Create);

                _bf.Serialize(_fs, _subject);
                _fs.Close();
                Console.WriteLine(_backupName + " backed up !");
                Thread.Sleep(_backupInterval);
            }
        }

        public T Read()
        {
            T results;
            if (_fs.Length != 0)
            {
                results = (T)_bf.Deserialize(_fs);
            }
            else
            {
                results = default;
            }
            _fs.Close();
            return results;
        }
    }
}
