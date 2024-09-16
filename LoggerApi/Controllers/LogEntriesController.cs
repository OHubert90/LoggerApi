using LoggerApi.Models;
using LoggerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoggerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogEntriesController : ControllerBase
    {
        private readonly LogEntriesService _logEntryService;

        public LogEntriesController(LogEntriesService logEntryService)
        {
            _logEntryService = logEntryService;
        }

        [HttpGet("source")]
        public async Task<ActionResult<IEnumerable<LogEntry>>> GetSourceLogEntries()
        {
            var sourceLogEntries = await _logEntryService.GetSourceLogEntriesAsync();
            return Ok(sourceLogEntries);
        }

        [HttpGet("destination")]
        public async Task<ActionResult<IEnumerable<LogEntry>>> GetDestinationLogEntries()
        {
            var destinationLogEntries = await _logEntryService.GetDestinationLogEntriesAsync();
            return Ok(destinationLogEntries);
        }

        [HttpGet("source/{id}")]
        public async Task<ActionResult<LogEntry>> GetSourceLogEntryById(int id)
        {
            var logEntry = await _logEntryService.GetSourceLogEntryByIdAsync(id);

            if (logEntry == null)
                return NotFound();

            return Ok(logEntry);
        }

        [HttpGet("destination/{id}")]
        public async Task<ActionResult<LogEntry>> GetDestinationLogEntryById(int id)
        {
            var logEntry = await _logEntryService.GetDestinationLogEntryByIdAsync(id);

            if (logEntry == null)
                return NotFound();

            return Ok(logEntry);
        }

        [HttpPost("source")]
        public async Task<ActionResult<LogEntry>> CreateSourceLogEntry(LogEntry logEntry)
        {
            var createdLogEntry = await _logEntryService.CreateSourceLogEntryAsync(logEntry);
            return CreatedAtAction(nameof(GetSourceLogEntryById), new { id = createdLogEntry.Id }, createdLogEntry);
        }

        [HttpPost("destination")]
        public async Task<ActionResult<LogEntry>> CreateDestinationLogEntry(LogEntry logEntry)
        {
            var createdLogEntry = await _logEntryService.CreateDestinationLogEntryAsync(logEntry);
            return CreatedAtAction(nameof(GetDestinationLogEntryById), new { id = createdLogEntry.Id }, createdLogEntry);
        }

        [HttpPut("source/{id}")]
        public async Task<IActionResult> UpdateSourceLogEntry(int id, LogEntry logEntry)
        {
            if (id != logEntry.Id)
                return BadRequest();

            try
            {
                await _logEntryService.UpdateSourceLogEntryAsync(logEntry);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _logEntryService.GetSourceLogEntryByIdAsync(id) == null)
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPut("destination/{id}")]
        public async Task<IActionResult> UpdateDestinationLogEntry(int id, LogEntry logEntry)
        {
            if (id != logEntry.Id)
                return BadRequest();

            try
            {
                await _logEntryService.UpdateDestinationLogEntryAsync(logEntry);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _logEntryService.GetDestinationLogEntryByIdAsync(id) == null)
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("source/{id}")]
        public async Task<IActionResult> DeleteSourceLogEntry(int id)
        {
            await _logEntryService.DeleteSourceLogEntryAsync(id);
            return NoContent();
        }

        [HttpDelete("destination/{id}")]
        public async Task<IActionResult> DeleteDestinationLogEntry(int id)
        {
            await _logEntryService.DeleteDestinationLogEntryAsync(id);
            return NoContent();
        }
    }
}