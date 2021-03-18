using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventMicroservice.Domain.Models;
using EventMicroservice.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace EventMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class EventsController : ControllerBase
    {
        private readonly EventDbContextFactory _contextFactory;

        public EventsController(EventDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // GET: api/Events
        public async Task<ActionResult<IEnumerable<Event>>> GetAll()
        {
            var context = _contextFactory.CreateDbContext();
            return await context.Events.ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Event>> Get(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            var @event = await context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, Event eventupdate)
        {
            if (id != eventupdate.Id)
            {
                return BadRequest();
            }
            var context = _contextFactory.CreateDbContext();
            var @event = await context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            @event.Author = eventupdate.Author;
            @event.Guest = eventupdate.Guest;
            @event.Status = eventupdate.Status;
            @event.Subject = eventupdate.Subject;
            @event.EpochEnd = eventupdate.EpochEnd;
            @event.EpochStart = eventupdate.EpochStart;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Event>> Post(Event @event)
        {
            var newEvent = new Event()
            {
                Author = @event.Author,
                Guest = @event.Guest,
                EpochEnd = @event.EpochEnd,
                EpochStart = @event.EpochStart,
                Status = @event.Status,
                Subject = @event.Subject
            };

            var context = _contextFactory.CreateDbContext();
            context.Events.Add(newEvent);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = newEvent.Id }, newEvent);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            var @event = await context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            context.Events.Remove(@event);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            return context.Events.Any(e => e.Id == id);
        }
    }
}
