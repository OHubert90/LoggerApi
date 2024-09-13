using LoggerApi.Data;
using LoggerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoggerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogEntriesController : ControllerBase
    {
        private readonly SourceDbContext _sourceContext;
        private readonly DestinationDbContext _destinationContext;

        public LogEntriesController(SourceDbContext sourceContext, DestinationDbContext destinationContext)
        {
            _sourceContext = sourceContext;
            _destinationContext = destinationContext;
        }

        // Get all Log Entries from Source Database
        [HttpGet("source")]
        public async Task<ActionResult<IEnumerable<LogEntry>>> GetSourceLogEntries()
        {
            var sourceLogEntries = await _sourceContext.LogEntries.ToListAsync();
            return Ok(sourceLogEntries);
        }

        // Get all Log Entries from Destination Database
        [HttpGet("destination")]
        public async Task<ActionResult<IEnumerable<LogEntry>>> GetDestinationLogEntries()
        {
            var destinationLogEntries = await _destinationContext.LogEntries.ToListAsync();
            return Ok(destinationLogEntries);
        }

        // Get Log Entry by ID from Source Database
        [HttpGet("source/{id}")]
        public async Task<ActionResult<LogEntry>> GetSourceLogEntryById(int id)
        {
            var logEntry = await _sourceContext.LogEntries.FindAsync(id);

            if (logEntry == null)
                return NotFound();

            return Ok(logEntry);
        }

        // Get Log Entry by ID from Destination Database
        [HttpGet("destination/{id}")]
        public async Task<ActionResult<LogEntry>> GetDestinationLogEntryById(int id)
        {
            var logEntry = await _destinationContext.LogEntries.FindAsync(id);

            if (logEntry == null)
                return NotFound();

            return Ok(logEntry);
        }

        // Add new Log Entry to Source Database
        [HttpPost("source")]
        public async Task<ActionResult<LogEntry>> CreateSourceLogEntry(LogEntry logEntry)
        {
            logEntry.MpnGroupGuid = Guid.NewGuid(); 
            _sourceContext.LogEntries.Add(logEntry);
            await _sourceContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSourceLogEntryById), new { id = logEntry.Id }, logEntry);
        }

        // Add new Log Entry to Destination Database
        [HttpPost("destination")]
        public async Task<ActionResult<LogEntry>> CreateDestinationLogEntry(LogEntry logEntry)
        {
            logEntry.MpnGroupGuid = Guid.NewGuid(); 
            _destinationContext.LogEntries.Add(logEntry);
            await _destinationContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDestinationLogEntryById), new { id = logEntry.Id }, logEntry);
        }

        // Update Log Entry in Source Database
        [HttpPut("source/{id}")]
        public async Task<IActionResult> UpdateSourceLogEntry(int id, LogEntry logEntry)
        {
            if (id != logEntry.Id)
                return BadRequest();

            _sourceContext.Entry(logEntry).State = EntityState.Modified;

            try
            {
                await _sourceContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SourceLogEntryExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Update Log Entry in Destination Database
        [HttpPut("destination/{id}")]
        public async Task<IActionResult> UpdateDestinationLogEntry(int id, LogEntry logEntry)
        {
            if (id != logEntry.Id)
                return BadRequest();

            _destinationContext.Entry(logEntry).State = EntityState.Modified;

            try
            {
                await _destinationContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationLogEntryExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Delete Log Entry from Source Database
        [HttpDelete("source/{id}")]
        public async Task<IActionResult> DeleteSourceLogEntry(int id)
        {
            var logEntry = await _sourceContext.LogEntries.FindAsync(id);
            if (logEntry == null)
                return NotFound();

            _sourceContext.LogEntries.Remove(logEntry);
            await _sourceContext.SaveChangesAsync();

            return NoContent();
        }

        // Delete Log Entry from Destination Database
        [HttpDelete("destination/{id}")]
        public async Task<IActionResult> DeleteDestinationLogEntry(int id)
        {
            var logEntry = await _destinationContext.LogEntries.FindAsync(id);
            if (logEntry == null)
                return NotFound();

            _destinationContext.LogEntries.Remove(logEntry);
            await _destinationContext.SaveChangesAsync();

            return NoContent();
        }

        // Helper methods to check if a LogEntry exists in Source/Destination Database
        private bool SourceLogEntryExists(int id)
        {
            return _sourceContext.LogEntries.Any(e => e.Id == id);
        }

        private bool DestinationLogEntryExists(int id)
        {
            return _destinationContext.LogEntries.Any(e => e.Id == id);
        }
    }
}
