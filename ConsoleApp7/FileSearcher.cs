using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DuplicatesFinder
{
    class FileSearcher
    {
        private readonly DirectoryInfo _directoryInfo;
        private readonly ConcurrentBag<FileInfo> _files;

        public FileSearcher(string[] location)
        {
            if (location == null || location.Length != 1 || string.IsNullOrEmpty(location[0]))
                throw new ArgumentException("Empty or unproper directory location.");

            _files = new ConcurrentBag<FileInfo>();
            _directoryInfo = new DirectoryInfo(location[0]);
        }

        public FileInfo[] GetFiles()
        {
            ScanFolderAndSubfolders(_directoryInfo);
            return _files.ToArray();
        }

        private void ScanFolderAndSubfolders(DirectoryInfo directory)
        {
            var internalDirectories = directory.EnumerateDirectories();
            var internalFiles = directory.EnumerateFiles();

            foreach (var file in internalFiles)
                _files.Add(file);

            var tasks = new List<Task>();
            foreach (var dir in internalDirectories)
                tasks.Add(Task.Factory.StartNew(() => ScanFolderAndSubfolders(dir)));

            Task.WaitAll(tasks.ToArray());
        }
    }
}
