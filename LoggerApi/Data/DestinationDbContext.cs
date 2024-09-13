using LoggerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LoggerApi.Data
{
    public class DestinationDbContext : DbContext
    {
        public DestinationDbContext(DbContextOptions<DestinationDbContext> options) : base(options)
        {
        }
        public DbSet<LogEntry> LogEntries { get; set; }
    }
}