using System;
using LoggerMicroservice.Common;
using LoggerMicroservice.Interfaces;

namespace LoggerMicroservice.Services
{
    public class LogService : ILogService
    {
        private readonly ILoggerFactory _loggerFactory;

        public LogService(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        
        public void WriteLog(String message, Logger on)
        {
            var loggerStrategy = _loggerFactory.GetLogger(on);
            loggerStrategy.WriteLog(message);
        }
    }
    
}