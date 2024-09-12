using LoggerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LoggerApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
