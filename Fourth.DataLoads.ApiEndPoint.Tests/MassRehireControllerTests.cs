using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.ApiEndPoint.Authorization;
using Fourth.DataLoads.ApiEndPoint.Controllers;
using Fourth.DataLoads.Data.Repositories;
using Fourth.DataLoads.Data.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Fourth.DataLoads.ApiEndPoint.Mappers;
using AutoMapper;
using Fourth.DataLoads.Data.Entities;

namespace Fourth.PSLiveDataLoads.ApiEndPoint.Tests
{
    [TestClass]
    public class MassRehireControllerTests
    {
        private List<MassRehireModel> models
            = new List<MassRehireModel>();
        private List<MassRehireModelSerialized> Serializedmodels
            = new List<MassRehireModelSerialized>();
        private Mock<IDataFactory> dataFactory;
        private Mock<IAuthorizationProvider> authorization;
        private IMappingFactory mappingFactory;
        const string ValidGroupID = "426";

        [TestInitialize]
        public void InitializeMassRehireController()
        {
            //Mock the repository
            var repository = new Mock<IStagingRepository<MassRehireModelSerialized>>(MockBehavior.Strict);
            var qRepo = new Mock<IQueueRepository>(MockBehavior.Strict);

            repository.Setup(r => r.SetDataAsync(It.IsAny<UserContext>(),
                It.IsAny<List<MassRehireModelSerialized>>()))
                .ReturnsAsync(new List<DataloadBatch>()
                { new DataloadBatch { BatchID = Guid.NewGuid(), JobID = Guid.NewGuid() } });

            qRepo.Setup(r => r.PushDataAsync(It.IsAny<IEnumerable<DataloadBatch>>()))
                .ReturnsAsync(true);


            //Mock data factory
            dataFactory = new Mock<IDataFactory>(MockBehavior.Strict);

            dataFactory.Setup(d => d.GetMassRehireRepository()).Returns(repository.Object);
            dataFactory.Setup(d => d.GetQueueRepository()).Returns(qRepo.Object);

            //Mock Auth
            authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(s => s.IsAuthorized(It.IsIn<string>(new List<string>() { ValidGroupID }))).Returns(true);

            //Mock Mapper
            mappingFactory = new MappingFactory();
            models.Add(new MassRehireModel { EmployeeNumber="abc"});

            //Serializedmodels = mappingFactory.Mapper.Map<List<MassRehireModelSerialized>>(models);


        }

        [TestMethod]
        public void OnRequestRehireControllerIsNotNull()
        {
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            var controller = new MassRehireController(datafactory.Object, authorization.Object,mappingFactory);
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void Passing_AuthorisableGroupID_ToControllerReturnsOkResult()
        {

            //Arrange
            var controller = new MassRehireController(dataFactory.Object, authorization.Object, mappingFactory);

            // Act
            Task<IHttpActionResult> actionResult = controller.SetDataAsync(ValidGroupID, models);
            
            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkResult));
        }
        [TestMethod]
        public void Passing_NullGroupID_ShouldReturnaBadRequestStatus()
        {
            //Arrange
            var controller = new MassRehireController(dataFactory.Object, authorization.Object, mappingFactory);

            // Act
            Task<IHttpActionResult> actionResult = controller.SetDataAsync(null, models);
            
            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));
        }
        [TestMethod]
        public void Passing_NonNumericGroupID_ReturnsBadRequestStatus()
        {
            //Arrange
            var controller = new MassRehireController(dataFactory.Object, authorization.Object, mappingFactory);
            //Act
            Task<IHttpActionResult> result = controller.SetDataAsync("abc", models);
            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Passing_NullInputModel_ReturnsBadRequestStatus()
        {
            //Arrange
            var controller = new MassRehireController(dataFactory.Object, authorization.Object, mappingFactory);
            //Act
            Task<IHttpActionResult> result = controller.SetDataAsync(ValidGroupID, null);
            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }
        [TestMethod]
        public void MakingUnauthorised_RequestToMassRehire_ReturnsUnauthorisedStatus()
        {
            //Arrange
            authorization.Setup(a => a.IsAuthorized(ValidGroupID)).Returns(false);

            var controller = new MassRehireController(dataFactory.Object, authorization.Object, mappingFactory);
            //Act
            var actionResult = controller.SetDataAsync(ValidGroupID, models);
            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(UnauthorizedResult));
        }
    }
}
