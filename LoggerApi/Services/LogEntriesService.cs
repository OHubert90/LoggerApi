using LoggerApi.Data;
using LoggerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LoggerApi.Services
{
    public class LogEntriesService
    {
        private readonly SourceDbContext _sourceContext;
        private readonly DestinationDbContext _destinationContext;

        public LogEntriesService(SourceDbContext sourceContext, DestinationDbContext destinationContext)
        {
            _sourceContext = sourceContext;
            _destinationContext = destinationContext;
        }

        public async Task<List<LogEntry>> GetSourceLogEntriesAsync()
        {
            return await _sourceContext.LogEntries.ToListAsync();
        }

        public async Task<List<LogEntry>> GetDestinationLogEntriesAsync()
        {
            return await _destinationContext.LogEntries.ToListAsync();
        }

        public async Task<LogEntry?> GetSourceLogEntryByIdAsync(int id)
        {
            return await _sourceContext.LogEntries.FindAsync(id);
        }

        public async Task<LogEntry?> GetDestinationLogEntryByIdAsync(int id)
        {
            return await _destinationContext.LogEntries.FindAsync(id);
        }

        public async Task<LogEntry> CreateSourceLogEntryAsync(LogEntry logEntry)
        {
            _sourceContext.LogEntries.Add(logEntry);
            await _sourceContext.SaveChangesAsync();
            return logEntry;
        }

        public async Task<LogEntry> CreateDestinationLogEntryAsync(LogEntry logEntry)
        {
            _destinationContext.LogEntries.Add(logEntry);
            await _destinationContext.SaveChangesAsync();
            return logEntry;
        }

        public async Task UpdateSourceLogEntryAsync(LogEntry logEntry)
        {
            _sourceContext.Entry(logEntry).State = EntityState.Modified;
            await _sourceContext.SaveChangesAsync();
        }

        public async Task UpdateDestinationLogEntryAsync(LogEntry logEntry)
        {
            _destinationContext.Entry(logEntry).State = EntityState.Modified;
            await _destinationContext.SaveChangesAsync();
        }

        public async Task DeleteSourceLogEntryAsync(int id)
        {
            var logEntry = await _sourceContext.LogEntries.FindAsync(id);
            if (logEntry != null)
            {
                _sourceContext.LogEntries.Remove(logEntry);
                await _sourceContext.SaveChangesAsync();
            }
        }

        public async Task DeleteDestinationLogEntryAsync(int id)
        {
            var logEntry = await _destinationContext.LogEntries.FindAsync(id);
            if (logEntry != null)
            {
                _destinationContext.LogEntries.Remove(logEntry);
                await _destinationContext.SaveChangesAsync();
            }
        }

        public async Task<bool> EntryExistsInDestinationAsync(int id)
        {
            return await _destinationContext.LogEntries.AnyAsync(e => e.Id == id);
        }

        public async Task CopyLogEntryToDestinationAsync(int sourceId)
        {
            var sourceEntry = await _sourceContext.LogEntries
                .Where(e => e.Id == sourceId)
                .FirstOrDefaultAsync();

            if (sourceEntry == null)
            {
                throw new InvalidOperationException($"Log entry with Id {sourceId} not found in the source database.");
            }

            var destinationEntry = new LogEntry
            {
                Manufacturer = sourceEntry.Manufacturer,
                Mpn = sourceEntry.Mpn,
                MpnGroupGuid = sourceEntry.MpnGroupGuid
            };
            _destinationContext.LogEntries.Add(destinationEntry);
            await _destinationContext.SaveChangesAsync();
        }
    }
}