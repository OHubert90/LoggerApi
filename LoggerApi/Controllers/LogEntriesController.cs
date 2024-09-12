using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoggerApi.Data;
using LoggerApi.Models;

namespace LoggerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogEntriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LogEntriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LogEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogEntry>>> GetLogEntries()
        {
            return await _context.LogEntries.ToListAsync();
        }

        // GET: api/LogEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogEntry>> GetLogEntry(int id)
        {
            var logEntry = await _context.LogEntries.FindAsync(id);

            if (logEntry == null)
            {
                return NotFound();
            }

            return logEntry;
        }

        // POST: api/LogEntries
        [HttpPost]
        public async Task<ActionResult<LogEntry>> PostLogEntry(LogEntry logEntry)
        {
            _context.LogEntries.Add(logEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLogEntry), new { id = logEntry.Id }, logEntry);
        }

        // PUT: api/LogEntries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogEntry(int id, LogEntry logEntry)
        {
            if (id != logEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(logEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogEntryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/LogEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogEntry(int id)
        {
            var logEntry = await _context.LogEntries.FindAsync(id);
            if (logEntry == null)
            {
                return NotFound();
            }

            _context.LogEntries.Remove(logEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogEntryExists(int id)
        {
            return _context.LogEntries.Any(e => e.Id == id);
        }
    }
}
