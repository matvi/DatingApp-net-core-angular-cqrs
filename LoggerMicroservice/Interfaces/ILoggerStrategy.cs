using System;

namespace LoggerMicroservice.Interfaces
{
    public interface ILoggerStrategy
    {
        void WriteLog(String message);
    }
}