using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Server.Utilities
{
    /// <summary>
    /// It allows us to persist the data using serialization;
    /// </summary>
    /// <typeparam name="T">List Topic or List User</typeparam>
    class Backer<T>
    {
        private T _subject;

        private string _fileName;
        private Semaphore _fileSemaphore;

        private BinaryFormatter _bf;

        /// <summary>
        /// Constructor of the Backer class
        /// 
        /// It initializes :
        /// 
        /// - subject : the list of items (Topic and User)
        /// - fileName : the file in which the data is stored (user or topic)
        /// - fileSemaphore : a semaphore that restrains the access to the file.
        /// - bf : the binaryFormatter
        ///
        /// It runs the "Read" function.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="subject"></param>
        public Backer(string fileName, T subject)
        {
            _subject = subject;

            _fileName = fileName;
            _fileSemaphore = new Semaphore(1, 1);

            _bf = new BinaryFormatter();

            Read();
        }

        /// <summary>
        /// Serializes the subject in its own file.
        /// </summary>
        public void Backup()
        {
            _fileSemaphore.WaitOne();

            FileStream fs = new FileStream(_fileName, FileMode.Create);
            _bf.Serialize(fs, _subject);
            fs.Close();

            _fileSemaphore.Release();
            Console.WriteLine(_fileName + " backed up !");
        }

        /// <summary>
        /// Deserializes the subject from its own file.
        /// </summary>
        public void Read()
        {
            _fileSemaphore.WaitOne();

            FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate);
            _subject = fs.Length != 0 ? (T)_bf.Deserialize(fs) : _subject;
            fs.Close();

            _fileSemaphore.Release();
        }

        /// <summary>
        /// It returns the subject (the User list or Topic list)
        /// </summary>
        public T Subject
        {
            get { return _subject; }
        }
    }
}
