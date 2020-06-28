using DotNetPersonApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetPersonApi.Service
{
    public class PersonRepository : IPersonRepository
    {
        public async Task<Person> AddPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> DeletePerson(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> EditPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetPerons()
        {
            throw new NotImplementedException();
        }

        public async Task<long> GetPersonCount()
        {
            throw new NotImplementedException();
        }
    }
}
