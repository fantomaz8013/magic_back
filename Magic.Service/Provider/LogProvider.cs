using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Enums;

namespace Magic.Service.Provider
{
    public class LogProvider : ILogProvider
    {
        private DataBaseContext _dbContext;

        public LogProvider(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Write(LogLevelEnum Level, string text)
        {
            var newLog = new Log
            {
                Text = text,
                CreatedDate = DateTime.UtcNow,
                Level = Level,
                Category = LogCategoryEnum.None,
            };
            try
            {
                await _dbContext.Log.AddAsync(newLog);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        public async Task Write(string text)
        {
            await Write(LogLevelEnum.None, text);
        }

        public async Task WriteError(string text)
        {
            await Write(LogLevelEnum.Error, text);
        }

        public async Task WriteWarning(string text)
        {
            await Write(LogLevelEnum.Warning, text);
        }

        public async Task WriteInformation(string text)
        {
            await Write(LogLevelEnum.Information, text);
        }
    }
}
