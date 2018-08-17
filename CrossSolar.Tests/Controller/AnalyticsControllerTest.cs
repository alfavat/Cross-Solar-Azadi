using CrossSolar.Controllers;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CrossSolar.Tests.Controller
{
    public class AnalyticsControllerTest
    {
        public AnalyticsControllerTest()
        {
            _analyticsController = new AnalyticsController(_analyticsRepositoryMock.Object,_panelRepositoryMock.Object, _dayAnalyticsRepositoryMock.Object);
        }
        private readonly Mock<IPanelRepository> _panelRepositoryMock = new Mock<IPanelRepository>();

        private readonly AnalyticsController _analyticsController;
        private readonly Mock<IAnalyticsRepository> _analyticsRepositoryMock = new Mock<IAnalyticsRepository>();
        private readonly Mock<IDayAnalyticsRepository> _dayAnalyticsRepositoryMock = new Mock<IDayAnalyticsRepository>();
        [Fact]
        public async Task _GetPanelById()
        {
            // arrange

            // Act
            var ex = await _analyticsController.Get("0");
            // Assert
            Assert.Equal(Assert.IsType<ContentResult>(ex).Content, ("-100"));
        }
        [Trait("test", "CanAddTheoryMemberDataMethod")]
        [Theory]
        [MemberData(nameof(GetData), parameters: 2)]
        public async Task _PostData(int Id,long KiloWatt)
        {
            // Arrange
            string panelId = "1";
            var data = new OneHourElectricityModel() {
                Id=Id,
                KiloWatt=KiloWatt
            };
            // Act
            var result = await _analyticsController.Post(panelId,data);
            // Assert
            Assert.NotNull(result);
            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }
        public static IEnumerable<object[]> GetData(int numTests)
        {
            var allData = new List<object[]>
        {
            new object[] { 1,2000 },
            new object[] { 2,5000},
            new object[] { -1,0 }
        };
            return allData.Take(numTests);
        }



        [Fact]
        public async Task _PostTestPanelIdNullOrEmpty()
        {
            // Arrange
            string panelId = "";
            var data = new OneHourElectricityModel()
            {
                Id = 1,
                KiloWatt = 14000
            };
            // Act
            var result = await _analyticsController.Post(panelId, data);
            // Assert
            Assert.Equal(Assert.IsType<ContentResult>(result).Content,("-1"));
        }
        [Fact]
        [Trait("get", "getData")]
        public void GetAll()
        {
            // Arrange
            // Act
            var data= _dayAnalyticsRepositoryMock.Object.GetPanelData("1");
            // Assert
            Assert.True(data!=null);
        }        
    }
}
