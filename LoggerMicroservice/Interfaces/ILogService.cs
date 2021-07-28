using System;

namespace LoggerMicroservice.Interfaces
{
    public interface ILogService
    {
        void WriteLog(String message);
    }
}