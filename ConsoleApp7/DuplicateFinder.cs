using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace ConsoleApp7
{
    class DuplicateFinder
    {
        private readonly FileInfo[] _fileInfos;
        private readonly IMessageService _messager;

        public DuplicateFinder(FileInfo[] fileInfos, IMessageService messageService)
        {
            _fileInfos = fileInfos;
            _messager = messageService;
        }

        public Dictionary<byte[], List<string>> GetDuplicates()
        {
            var fileHashes = new ConcurrentDictionary<string, byte[]>();

            foreach (var file in _fileInfos)
            {
                try
                {
                    var fileMd5 = CreateMD5(file);
                    fileHashes.TryAdd(file.FullName, fileMd5);
                    _messager.LogInfo($"{ByteArrayToString(fileMd5),-35}{file.Name}");
                }
                catch (Exception ex)
                {
                    _messager.LogError(ex, $"Problem when creating MD5 from file {file.FullName}");
                }
            }

            //Parallel.ForEach(_fileInfos, (file) => {
            //    try
            //    {
            //        var fileMd5 = CreateMD5(file);
            //        fileHashes.TryAdd(file.FullName, fileMd5);
            //        _messager.LogInfo($"{ByteArrayToString(fileMd5),-35}{file.Name}");
            //    }
            //    catch (Exception ex)
            //    {
            //        _messager.LogError(ex, $"Problem when creating MD5 from file {file.FullName}");
            //    }
            //});

                var grouped = fileHashes.GroupBy(x => x.Value, new BitArrayComparer())
                .ToDictionary(x => x.Key, y => y.Select(z => z.Key)
                .ToList());
            return grouped;
        }

        private byte[] CreateMD5(FileInfo file)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            using (var fs = file.Open(FileMode.Open))
            {
                fs.Position = 0;
                return md5.ComputeHash(fs);
            }
        }

        private string ByteArrayToString(byte[] obj)
        {
            var byteString = string.Empty;

            foreach (var singleByte in obj)
                byteString += singleByte.ToString("X");

            return byteString;
        }

        class BitArrayComparer : IEqualityComparer<byte[]>
        {
            public bool Equals(byte[] x, byte[] y)
            {
                if (x == null || y == null || x.Length != y.Length)
                    return false;

                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] != y[i])
                        return false;
                }

                return true;
            }

            public int GetHashCode(byte[] obj)
            {
                int hash = 0;

                foreach (var singleByte in obj)
                    hash ^= singleByte.GetHashCode();

                return hash;
            }
        }
    }
}
