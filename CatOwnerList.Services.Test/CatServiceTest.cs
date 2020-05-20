using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using CatOwnerList.Services.External;
using CatOwnerList.Services.External.Models;

namespace CatOwnerList.Services.Test
{
    public class CatServiceTest
    {
        private Mock<IPeopleGetter> peopleGetterMock;
        private ICatService catService;

        public CatServiceTest()
        {
            peopleGetterMock = new Mock<IPeopleGetter>();
            catService = new CatService(peopleGetterMock.Object);
        }

        [Fact]
        public void GetCatNamesGroupedByOwnerGendersAsync_GetsNoData_ReturnsEmpty()
        {
            // Arrange
            peopleGetterMock.Setup(m => m.GetAsync())
                .Returns(Task.FromResult<List<PetOwner>>(null));

            // Act
            var result = catService.GetCatNamesGroupedByOwnerGendersAsync().Result;

            // Assert
            var expected = Enumerable.Empty<IGrouping<string, CatNameOwnerGenderOutput>>();
            Assert.Equal(expected, result);

        }

        [Fact]
        public void GetCatNamesGroupedByOwnerGendersAsync_GetsPeopleWithNoPets_ReturnsEmpty()
        {
            // Arrange
            var noPets = new List<PetOwner>();
            noPets.Add(new PetOwner
            {
                Name = "Bob",
                Age = 23,
                Gender = "Male",
                Pets = null
            });
            peopleGetterMock.Setup(m => m.GetAsync())
                .Returns(Task.FromResult<List<PetOwner>>(noPets));

            // Act
            var result = catService.GetCatNamesGroupedByOwnerGendersAsync().Result;

            // Assert
            var expected = Enumerable.Empty<IGrouping<string, CatNameOwnerGenderOutput>>();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetCatNamesGroupedByOwnerGendersAsync_GetsPeopleWithNoCats_ReturnsEmpty()
        {
            // Arrange
            var dogs = new List<Pet>
            {
                new Pet { Name = "Garfield", Type = "Cat" },
                new Pet { Name = "Fido", Type = "Dog" }
            };
            var noCats = new List<PetOwner>();
            noCats.Add(new PetOwner
            {
                Name = "Bob",
                Age = 23,
                Gender = "Male",
                Pets = dogs
            });
            peopleGetterMock.Setup(m => m.GetAsync())
                .Returns(Task.FromResult(noCats));

            // Act
            var result = catService.GetCatNamesGroupedByOwnerGendersAsync().Result;

            // Assert
            var expected = Enumerable.Empty<IGrouping<string, CatNameOwnerGenderOutput>>();
            Assert.Equal(expected, result);

        }

        [Fact]
        public void GetCatNamesGroupedByOwnerGendersAsync_GetsMaleOnlyCatOwners_ReturnsSingleGrouping()
        {
            // Arrange
            var petOwners = new List<PetOwner>();
            petOwners.Add(new PetOwner
            {
                Name = "Bob",
                Age = 23,
                Gender = "Male",
                Pets = new List<Pet>
                {
                    new Pet { Name = "Garfield", Type = "Cat" },
                    new Pet { Name = "Fido", Type = "Dogs" }
                }
            });
            petOwners.Add(new PetOwner
            {
                Name = "Jack",
                Age = 30,
                Gender = "Male",
                Pets = new List<Pet>
                {
                    new Pet { Name = "Sam", Type = "Cat" },
                    new Pet { Name = "Brix", Type = "Cat" },
                    new Pet { Name = "Rufus", Type = "Dog" }
                }
            });
            peopleGetterMock.Setup(m => m.GetAsync())
                .Returns(Task.FromResult<List<PetOwner>>(petOwners));

            // Act
            var result = catService.GetCatNamesGroupedByOwnerGendersAsync().Result;

            // Assert
            var actualGroupsCount = result.Count();
            Assert.Equal(1, actualGroupsCount);
            var cats = result.FirstOrDefault();
            Assert.Equal("Male", cats.Key);
            Assert.Equal(3, cats.Count());
        }

        [Fact]
        public void GetCatNamesGroupedByOwnerGendersAsync_GetsMixedGenderCatOwners_ReturnsMultipleGroupings()
        {
            // Arrange
            var petOwners = new List<PetOwner>();
            petOwners.Add(new PetOwner
            {
                Name = "Peter",
                Age = 24,
                Gender = "Male",
                Pets = new List<Pet>
                {
                    new Pet { Name = "Charlie", Type = "Dog" },
                    new Pet { Name = "Felix", Type = "Cat" }
                }
            });
            petOwners.Add(new PetOwner
            {
                Name = "Jack",
                Age = 30,
                Gender = "Male",
                Pets = new List<Pet>
                {
                    new Pet { Name = "Simba", Type = "Cat" },
                    new Pet { Name = "Chewy", Type = "Cat" },
                    new Pet { Name = "Rufus", Type = "Dog" }
                }
            });
            petOwners.Add(new PetOwner
            {
                Name = "Cathy",
                Age = 19,
                Gender = "Female",
                Pets = new List<Pet>
                {
                    new Pet { Name = "Sally", Type = "Cat" },
                    new Pet { Name = "Harry", Type = "Cat" },
                    new Pet { Name = "Daisy", Type = "Bird" }
                }
            });
            petOwners.Add(new PetOwner
            {
                Name = "Jess",
                Age = 28,
                Gender = "Female",
                Pets = new List<Pet>
                {
                    new Pet { Name = "Ruby", Type = "Rabbit" }
                }
            });
            peopleGetterMock.Setup(m => m.GetAsync())
                .Returns(Task.FromResult<List<PetOwner>>(petOwners));

            // Act
            var result = catService.GetCatNamesGroupedByOwnerGendersAsync().Result;

            // Assert

            // There must be 2 groupings
            var actualGroupsCount = result.Count();
            Assert.Equal(2, actualGroupsCount);

            // The first grouping must be for cats with "Male" pet owners
            var firstGroup = result.ElementAt(0).Key;

            // The second grouping must be for cats with "Female" pet owners
            Assert.Equal("Male", firstGroup);
            var secondGroup = result.ElementAt(1).Key;
            Assert.Equal("Female", secondGroup);

            // Names must be in alphabetical order
            var catsWithMaleOwners = string.Join(", ", 
                result.ElementAt(0).Select(cat => cat.Name).ToList());
            Assert.Equal("Chewy, Felix, Simba", catsWithMaleOwners);
            var catsWithFemaleOwners = string.Join(", ",
                result.ElementAt(1).Select(cat => cat.Name).ToList());
            Assert.Equal("Harry, Sally", catsWithFemaleOwners);
        }
    }
}
