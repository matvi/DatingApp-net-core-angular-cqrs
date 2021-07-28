using System;
using LoggerMicroservice.Common;
using LoggerMicroservice.Services;

namespace LoggerMicroservice
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO:depending on file configuration you can choose if logging using console or database
            Console.WriteLine("ILogger Microservice initialized. Logging in console");

            var factory = new LoggerFactory();
            var loggerStrategy = factory.GetLogger(Logger.Console);
            var loggerService = new LogService(loggerStrategy);
            loggerService.WriteLog("this is my message");
        }
    }
}