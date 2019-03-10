using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace DuplicatesFinder
{
    class MessageService : IMessageService
    {
        private const string _outputFileName = "output.txt";
        private readonly string _location = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        private readonly StreamWriter _streamWriter;
        private object _locker = new object();

        public MessageService()
        {
            var fileLocation = Path.Combine(_location, _outputFileName);
            _streamWriter = new StreamWriter(fileLocation);
        }

        public void LogInfo(string message)
        {
            var fullMessage = DateTime.Now.ToString("hh:mm:ss.fff") + " [INF] " + message;

            Console.WriteLine(fullMessage);

            lock(_locker)
            {
                _streamWriter.WriteLine(fullMessage);
            }
            
        }

        public void LogError(Exception ex, string message)
        {
            var fullMessage = DateTime.Now.ToString("hh:mm:ss.fff") + " [ERR] " + message;

            Console.WriteLine(fullMessage);
            Console.WriteLine(ex);

            lock (_locker)
            {
                _streamWriter.WriteLine(fullMessage);
                _streamWriter.WriteLine(ex);
            }
            
        }
    }
}
