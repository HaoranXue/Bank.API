using System;

namespace Bank.API
{ 
    public interface ILoggerService : IService
    {
        void Debug<T>(string message, Exception ex = null);
        void Info<T>(string message, Exception ex = null);
        void Warn<T>(string message, Exception ex = null);
        void Error<T>(string message, Exception ex = null);
    }
}
