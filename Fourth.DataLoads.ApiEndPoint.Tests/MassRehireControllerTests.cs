using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.ApiEndPoint.Authorization;
using Fourth.DataLoads.ApiEndPoint.Controllers;

namespace Fourth.PSLiveDataLoads.ApiEndPoint.Tests
{
    [TestClass]
    public class MassRehireControllerTests
    {
        [TestMethod]
        public void OnRequestRehireControllerIsNotNull()
        {
            var datafactory = new Mock<IDataFactory>(MockBehavior.Strict);
            var authorization = new Mock<IAuthorizationProvider>(MockBehavior.Strict);
            var controller = new MassRehireController(datafactory.Object, authorization.Object);
            Assert.IsNotNull(controller);
        }
    }
}
