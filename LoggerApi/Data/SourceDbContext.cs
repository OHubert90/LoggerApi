using LoggerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LoggerApi.Data
{
    public class SourceDbContext : DbContext
    {
        public SourceDbContext(DbContextOptions<SourceDbContext> options) : base(options)
        {
        }
        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
