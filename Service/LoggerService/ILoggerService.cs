﻿using Core.Enums;

namespace Services.LoggerService
{
    public interface ILoggerService<T> where T : class
    {
        Task LogErrorAsync(string source, Exception ex, LogImportance logImportance = LogImportance.Low);
        Task LogErrorAsync(string source, string error, LogImportance logImportance = LogImportance.Low);
    }
}