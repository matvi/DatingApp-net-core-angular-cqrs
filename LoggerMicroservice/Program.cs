using System;
using LoggerMicroservice.Common;
using LoggerMicroservice.Interfaces;
using LoggerMicroservice.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LoggerMicroservice
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO:depending on file configuration you can choose if logging using console or database
            Console.WriteLine("ILogger Microservice initialized. Logging in console");
            var serviceProvider = new ServiceCollection()
                .AddScoped<ILoggerFactory, LoggerFactory>()
                .AddScoped<ILogService, LogService>()
                .BuildServiceProvider();

            var logService = serviceProvider.GetService<ILogService>();
            logService.WriteLog("my message for DI", Logger.Console);
        }
    }
}