using DotNetPersonApi.Controllers;
using DotNetPersonApi.Models;
using DotNetPersonApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetPersonApiTests.Controllers
{
    [TestClass]
    public class PersonControllerTests
    {
        private PersonController _personController;

        private IPersonRepository _personRepository;

        private IPersonRepository GetInMemoryReository()
        {
            var options = new DbContextOptionsBuilder<PersonContext>()
                .UseInMemoryDatabase(databaseName: "Persons")
                .Options;
            PersonContext personContext = new PersonContext(options);

            PersonRepository personRepository = new PersonRepository(personContext);

            return personRepository;
        }

        [TestInitialize]
        public void Setup()
        {
            _personRepository = GetInMemoryReository();
            _personController = new PersonController(_personRepository);
        }

        [TestMethod]
        public async Task WhenPersonAddedPersonIsReturned()
        {
            Person personToAdd = new Person
            {
                FirstName = "John",
                LastName = "Doe"
            };
            long personCountBefore = await _personRepository.GetPersonCount();
            Person addedPerson = GetValueFromOkObjectResult<Person>(await _personController.AddPerson(personToAdd) as OkObjectResult);
            long personCountAfter = await _personRepository.GetPersonCount();

            Assert.IsTrue(--personCountAfter == personCountBefore);
            Assert.AreEqual(personToAdd.FirstName, addedPerson.FirstName);
            Assert.AreEqual(personToAdd.LastName, addedPerson.LastName);

            await _personController.DeletePerson(personToAdd.Id);
        }

        [TestMethod]
        public async Task WhenAddingPersonIdIsAutoGenereated()
        {
            int badId = 100;
            Person personToAdd = new Person()
            {
                Id = badId,
                FirstName = "John",
                LastName = "Doe"
            };

            Person addedPerson = GetValueFromOkObjectResult<Person>(await _personController.AddPerson(personToAdd) as OkObjectResult);

            Assert.AreNotEqual(badId, addedPerson.Id);
            Assert.AreEqual(personToAdd.FirstName, addedPerson.FirstName);
            Assert.AreEqual(personToAdd.LastName, addedPerson.LastName);
        }

        [TestMethod]
        public async Task WhenPersonUpdatedDbIsUpdated()
        {
            string firstName = "John";
            string lastName = "Doe";

            Person updatedPerson = new Person
            {
                FirstName = "Doe",
                LastName = "John"
            };

            Person addedPerson = GetValueFromOkObjectResult<Person>(await _personController.AddPerson(
                new Person()
                {
                    FirstName = firstName,
                    LastName = lastName
                }) as OkObjectResult);
            Person updatedPersonResult = GetValueFromOkObjectResult<Person>(await _personController.UpdatePerson(
                new Person()
                {
                    FirstName = lastName,
                    LastName = firstName
                },
                addedPerson.Id) as OkObjectResult);

            Assert.AreEqual(addedPerson.Id, updatedPersonResult.Id);
            Assert.AreNotEqual(firstName, updatedPersonResult.FirstName);
            Assert.AreNotEqual(lastName, updatedPersonResult.LastName);

            await _personController.DeletePerson(updatedPerson.Id);
        }

        [TestMethod]
        public async Task UpdatingPersonWithNoRecordReturnsNotFound()
        {
            int badId = 100;
            Person updatedPerson = new Person()
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var updatedResult = await _personController.UpdatePerson(updatedPerson, badId);

            Assert.IsInstanceOfType(updatedResult, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeletingPersonRemovesPerson()
        {
            Person personToDelete = new Person()
            {
                FirstName = "John",
                LastName = "Doe"
            };

            Person addedPerson = GetValueFromOkObjectResult<Person>(await _personController.AddPerson(personToDelete) as OkObjectResult);
            List<Person> listOfPersons = GetValueFromOkObjectResult<List<Person>>(await _personController.GetPersons() as OkObjectResult);

            Assert.IsTrue(listOfPersons.Contains(personToDelete));

            Person deletedPerson = GetValueFromOkObjectResult<Person>(await _personController.DeletePerson(addedPerson.Id) as OkObjectResult);
            listOfPersons = GetValueFromOkObjectResult<List<Person>>(await _personController.GetPersons() as OkObjectResult);

            Assert.IsFalse(listOfPersons.Contains(deletedPerson));
            Assert.AreEqual(addedPerson, deletedPerson);
        }

        [TestMethod]
        public async Task DeletingPersonWithNoRecorReturnsNotFound()
        {
            int badId = 100;

            var deleteResult = await _personController.DeletePerson(badId);

            Assert.IsInstanceOfType(deleteResult, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetCountsReturnsCorrectNumberAdded()
        {
            Person person1 = new Person
            {
                FirstName = "John",
                LastName = "Doe"
            };
            Person person2 = new Person
            {
                FirstName = "Mary",
                LastName = "Shelly"
            };
            Person person3 = new Person
            {
                FirstName = "Patrick",
                LastName = "Bateman"
            };
            await _personController.AddPerson(person1);
            await _personController.AddPerson(person2);
            await _personController.AddPerson(person3);
            long counts = GetValueFromOkObjectResult<long>(await _personController.GetCountOfPersons() as OkObjectResult);

            Assert.AreEqual(counts, 3);
            await _personController.DeletePerson(person1.Id);
            await _personController.DeletePerson(person2.Id);
            await _personController.DeletePerson(person3.Id);
        }

        [TestMethod]
        public async Task GetPersonsReturnsCorrectNumberAddedPersons()
        {
            Person person1 = new Person
            {
                FirstName = "John",
                LastName = "Doe"
            };
            Person person2 = new Person
            {
                FirstName = "Mary",
                LastName = "Shelly"
            };
            Person person3 = new Person
            {
                FirstName = "Patrick",
                LastName = "Bateman"
            };
            await _personController.AddPerson(person1);
            await _personController.AddPerson(person2);
            await _personController.AddPerson(person3);
            long counts = GetValueFromOkObjectResult<long>(await _personController.GetCountOfPersons() as OkObjectResult);
            List<Person> listOfPersons = GetValueFromOkObjectResult<List<Person>>(await _personController.GetPersons() as OkObjectResult);

            Assert.AreEqual(counts, 3);
            Assert.IsTrue(counts == listOfPersons.Count);
            Assert.IsTrue(listOfPersons.Contains(person1));
            Assert.IsTrue(listOfPersons.Contains(person2));
            Assert.IsTrue(listOfPersons.Contains(person3));

            await _personController.DeletePerson(person1.Id);
            await _personController.DeletePerson(person2.Id);
            await _personController.DeletePerson(person3.Id);
        }

        [TestMethod]
        public async Task AddingPersonsAddsAllPersons()
        {
            List<Person> personsToAdd = new List<Person>()
            {
               new Person
                {
                    FirstName = "John",
                    LastName = "Doe"
                },
                new Person
                {
                    FirstName = "Mary",
                    LastName = "Shelly"
                },
                new Person
                {
                    FirstName = "Patrick",
                    LastName = "Bateman"
                }
            };

            List<Person> listOfAddedPersons = GetValueFromOkObjectResult<List<Person>>(await _personController.AddPersons(personsToAdd) as OkObjectResult);

            Assert.IsTrue(personsToAdd.Count == listOfAddedPersons.Count);

            foreach(Person p in personsToAdd)
            {
                Assert.IsTrue(listOfAddedPersons.Contains(p));
            }

            foreach(Person p in listOfAddedPersons)
            {
                await _personController.DeletePerson(p.Id);
            }
        }

        private T GetValueFromOkObjectResult<T>(OkObjectResult actionResult)
        {
            return (T)actionResult.Value;
        }
    }
}
