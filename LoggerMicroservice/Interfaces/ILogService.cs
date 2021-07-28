using System;
using LoggerMicroservice.Common;

namespace LoggerMicroservice.Interfaces
{
    public interface ILogService
    {
        void WriteLog(String message, Logger option);
    }
}