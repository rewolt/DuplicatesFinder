using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp7
{
    interface IMessageService
    {
        void LogError(Exception ex, string message);
        void LogInfo(string message);
    }
}
