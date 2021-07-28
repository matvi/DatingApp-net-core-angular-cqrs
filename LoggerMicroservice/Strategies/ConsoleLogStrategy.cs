using System;
using LoggerMicroservice.Interfaces;

namespace LoggerMicroservice.Strategies
{
    public class ConsoleLogStrategy : ILoggerStrategy
    {
        public void WriteLog(string message)
        {
            Console.WriteLine($"Logging from ConsoleLogStrategy {message}");
        }
    }
}