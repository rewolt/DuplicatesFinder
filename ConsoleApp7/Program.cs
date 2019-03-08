using System;

namespace ConsoleApp7
{
    class Program
    {
        static void Main(string[] args)
        {
            var messanger = new MessageService();

            var fileSearcher = new FileSearcher(args);
            var files = fileSearcher.GetFiles();

            var duplicateFinder = new DuplicateFinder(files, messanger);
            var duplicates = duplicateFinder.GetDuplicates();

            var resultSaver = new ResultSaver();
            var fileLocation = resultSaver.SaveResult(duplicates);

            messanger.LogInfo("\nResults saved in:\n" + fileLocation);
            messanger.LogInfo("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
