using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DuplicatesFinder
{
    class ResultSaver
    {
        private const string _fileName = "results.txt";
        private readonly string _localPath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
        private const string _separator = "--------------------------------------------------------------";

        public string SaveResult(Dictionary<byte[], List<string>> keyValuePairs)
        {
            var saveFilePath = Path.Combine(_localPath, _fileName);

            using (var sw = new StreamWriter(path: saveFilePath, append: false, encoding: Encoding.UTF8))
            {
                foreach (var sameFiles in keyValuePairs)
                {
                    if (sameFiles.Value.Count < 2)
                        continue;

                    foreach (var filePath in sameFiles.Value)
                        sw.WriteLine(filePath);
                    
                    sw.WriteLine(_separator);
                }
            }

            return saveFilePath;
        }
    }
}
