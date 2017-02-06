using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.ApiEndPoint.Authorization;
using Fourth.DataLoads.ApiEndPoint.Controllers;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Collections.Generic;
using Fourth.DataLoads.Data.Models;

namespace Fourth.PSLiveDataLoads.ApiEndPoint.Tests
{
    [TestClass]
    public class AuditControllerTests
    {
        private IList<DataLoadUploads> dataloads = new List<DataLoadUploads>();
        private IList<ErrorModel> errors = new List<ErrorModel>();
        public AuditControllerTests()
        {
            SetupModelObjects();
        }

        private void SetupModelObjects()
        {
            dataloads.Add(new DataLoadUploads
            {   DataloadType = "MassTermination",
                DateUploaded = DateTime.Now,
                jobID = "TestGUID",
                UploadedBy = "Informatica"
            });
            errors.Add(new ErrorModel
            {
                ClientID = 112,
                EmployeeNumber = "1212",
                ErrorDescription = "Some issue",
                ErrorStatus = 2
            });


        }

        [TestMethod]
        public void OnRequestAuditControllerIsNotNull()
        {
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            var controller = new AuditController(datafactory.Object, authorization.Object);
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void PassingNullGroupidToAuditControllerResultsBadRequest()
        {
            //Mock data factory
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            //Mock Authorization
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);

            var controller = new AuditController(datafactory.Object, authorization.Object);

            Task<IHttpActionResult> actionResult = controller.GetDataAsync(null, null);
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PassingSomeunauthorizableGroupidToAuditControllerGetDataAsyncResultsUnauthorizedResult()
        {
            //Mock data factory
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            //Mock Authorization
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);

            var controller = new AuditController(datafactory.Object, authorization.Object);

            authorization.Setup(m => m.IsAuthorized("1")).Returns(false);

            Task<IHttpActionResult> actionResult = controller.GetDataAsync("1", null);

            Assert.IsInstanceOfType(actionResult.Result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void PassingCorrectGroupidToAuditControllerResultsInProcess()
        {
            //Mock Repository
            var repository = new Mock<IPortalRepository>(MockBehavior.Strict);
            //Mock data factory
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            //Mock Authorization
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);

            datafactory.Setup(d => d.GetPortalRepository()).Returns(repository.Object);

            var controller = new AuditController(datafactory.Object, authorization.Object);

            authorization.Setup(m => m.IsAuthorized("11")).Returns(true);
            repository.Setup(x => x.GetDataLoadUploads(11, "",1))
                .ReturnsAsync(dataloads);

            Task<IHttpActionResult> actionResult = controller.GetDataAsync("11", "");
            
            Assert.AreEqual(((OkNegotiatedContentResult<List<DataLoadUploads>>)actionResult.Result).Content.Capacity, 1);
        }

        [TestMethod]
        public void PassingInvalidGroupidToAuditControllerResultsUnauthorizedResult()
        {
            //Mock Repository
            var repository = new Mock<IPortalRepository>(MockBehavior.Strict);
            //Mock data factory
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            //Mock Authorization
            datafactory.Setup(d => d.GetPortalRepository()).Returns(repository.Object);

            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);

            authorization.Setup(s => s.IsAuthorized(It.IsNotIn<string>(new List<string>() { "426" }))).Returns(false);

            var controller = new AuditController(datafactory.Object, authorization.Object);

            Task<IHttpActionResult> actionResult = controller.GetDataAsync("123", null);
            Assert.IsInstanceOfType(actionResult.Result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void PassingNullGroupidToAuditControllerGetErrorDataAsyncResultsBadRequest()
        {
            //Mock data factory
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            //Mock Authorization
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);

            var controller = new AuditController(datafactory.Object, authorization.Object);

            Task<IHttpActionResult> actionResult = controller.GetErrorDataAsync(null, null);
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PassingSomeUnauthorizableGroupidToAuditControllerGetErrorDataAsyncResultsUnauthorizedResult()
        {
            //Mock data factory
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            //Mock Authorization
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);

            var controller = new AuditController(datafactory.Object, authorization.Object);

            authorization.Setup(m => m.IsAuthorized("1")).Returns(false);

            Task<IHttpActionResult> actionResult = controller.GetErrorDataAsync("1", null);

            Assert.IsInstanceOfType(actionResult.Result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void PassingCorrectGroupidToAuditControllerGetErrorDataAsyncResultsInProcess()
        {
            //Mock Repository
            var repository = new Mock<IPortalRepository>(MockBehavior.Strict);
            //Mock data factory
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            //Mock Authorization
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);

            datafactory.Setup(d => d.GetPortalRepository()).Returns(repository.Object);

            var controller = new AuditController(datafactory.Object, authorization.Object);

            authorization.Setup(m => m.IsAuthorized("11")).Returns(true);
            repository.Setup(x => x.GetDataLoadErrors(11, ""))
                .ReturnsAsync(errors);

            Task<IHttpActionResult> actionResult = controller.GetErrorDataAsync("11", "");

            Assert.AreEqual(((OkNegotiatedContentResult<List<ErrorModel>>)actionResult.Result).Content.Capacity, 1);
        }

        [TestMethod]
        public void PassingInvalidGroupidToAuditControllerGetErrorDataAsyncResultsUnauthorizedResult()
        {
            //Mock Repository
            var repository = new Mock<IPortalRepository>(MockBehavior.Strict);
            //Mock data factory
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            //Mock Authorization
            datafactory.Setup(d => d.GetPortalRepository()).Returns(repository.Object);

            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);

            authorization.Setup(s => s.IsAuthorized(It.IsNotIn<string>(new List<string>() { "426" }))).Returns(false);

            var controller = new AuditController(datafactory.Object, authorization.Object);

            Task<IHttpActionResult> actionResult = controller.GetErrorDataAsync("123", "");
            Assert.IsInstanceOfType(actionResult.Result, typeof(UnauthorizedResult));
        }
    }
}
