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

namespace Fourth.PSLiveDataLoads.ApiEndPoint.Tests
{
    [TestClass]
    public class MassRehireControllerTests
    {
        private List<MassRehireModel> models
            = new List<MassRehireModel>();
        [TestMethod]
        public void OnRequestRehireControllerIsNotNull()
        {
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            var controller = new MassRehireController(datafactory.Object, authorization.Object);
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void PassingAuthorisableGroupIDToControllerReturnsOkResult()
        {

            //Mock the repository
            var repository =
                new Mock<IStagingRepository<MassRehireModelSerialized>>(MockBehavior.Strict);
            var qRepo = new Mock<IQueueRepository>(MockBehavior.Strict);
            repository.Setup(r => r.SetDataAsync(It.IsAny<UserContext>(),
                It.IsAny<List<MassRehireModelSerialized>>()))
                .ReturnsAsync(new List<DataloadBatch>()
                { new DataloadBatch { BatchID = Guid.NewGuid(), JobID = Guid.NewGuid() } });

            qRepo.Setup(r => r.PushDataAsync(It.IsAny<IEnumerable<DataloadBatch>>()))
                .ReturnsAsync(true);


            //Mock data factory
            var dataFactory =
                new Mock<IDataFactory>(MockBehavior.Strict);

            dataFactory.Setup(d => d.GetMassRehireRepository()).Returns(repository.Object);
            dataFactory.Setup(d => d.GetQueueRepository()).Returns(qRepo.Object);

            //Mock Auth
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            authorization.Setup(s => s.IsAuthorized(It.IsIn<string>(new List<string>() { "426" }))).Returns(true);

            var controller = new MassRehireController(dataFactory.Object, authorization.Object);


            Task<IHttpActionResult> actionResult = controller.SetDataAsync("426", models);

            Assert.IsInstanceOfType(actionResult.Result, typeof(OkResult));
        }
    }
}
