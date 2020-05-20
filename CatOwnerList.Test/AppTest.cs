using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using CatOwnerList.Services;
using CatOwnerList;

namespace CatOwnList.Test
{
    public class AppTest
    {
        private Mock<ILogger<App>> logger;
        private Mock<ICatService> catServiceMock;
        private App app;
        private StringWriter outputWriter;
        private StringReader inputReader;
        private string input = "a";

        public AppTest()
        {
            logger = new Mock<ILogger<App>>();
            catServiceMock = new Mock<ICatService>();
            outputWriter = new StringWriter();
            inputReader = new StringReader(input);
            app = new App(logger.Object, catServiceMock.Object, outputWriter, inputReader);
        }

        [Fact]
        public void RunAsync_GetsNoData_ReturnsNoInfoText()
        {
            // Arrange
            catServiceMock.Setup(m => m.GetCatNamesGroupedByOwnerGendersAsync())
                .Returns(Task.FromResult<IEnumerable<IGrouping<string, CatNameOwnerGenderOutput>>>(null));

            // Act
            app.RunAsync().Wait();
            var result = outputWriter.ToString();

            // Assert
            var expected = App.NilInfoText + Environment.NewLine 
                + App.PressAnyKeyToContinue + Environment.NewLine;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void RunAsync_GetsMaleOnlyCatOwners_ReturnsSingleList()
        {
            // Arrange
            var list = new List<CatNameOwnerGenderOutput>
            {
                new CatNameOwnerGenderOutput { Name = "Brutus", OwnerGender = "Male" },
                new CatNameOwnerGenderOutput { Name = "Felix", OwnerGender = "Male" },
            };
            var grouping = list
                .OrderBy(p => p.Name)
                .GroupBy(p => p.OwnerGender);

            catServiceMock.Setup(m => m.GetCatNamesGroupedByOwnerGendersAsync())
                .Returns(Task.FromResult(grouping));

            // Act
            app.RunAsync().Wait();
            var result = outputWriter.ToString();

            // Assert
            // "Male" owned cats.
            // And cat names in alphabetical order.
            var expected = "Male" + Environment.NewLine
                + App.PetNamePointer + "Brutus" + Environment.NewLine
                + App.PetNamePointer + "Felix" + Environment.NewLine
                + Environment.NewLine
                + App.PressAnyKeyToContinue + Environment.NewLine;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void RunAsync_GetshMixedGenderCatOwners_ReturnsMultipleList()
        {
            // Arrange
            var list = new List<CatNameOwnerGenderOutput>
            {
                new CatNameOwnerGenderOutput { Name = "Sam", OwnerGender = "Female" },
                new CatNameOwnerGenderOutput { Name = "Brek", OwnerGender = "Male" },
                new CatNameOwnerGenderOutput { Name = "Felix", OwnerGender = "Male" },
                new CatNameOwnerGenderOutput { Name = "Chelsea", OwnerGender = "Female" },
            };
            var grouping = list
                .OrderBy(p => p.Name)
                .GroupBy(p => p.OwnerGender); 

            catServiceMock.Setup(m => m.GetCatNamesGroupedByOwnerGendersAsync())
                .Returns(Task.FromResult(grouping));

            // Act
            app.RunAsync().Wait();
            var result = outputWriter.ToString();

            // Assert
            // "Male" owned cats followed by "Female" owned cats.
            // And cat names in alphabetical order within their groups.
            var expected = "Male" + Environment.NewLine
                + App.PetNamePointer + "Brek" + Environment.NewLine
                + App.PetNamePointer + "Felix" + Environment.NewLine
                + Environment.NewLine
                + "Female" + Environment.NewLine
                + App.PetNamePointer + "Chelsea" + Environment.NewLine
                + App.PetNamePointer + "Sam" + Environment.NewLine
                + Environment.NewLine
                + App.PressAnyKeyToContinue + Environment.NewLine;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void RunAsync_ReceivesException_ReturnsUnexpectedErrorText()
        {
            // Arrange
            catServiceMock.Setup(m => m.GetCatNamesGroupedByOwnerGendersAsync())
                .Throws(new HttpRequestException());

            // Act
            app.RunAsync().Wait();
            var result = outputWriter.ToString();

            // Assert
            var expected = App.UnexpectedErrorText + Environment.NewLine
                + Environment.NewLine
                + App.PressAnyKeyToContinue + Environment.NewLine;
            Assert.Equal(expected, result);
        }
    }
}
