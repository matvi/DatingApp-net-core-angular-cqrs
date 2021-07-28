using System;
using System.Threading.Tasks;
using Common.Notifications;
using LoggerMicroservice.Common;
using LoggerMicroservice.Interfaces;
using LoggerMicroservice.Services;
using MassTransit;
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
            
            var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host("amqp://guest:guest@localhost:5672");
                config.ReceiveEndpoint("temp-queue", c =>
                {
                    c.Handler<RegisterUserCreatedCommand>(ctx =>
                    {
                        logService.WriteLog($"User created, user id: {ctx.Message.UserId}", Logger.Console);
                        return Task.CompletedTask;
                    });
                });
            });
            
            bus.Start();
            
            Console.WriteLine("Press any key to scape");
            Console.ReadKey();
            
            bus.Stop();

        }
    }
}