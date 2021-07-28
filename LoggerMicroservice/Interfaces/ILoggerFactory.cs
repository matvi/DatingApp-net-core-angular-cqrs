using LoggerMicroservice.Common;

namespace LoggerMicroservice.Interfaces
{
    public interface ILoggerFactory
    {
        ILoggerStrategy GetLogger(Logger option);
    }
}