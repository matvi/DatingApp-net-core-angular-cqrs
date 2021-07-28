using LoggerMicroservice.Common;
using LoggerMicroservice.Interfaces;

namespace LoggerMicroservice
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogService _logService;

        public Startup(ILoggerFactory loggerFactory, ILogService logService)
        {
            _loggerFactory = loggerFactory;
            _logService = logService;
        }

        public void LogMessage(string message)
        {
            var logger = _loggerFactory.GetLogger(Logger.Console);
            _logService.WriteLog(message);
        }
    }
}