using DotNetPersonApi.Models;
using DotNetPersonApi.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetPersonApi.Controllers
{
    [Route("api/v1/persons")]
    public class PersonController : ControllerBase
    {
        private IPersonRepository _personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [HttpPost, Route("Person")]
        public async Task<IActionResult> AddPerson([FromBody, Required]Person person)
        {
            try
            {
                Person addedPerson = await _personRepository.AddPerson(person);
                return Ok(addedPerson);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut, Route("person/{id}")]
        public async Task<IActionResult> UpdatePerson([FromBody, Required]Person person, [FromRoute,  Required] int id)
        {
            try
            {
                Person updatedPerson = await _personRepository.EditPerson(person, id);
                if (updatedPerson == null)
                {
                    return NotFound($"No person with the id {id} exists.");
                }
                return Ok(updatedPerson);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete, Route("person/{id}")]
        public async Task<IActionResult> DeletePerson([FromRoute, Required]int id)
        {
            try
            {
                Person deletedPerson = await _personRepository.DeletePerson(id);
                if(deletedPerson == null)
                {
                    return NotFound($"No person with the id {id} exists.");
                }
                return Ok(deletedPerson);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet, Route("count")]
        public async Task<IActionResult> GetCountOfPersons()
        {
            try
            {
                return Ok(await _personRepository.GetPersonCount());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            try
            {
                return Ok(await _personRepository.GetPerons());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
