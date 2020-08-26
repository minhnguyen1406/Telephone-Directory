using System;
using System.Collections.Generic;
using System.Text;

namespace TelephoneDirectoryApp.Contracts
{
    public interface ILogger
    { 
        void LogError(string message);
        void LogInfo(string message);
        void LogWarning(string message);
    }
}
