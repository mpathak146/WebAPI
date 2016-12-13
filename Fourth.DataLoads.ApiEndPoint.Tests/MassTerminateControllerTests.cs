using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fourth.DataLoads.Data.Entities;
using System.Collections.Generic;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.ApiEndPoint.Mappers;
using Moq;
using AutoMapper;
using Fourth.DataLoads.ApiEndPoint.Controllers;
using Fourth.DataLoads.ApiEndPoint.Authorization;
using Fourth.DataLoads.Data.Repositories;
using Fourth.DataLoads.ApiEndPoint;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Fourth.DataLoads.Data.Models;

namespace Fourth.PSLiveDataLoads.ApiEndPoint.Tests
{
    [TestClass]
    public class MassTerminateControllerTests
    {
        private List<MassTerminationModelSerialized> Serializedmodels 
            = new List<MassTerminationModelSerialized>();
        private List<MassTerminationModel> models
            = new List<MassTerminationModel>();

        public MassTerminateControllerTests()
        {
            SetupModelObjects();
        }

        private void SetupModelObjects()
        {

            // ARRANGE
            //Serializedmodels.Clear();
            models.Clear();

            IMappingFactory mapper = new MappingFactory();

            models.Add(new MassTerminationModel
                { DataLoadBatchId = Guid.NewGuid(),
                EmployeeNumber = "121",
                ErrValidation = "",
                TerminationDate = (DateTime.Now.ToString()),
                TerminationReason = "Training required" });

            models.Add(new MassTerminationModel
            {
                DataLoadBatchId = Guid.NewGuid(),
                EmployeeNumber = "122",
                TerminationDate = (DateTime.Now.ToString()),
                TerminationReason = "Testing termination if it works"
            });

            models.Add(new MassTerminationModel
            {
                DataLoadBatchId = Guid.NewGuid(),
                EmployeeNumber = "123",
                TerminationDate = (DateTime.Now.ToString()),
                TerminationReason = "Over qualified for the job"
            });

            Serializedmodels = mapper.Mapper.Map<List<MassTerminationModelSerialized>>(models);
        }

        [TestMethod]
        public void OnRequestMassTerminateControllerIsNotNull()
        {

            //Mock the repository
            var repository =
                new Mock<IRepository<MassTerminationModelSerialized>>(MockBehavior.Strict);


            //Mock data factory
            var dataFactory =
                new Mock<IDataFactory<MassTerminationModelSerialized>>(MockBehavior.Strict);

            dataFactory.Setup(d => d.GetMassTerminateRepository()).Returns(repository.Object);

            //Mapping factory
            IMappingFactory mapFactory = new MappingFactory();

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);

            authorization.Setup(m => m.IsAuthorized(It.IsAny<string>())).Returns(true);

            repository.Setup(x => x.SetDataAsync(It.IsAny<UserContext>(),
                It.IsAny<List<MassTerminationModelSerialized>>()))
                .ReturnsAsync(new List<DataloadBatch>()
                { new DataloadBatch { BatchID = Guid.NewGuid(), JobID = Guid.NewGuid() } });

            //Mock controller
            var controller =
                new MassTerminateController(dataFactory.Object, authorization.Object, mapFactory);

            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void PassingNullGroupidToControllerResultsBadRequest()
        {

            //Mock the repository
            var repository =
                new Mock<IRepository<MassTerminationModelSerialized>>(MockBehavior.Strict);


            //Mock data factory
            var dataFactory =
                new Mock<IDataFactory<MassTerminationModelSerialized>>(MockBehavior.Strict);

            dataFactory.Setup(d => d.GetMassTerminateRepository()).Returns(repository.Object);

            //Mapping factory
            IMappingFactory mapFactory = new MappingFactory();

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);

            //Mock controller

            var controller = new MassTerminateController(dataFactory.Object, authorization.Object, mapFactory);
            Task<IHttpActionResult> actionResult = controller.SetDataAsync(null, models);

            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));


        }

        [TestMethod]
        public void PassingNullInputFromBodyToControllerResultsBadRequest()
        {

            //Mock the repository
            var repository =
                new Mock<IRepository<MassTerminationModelSerialized>>(MockBehavior.Strict);


            //Mock data factory
            var dataFactory =
                new Mock<IDataFactory<MassTerminationModelSerialized>>(MockBehavior.Strict);

            dataFactory.Setup(d => d.GetMassTerminateRepository()).Returns(repository.Object);

            //Mapping factory
            IMappingFactory mapFactory = new MappingFactory();

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);

            //Mock controller
            var controller = new MassTerminateController(dataFactory.Object, authorization.Object, mapFactory);
            Task<IHttpActionResult> actionResult = controller.SetDataAsync("121", null);

            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));


        }
        [TestMethod]
        public void PassingNonNumericGroupIDToControllerResultsBadRequest()
        {

            //Mock the repository
            var repository =
                new Mock<IRepository<MassTerminationModelSerialized>>(MockBehavior.Strict);


            //Mock data factory
            var dataFactory =
                new Mock<IDataFactory<MassTerminationModelSerialized>>(MockBehavior.Strict);

            dataFactory.Setup(d => d.GetMassTerminateRepository()).Returns(repository.Object);

            //Mapping factory
            IMappingFactory mapFactory = new MappingFactory();

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            

            var controller = new MassTerminateController(dataFactory.Object, authorization.Object, mapFactory);
            Task<IHttpActionResult> actionResult = controller.SetDataAsync("abc", null);

            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void PassingUnAuthorisableGroupIDToControllerThrowsUnauthorizedResult()
        {

            //Mock the repository
            var repository =
                new Mock<IRepository<MassTerminationModelSerialized>>(MockBehavior.Strict);


            //Mock data factory
            var dataFactory =
                new Mock<IDataFactory<MassTerminationModelSerialized>>(MockBehavior.Strict);

            dataFactory.Setup(d => d.GetMassTerminateRepository()).Returns(repository.Object);

            //Mapping factory
            IMappingFactory mapFactory = new MappingFactory();

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(s => s.IsAuthorized(It.IsNotIn<string>(new List<string>() { "426" }))).Returns(false);

            var controller = new MassTerminateController(dataFactory.Object, authorization.Object, mapFactory);


            Task<IHttpActionResult> actionResult = controller.SetDataAsync("123",models);

            Assert.IsInstanceOfType(actionResult.Result, typeof(UnauthorizedResult));

        }

        [TestMethod]
        public void PassingAuthorisableGroupIDToControllerReturnsOkResult()
        {

            //Mock the repository
            var repository =
                new Mock<IRepository<MassTerminationModelSerialized>>(MockBehavior.Strict);
            repository.Setup(r => r.SetDataAsync(It.IsAny<UserContext>(), 
                It.IsAny<List<MassTerminationModelSerialized>>()))
                .ReturnsAsync(new List<DataloadBatch>()
                { new DataloadBatch { BatchID = Guid.NewGuid(), JobID = Guid.NewGuid() } });

            repository.Setup(r => r.PushDataAsync(It.IsAny<IEnumerable<DataloadBatch>>()))
                .ReturnsAsync(true);
            

            //Mock data factory
            var dataFactory =
                new Mock<IDataFactory<MassTerminationModelSerialized>>(MockBehavior.Strict);

            dataFactory.Setup(d => d.GetMassTerminateRepository()).Returns(repository.Object);

            //Mapping factory
            IMappingFactory mapFactory = new MappingFactory();

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(s => s.IsAuthorized(It.IsIn<string>(new List<string>() { "426" }))).Returns(true);

            var controller = new MassTerminateController(dataFactory.Object, authorization.Object, mapFactory);


            Task<IHttpActionResult> actionResult = controller.SetDataAsync("426", models);

            Assert.IsInstanceOfType(actionResult.Result, typeof(OkResult));
        }
        [TestMethod]
        public void SetDataAsyncWithoutSaveFromRepositoryReturnsNotFoundResult()
        {

            //Mock the repository
            var repository =
                new Mock<IRepository<MassTerminationModelSerialized>>(MockBehavior.Strict);
            repository.Setup(r => r.SetDataAsync(It.IsAny<UserContext>(),
                It.IsAny<List<MassTerminationModelSerialized>>()))
                .ReturnsAsync(new List<DataloadBatch>());

            //Mock data factory
            var dataFactory =
                new Mock<IDataFactory<MassTerminationModelSerialized>>(MockBehavior.Strict);

            dataFactory.Setup(d => d.GetMassTerminateRepository()).Returns(repository.Object);

            //Mapping factory
            IMappingFactory mapFactory = new MappingFactory();

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(s => s.IsAuthorized(It.IsIn<string>(new List<string>() { "426" }))).Returns(true);

            var controller = new MassTerminateController(dataFactory.Object, authorization.Object, mapFactory);


            Task<IHttpActionResult> actionResult = controller.SetDataAsync("426", models);

            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }
    }
}
