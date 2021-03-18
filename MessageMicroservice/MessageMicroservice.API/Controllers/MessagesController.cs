using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using MessageMicroservice.Domain.Models;
using MessageMicroservice.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _service;

        public MessagesController(IMessageService service)
        {
            _service = service;
        }

        // GET: api/<MessagesController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Message>>> GetAll()
        {
            var results = await _service.GetAllAsync();
            return Ok(results);
        }

        // GET api/<MessagesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Message>> GetById(string id)
        {
            var message = await _service.GetByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        // GET api/<MessagesController>/author/{author}
        [HttpGet("author/{author}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<List<Message>> GetByAuthorAsync(string author)
        {
            return await _service.GetByAuthorAsync(author);
        }

        // POST api/<MessagesController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Message))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Message>> Post([FromBody] Message postMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var message = new Message()
            {
                Author = postMessage.Author,
                TimeStamp = postMessage.TimeStamp,
                Body = postMessage.Body
            };
            
            await _service.Insert(message);
            return CreatedAtAction(nameof(GetById), new {id = message.Id}, message);
        }

        // PUT/PATCH api/<MessagesController>/5
        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(string id, [FromBody] Message message)
        {
            var oldMessage = await _service.GetByIdAsync(id);
            if(oldMessage == null)
            {
                return NotFound(id);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var newMessage = await _service.UpdateById(id, message);
            return NoContent();
        }

        // DELETE api/<MessagesController>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete()
        {      
            var result = await _service.DeleteAll();
            if (result)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE api/<MessagesController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var oldMessage = await _service.GetByIdAsync(id);
            if (oldMessage == null)
            {
                return NotFound();
            }
            var result = await _service.DeleteById(id);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
