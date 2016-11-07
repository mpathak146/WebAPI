namespace Fourth.DataLoads.ApiEndPoint.Tests
{
    using Fourth.DataLoads.Data.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using Moq;
    using Fourth.DataLoads.Data.Repositories;
    using Fourth.DataLoads.ApiEndPoint.Authorization;
    using Fourth.DataLoads.ApiEndPoint.Controllers;
    using Fourth.DataLoads.Data;
    using System.Web.Http.Results;


    [TestClass]
    public class DefaultHolidayAllowanceTests
    {
        [TestMethod]
        [TestCategory("UnitTests")]
        [Description("A GET request for data pertaining to an organisation id (group id) with no data in the DB should return a 404 Not Found response")]
        public void GetDataByOrg_NoData_ReturnNotFound()
        {
            // ARRANGE

            //an empty list
            var repoOutput = new List<DefaultHolidayAllowance>();
            //var testdata = new DefaultHolidayAllowance { JobTitleID = 1, Allowance = 1, DateCreated = DateTime.UtcNow, JobTitleName = "Cock", LastModified = DateTime.UtcNow, YearsWorked = 1 };
            //repoOutput.Add(testdata);

            //Mock the repository
            var repository = new Mock<IDefaultHolidayAllowanceRepository>(MockBehavior.Strict);
            repository.Setup(m => m.GetDataAsync(It.IsAny<int>())).ReturnsAsync(repoOutput);

            //Mock data factory
            var dataFactory = new Mock<IDataFactory>(MockBehavior.Strict);
            dataFactory.Setup(d => d.GetDefaultHolidayAllowanceRepository()).Returns(repository.Object);

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(m => m.IsAuthorized(It.IsAny<string>())).Returns(true);

            //Mock controller
            var controller = new DefaultHolidayAllowanceController(dataFactory.Object, authorization.Object);

            // ACT
            var response = controller.GetDataAsync("1").Result;

            // ASSERT
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        [TestCategory("UnitTests")]
        [Description("A GET request with an empty organisation id (group id) should return a BadRequest response")]
        public void GetDataByOrg_NoOrg_ReturnNotFound()
        {
            // ARRANGE

            //an empty list
            var repoOutput = new List<DefaultHolidayAllowance>();

            //Mock the repository
            var repository = new Mock<IDefaultHolidayAllowanceRepository>(MockBehavior.Strict);
            repository.Setup(m => m.GetDataAsync(It.IsAny<int>())).ReturnsAsync(repoOutput);

            //Mock data factory
            var dataFactory = new Mock<IDataFactory>(MockBehavior.Strict);
            dataFactory.Setup(d => d.GetDefaultHolidayAllowanceRepository()).Returns(repository.Object);

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(m => m.IsAuthorized(It.IsAny<string>())).Returns(true);

            //Mock controller
            var controller = new DefaultHolidayAllowanceController(dataFactory.Object, authorization.Object);

            // ACT
            var response = controller.GetDataAsync(string.Empty).Result;

            // ASSERT
            Assert.IsInstanceOfType(response, typeof(BadRequestResult));
        }

        [TestMethod]
        [TestCategory("UnitTests")]
        [Description("A GET request for data pertaining to a jobtitleID with no data in the DB should return a 404 Not Found response")]
        public void GetDataByJobTitle_NoData_ReturnNotFound()
        {
            // ARRANGE

            //an empty list
            var repoOutput = new List<DefaultHolidayAllowance>();
            //var testdata = new DefaultHolidayAllowance { JobTitleID = 1, Allowance = 1, DateCreated = DateTime.UtcNow, JobTitleName = "Cock", LastModified = DateTime.UtcNow, YearsWorked = 1 };
            //repoOutput.Add(testdata);

            //Mock the repository
            var repository = new Mock<IDefaultHolidayAllowanceRepository>(MockBehavior.Strict);
            repository.Setup(m => m.GetDataAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(repoOutput);

            //Mock data factory
            var dataFactory = new Mock<IDataFactory>(MockBehavior.Strict);
            dataFactory.Setup(d => d.GetDefaultHolidayAllowanceRepository()).Returns(repository.Object);

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(m => m.IsAuthorized(It.IsAny<string>())).Returns(true);

            //Mock controller
            var controller = new DefaultHolidayAllowanceController(dataFactory.Object, authorization.Object);

            // ACT
            var response = controller.GetDataAsync("1", 999999).Result;

            // ASSERT
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        [TestCategory("UnitTests")]
        [Description("A GET request for data pertaining to a jobtitleID with data in the DB should return a 200 OK response")]
        public void GetDataByJobTitle_WithData_ReturnOK()
        {
            // ARRANGE

            //test data
            var repoOutput = new List<DefaultHolidayAllowance>();
            var testdata = new DefaultHolidayAllowance { JobTitleID = 1, Allowance = 1, DateCreated = DateTime.UtcNow, JobTitleName = "Cock", LastModified = DateTime.UtcNow, YearsWorked = 1 };
            repoOutput.Add(testdata);

            //Mock the repository
            var repository = new Mock<IDefaultHolidayAllowanceRepository>(MockBehavior.Strict);
            repository.Setup(m => m.GetDataAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(repoOutput);

            //Mock data factory
            var dataFactory = new Mock<IDataFactory>(MockBehavior.Strict);
            dataFactory.Setup(d => d.GetDefaultHolidayAllowanceRepository()).Returns(repository.Object);

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(m => m.IsAuthorized(It.IsAny<string>())).Returns(true);

            //Mock controller
            var controller = new DefaultHolidayAllowanceController(dataFactory.Object, authorization.Object);

            // ACT
            var response = controller.GetDataAsync("1", 999999).Result;

            // ASSERT
            //Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult));
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<IEnumerable<DefaultHolidayAllowance>>));
        }

        [TestMethod]
        [TestCategory("UnitTests")]
        [Description("A GET request for data pertaining to an organisation id (group id) with data in the DB should return a 200 OK response")]
        public void GetDataByOrg_WithData_ReturnOK()
        {
            // ARRANGE

            //test data
            var repoOutput = new List<DefaultHolidayAllowance>();
            var testdata = new DefaultHolidayAllowance { JobTitleID = 1, Allowance = 1, DateCreated = DateTime.UtcNow, JobTitleName = "Cock", LastModified = DateTime.UtcNow, YearsWorked = 1 };
            repoOutput.Add(testdata);

            //Mock the repository
            var repository = new Mock<IDefaultHolidayAllowanceRepository>(MockBehavior.Strict);
            repository.Setup(m => m.GetDataAsync(It.IsAny<int>())).ReturnsAsync(repoOutput);

            //Mock data factory
            var dataFactory = new Mock<IDataFactory>(MockBehavior.Strict);
            dataFactory.Setup(d => d.GetDefaultHolidayAllowanceRepository()).Returns(repository.Object);

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(m => m.IsAuthorized(It.IsAny<string>())).Returns(true);

            //Mock controller
            var controller = new DefaultHolidayAllowanceController(dataFactory.Object, authorization.Object);

            // ACT
            var response = controller.GetDataAsync("1").Result;

            // ASSERT
            //Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult));
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<IEnumerable<DefaultHolidayAllowance>>));
        }

        [TestMethod]
        [TestCategory("UnitTests")]
        [Description("A PUT or POST request with an empty payload should return a BadRequest response")]
        public void SetDataByOrg_EmptyPayload_ReturnBadRequest()
        {
            // ARRANGE

            //Mock the repository
            var repository = new Mock<IDefaultHolidayAllowanceRepository>(MockBehavior.Strict);
            repository.Setup(m => m.SetDataAsync(It.IsAny<int>(), It.IsAny<List<DefaultHolidayAllowance>>())).ReturnsAsync(true);

            //Mock data factory
            var dataFactory = new Mock<IDataFactory>(MockBehavior.Strict);
            dataFactory.Setup(d => d.GetDefaultHolidayAllowanceRepository()).Returns(repository.Object);

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(m => m.IsAuthorized(It.IsAny<string>())).Returns(true);

            //Mock controller
            var controller = new DefaultHolidayAllowanceController(dataFactory.Object, authorization.Object);

            // ACT
            var response = controller.SetDataAsync("1", null).Result;

            // ASSERT
            Assert.IsInstanceOfType(response, typeof(BadRequestResult));
        }

        [TestMethod]
        [TestCategory("UnitTests")]
        [Description("A PUT or POST request should return a 200 OK response")]
        public void SetDataByOrg_ReturnOK()
        {
            // ARRANGE

            //test data
            var testInputData = new List<DefaultHolidayAllowance>();
            var testdata = new DefaultHolidayAllowance { JobTitleID = 1, Allowance = 1, DateCreated = DateTime.UtcNow, JobTitleName = "Cock", LastModified = DateTime.UtcNow, YearsWorked = 1 };
            testInputData.Add(testdata);

            //Mock the repository
            var repository = new Mock<IDefaultHolidayAllowanceRepository>(MockBehavior.Strict);
            repository.Setup(m => m.SetDataAsync(It.IsAny<int>(), It.IsAny<List<DefaultHolidayAllowance>>())).ReturnsAsync(true);

            //Mock data factory
            var dataFactory = new Mock<IDataFactory>(MockBehavior.Strict);
            dataFactory.Setup(d => d.GetDefaultHolidayAllowanceRepository()).Returns(repository.Object);

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(m => m.IsAuthorized(It.IsAny<string>())).Returns(true);

            //Mock controller
            var controller = new DefaultHolidayAllowanceController(dataFactory.Object, authorization.Object);
            //controller.Request = new HttpRequestMessage(HttpMethod.Put, "");

            // ACT
            var response = controller.SetDataAsync("1", testInputData).Result;

            // ASSERT
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        [TestCategory("UnitTests")]
        [Description("A DELETE request pertaining to an organisationID and jobtitleID where data exists should return a 200 OK response")]
        public void DeleteExistingDataByOrgAndJobTitle_ReturnOK()
        {
            // ARRANGE

            //Mock the repository
            var repository = new Mock<IDefaultHolidayAllowanceRepository>(MockBehavior.Strict);
            repository.Setup(m => m.DeleteDataAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            //Mock data factory
            var dataFactory = new Mock<IDataFactory>(MockBehavior.Strict);
            dataFactory.Setup(d => d.GetDefaultHolidayAllowanceRepository()).Returns(repository.Object);

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(m => m.IsAuthorized(It.IsAny<string>())).Returns(true);

            //Mock controller
            var controller = new DefaultHolidayAllowanceController(dataFactory.Object, authorization.Object);

            // ACT
            var response = controller.DeleteDataAsync("1", 1).Result;

            // ASSERT
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        [TestCategory("UnitTests")]
        [Description("A DELETE request pertaining to an organisationID and jobtitleID where data does not exist should return a 200 OK response")]
        public void DeleteNonExistingDataByOrgAndJobTitle_ReturnNotFound()
        {
            // ARRANGE

            //Mock the repository
            var repository = new Mock<IDefaultHolidayAllowanceRepository>(MockBehavior.Strict);
            repository.Setup(m => m.DeleteDataAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            //Mock data factory
            var dataFactory = new Mock<IDataFactory>(MockBehavior.Strict);
            dataFactory.Setup(d => d.GetDefaultHolidayAllowanceRepository()).Returns(repository.Object);

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(m => m.IsAuthorized(It.IsAny<string>())).Returns(true);

            //Mock controller
            var controller = new DefaultHolidayAllowanceController(dataFactory.Object, authorization.Object);

            // ACT
            var response = controller.DeleteDataAsync("1", 1).Result;

            // ASSERT
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
    }
}