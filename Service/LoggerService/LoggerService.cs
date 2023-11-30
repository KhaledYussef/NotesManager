using Core.Domains.System;
using Core.Enums;

using Data.DbContext;

namespace Services.LoggerService
{
    public class LoggerService<T> : ILoggerService<T> where T : class
    {
        private readonly AppDbContext _dbContext;

        public LoggerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogErrorAsync(string source, Exception ex, LogImportance logImportance = LogImportance.Low)
        {
            Console.WriteLine(String.Format("Error In [{0}.{1}] \r\n {2}", typeof(T).Name, source, ex.Message));
            await _dbContext.SystemErrors.AddAsync(new SystemError(ex));
        }

        public async Task LogErrorAsync(string source, string error, LogImportance logImportance = LogImportance.Low)
        {
            Console.WriteLine(String.Format("Error In [{0}.{1}] \r\n {2}", typeof(T).Name, source, error));
            await _dbContext.SystemErrors.AddAsync(new SystemError(error));
        }
    }
}
