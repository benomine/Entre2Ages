using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using MediaMicroservice.Domain.Models;
using MediaMicroservice.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MediaMicroservice.API.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [ApiController]
    public class MediasController : Controller
    {
        private readonly MediaDbContextFactory _factory;

        public MediasController(MediaDbContextFactory factory)
        {
            _factory = factory;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Media>))]
        public async Task<IEnumerable<Media>> GetAll()
        {
            var context = _factory.CreateDbContext();
            var medias = await context.Medias.ToListAsync();
            return medias;
        }

        [HttpGet("id:Guid")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Media))]
        public async Task<ActionResult<Media>> GetById(Guid id)
        {
            var context = _factory.CreateDbContext();
            var media = await context.Medias.FirstOrDefaultAsync(m => m.Id == id);
            if (media == null)
            {
                return NotFound();
            }

            return Ok(media);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Media>> Create([FromBody] Media media)
        {
            var context = _factory.CreateDbContext();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await context.Medias.AddAsync(media);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id = media.Id}, media);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteAll()
        {
            var context = _factory.CreateDbContext();
            context.Medias.RemoveRange();
            var result = await context.SaveChangesAsync();
            return Accepted(result > 0);
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var context = _factory.CreateDbContext();
            var media = await context.Medias.FirstOrDefaultAsync(m => m.Id == id);
            if (media == null)
            {
                return NotFound();
            }
            context.Medias.Remove(media);
            var result = await context.SaveChangesAsync();
            return Accepted(result > 0);
        }
    }
}
