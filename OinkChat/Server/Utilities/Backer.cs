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
        private BinaryFormatter _bf;
        private FileStream _fs;
        private Semaphore _backupSemaphore;

        public Backer(string backupName)
        {
            _backupName = backupName;
            _backupSemaphore = new Semaphore(1, 1);
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
            _backupSemaphore.WaitOne();
            _fs = new FileStream(_backupName, FileMode.Create);

            _bf.Serialize(_fs, _subject);
            _fs.Close();
            _backupSemaphore.Release();
            Console.WriteLine(_backupName + " backed up !");
        }

        public T Read()
        {
            _backupSemaphore.WaitOne();
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
            _backupSemaphore.Release();
            return results;
        }
    }
}
