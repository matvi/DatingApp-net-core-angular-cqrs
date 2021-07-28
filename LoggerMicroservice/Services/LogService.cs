using System;
using LoggerMicroservice.Interfaces;

namespace LoggerMicroservice.Services
{
    public class LogService : ILogService
    {
        private readonly ILoggerStrategy _loggerStrategy;

        public LogService(ILoggerStrategy loggerStrategy)
        {
            _loggerStrategy = loggerStrategy;
        }


        public void WriteLog(String message)
        {
            _loggerStrategy.WriteLog(message);
        }
    }
    
}