using LoggerMicroservice.Common;
using LoggerMicroservice.Interfaces;
using LoggerMicroservice.Strategies;

namespace LoggerMicroservice.Services
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILoggerStrategy GetLogger(Logger option)
        {
            if (option == Logger.Console)
            {
                return new ConsoleLogStrategy();
            }

            return new DataBaseLogStrategy();
        }
    }
}