using System;
using LoggerMicroservice.Interfaces;

namespace LoggerMicroservice.Strategies
{
    public class DataBaseLogStrategy : ILoggerStrategy
    {
        public void WriteLog(string message)
        {
            Console.WriteLine($"Writting bug in database {message}");
        }
    }
}