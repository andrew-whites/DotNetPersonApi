using DotNetPersonApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetPersonApi.Service
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonContext _personContext;

        public PersonRepository(PersonContext personContext)
        {
            _personContext = personContext;
        }

        public async Task<Person> AddPerson(Person person)
        {
            person.Id = 0;
            var addedPerson = await _personContext.Persons.AddAsync(person);
            _personContext.SaveChanges();
            return addedPerson.Entity;
        }

        public async Task<Person> DeletePerson(int id)
        {
            Person personToDelete = await _personContext.Persons.FindAsync(id);
            if(personToDelete != null)
            {
                _personContext.Persons.Attach(personToDelete);
                _personContext.Persons.Remove(personToDelete);
                _personContext.SaveChanges();
            }
            return personToDelete;
        }

        public async Task<Person> EditPerson(Person person, int id)
        {
            Person personToUpdate = await _personContext.Persons.FindAsync(id);
            if(personToUpdate != null)
            {
                personToUpdate = UpdatePersonEntity(personToUpdate, person);
                _personContext.SaveChanges();
            }
            return personToUpdate;
        }

        public async Task<List<Person>> GetPerons()
        {
            return await _personContext.Persons.ToListAsync<Person>();
        }

        public async Task<long> GetPersonCount()
        {
            return await _personContext.Persons.LongCountAsync<Person>();
        }

        private Person UpdatePersonEntity(Person personToUpdate, Person updatedPerson)
        {
            personToUpdate.FirstName = updatedPerson.FirstName;
            personToUpdate.LastName = updatedPerson.LastName;
            return personToUpdate;
        }
    }
}
