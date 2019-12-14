using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Server.Utilities
{
    class Backer<T>
    {
        private T _subject;

        private string _fileName;
        private Semaphore _fileSemaphore;

        private BinaryFormatter _bf;

        public Backer(string fileName, T subject)
        {
            _subject = subject;

            _fileName = fileName;
            _fileSemaphore = new Semaphore(1, 1);

            _bf = new BinaryFormatter();

            Read();
        }

        public bool HasData() 
        {
            _fileSemaphore.WaitOne();

            FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate);
            long len = fs.Length;
            fs.Close();

            _fileSemaphore.Release();
            return len != 0;
        }

        public void Backup()
        {
            _fileSemaphore.WaitOne();

            FileStream fs = new FileStream(_fileName, FileMode.Create);
            _bf.Serialize(fs, _subject);
            fs.Close();

            _fileSemaphore.Release();
            Console.WriteLine(_fileName + " backed up !");
        }

        public void Read()
        {
            _fileSemaphore.WaitOne();

            FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate);
            _subject = fs.Length != 0 ? (T)_bf.Deserialize(fs) : _subject;
            fs.Close();

            _fileSemaphore.Release();
        }

        public T Subject
        {
            get { return _subject; }
        }
    }
}
