using DotNetPersonApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetPersonApi.Service
{
    public interface IPersonRepository
    {
        Task<Person> AddPerson(Person person);

        Task<Person> EditPerson(Person person);

        Task<Person> DeletePerson(int id);

        Task<long> GetPersonCount();

        Task<Person> GetPerons();
    }
}
