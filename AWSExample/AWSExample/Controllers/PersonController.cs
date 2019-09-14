using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AWSExample.DataContext;
using AWSExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AWSExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly EFDataContext _dbContext;
        public PersonController(EFDataContext dbcontext)
        {
            _dbContext = dbcontext;
        }


        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<Person>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<Person>>> Get()
        {
            try
            {
                return await _dbContext.Persons.AsNoTracking().ToListAsync<Person>();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Server error while getting all persons. - {ex.StackTrace}");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Person), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<Person>> Get(int id)
        {
            try
            {
                Person person = await _dbContext.Persons.Where(p => p.ID.Equals(id)).FirstOrDefaultAsync();
                if (person != null)
                {
                    return Ok(person);
                }
                else
                {
                    return NotFound($"No Person found with id {id}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Server error while searching person id {id} - {ex.StackTrace}");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Person), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<Person>> Post([FromBody]Person person)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request object");
                }
                else
                {
                    await _dbContext.Persons.AddAsync(person);
                    await _dbContext.SaveChangesAsync();
                    return Created($"https://azwebapiexample20190910011131.azurewebsites.net/api/person/{person.ID}", person);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Person not created. {ex.StackTrace}");
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Person), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<Person>> Put([FromBody]Person person)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request object");
                }
                else
                {
                    Person personResponse = await _dbContext.Persons.Where(p => p.ID.Equals(person.ID)).FirstOrDefaultAsync();
                    if (personResponse != null)
                    {
                        personResponse.Name = person.Name;
                        personResponse.Age = person.Age;
                        await _dbContext.SaveChangesAsync();
                        return Ok(person);
                    }
                    else
                    {
                        return NotFound($"No Person found with id {person.ID}");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Person not updated. {ex.StackTrace}");
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Person), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete([FromBody]Person person)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request object");
                }
                else
                {
                    Person personResponse = await _dbContext.Persons.Where(p => p.ID.Equals(person.ID)).FirstOrDefaultAsync();
                    if (personResponse != null)
                    {
                        _dbContext.Persons.Remove(personResponse);
                        await _dbContext.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return NotFound($"No Person found with id {person.ID}");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Person not deleted. {ex.StackTrace}");
            }
        }

    }
}