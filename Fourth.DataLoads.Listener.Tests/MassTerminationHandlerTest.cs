using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fourth.Orchestration.Model.People;
using Fourth.DataLoads.Data.Interfaces;
using System.Threading.Tasks;
using Fourth.DataLoads.Listener.Handlers;
using Fourth.Orchestration.Messaging;


namespace Fourth.DataLoads.Listener.Tests
{
    [TestClass]
    public class MassTerminationHandlerTest: TestBase
    {
        MessageRelayHandler handler;
        public MassTerminationHandlerTest()
        {
            this.MassTerminateService.Setup(x => x.ProcessPayload(It.IsAny<Commands.DataloadRequest>(), It.IsAny<IDataFactory>())).ReturnsAsync(true).Verifiable();

        }
        [TestInitialize]
        public void Setup()
        {
            handler= new MessageRelayHandler(this.MassTerminateService.Object, this.MassRehireService.Object, It.IsAny<IDataFactory>());
        }

        [TestMethod]
        public void OnException_MassTerminationHandler_ShouldReturn_Fatal()
        {
            this.MassTerminateService.Setup(x => x.ProcessPayload(It.IsAny<Commands.DataloadRequest>(), 
                It.IsAny<IDataFactory>())).Throws(new Exception());

            Commands.DataloadRequest message = GetPayload();


            var result = handler.HandleAsync(message, "TrackingId");
            Assert.IsTrue(result.Result == MessageHandlerResult.Fatal);
        }

        [TestMethod]
        public void OnVerifiablePayload_MassTerminationHandler_Should_Process()
        {
            Commands.DataloadRequest message = GetPayload();


            var result = handler.HandleAsync(message, "TrackingId");

            this.MassTerminateService.Verify(m => m.ProcessPayload(It.IsAny<Commands.DataloadRequest>(), It.IsAny<IDataFactory>()));
        }

        [TestMethod]
        public void OnsqlException_MassTerminationHandler_ShouldReturn_Fatal()
        {
            var sqlException = new ExceptionHelper().WithErrorCode(50000)
                .WithErrorMessage("Database exception occured...")
                .Build();

            this.MassTerminateService.Setup(x => x.ProcessPayload(It.IsAny<Commands.DataloadRequest>(), 
                It.IsAny<IDataFactory>())).Throws(sqlException);

            Commands.DataloadRequest message = GetPayload();


            var result = handler.HandleAsync(message, "TrackingId");
            Assert.IsTrue(result.Result == MessageHandlerResult.Fatal);
        }

        [TestMethod]
        public void OnsqlExceptionWithDeadlockCode_MassTerminationHandler_Should_Retry()
        {
            var sqlException = new ExceptionHelper().WithErrorCode((int)SqlErrorCodes.Deadlock)
            .WithErrorMessage("Database exception occured...")
            .Build();

            this.MassTerminateService.Setup(x => x.ProcessPayload(It.IsAny<Commands.DataloadRequest>(),
                It.IsAny<IDataFactory>())).Throws(sqlException);

            Commands.DataloadRequest message = GetPayload();


            var result = handler.HandleAsync(message, "TrackingId");
            Assert.IsTrue(result.Result == MessageHandlerResult.Retry);
        }

        [TestMethod]
        public void OnsqlExceptionWithGeneralNetworkErrorCode_MassTerminationHandler_Should_Retry()
        {
 
            var sqlException = new ExceptionHelper().WithErrorCode((int)SqlErrorCodes.GeneralNetworkError)
            .WithErrorMessage("Database exception occured...")
            .Build();

            this.MassTerminateService.Setup(x => x.ProcessPayload(It.IsAny<Commands.DataloadRequest>(),
                It.IsAny<IDataFactory>())).Throws(sqlException);

            Commands.DataloadRequest message = GetPayload();


            var result = handler.HandleAsync(message, "TrackingId");
            Assert.IsTrue(result.Result == MessageHandlerResult.Retry);
        }

        [TestMethod]
        public void OnsqlExceptionWithTimeoutCode_MassTerminationHandler_Should_Retry()
        {


            var sqlException = new ExceptionHelper().WithErrorCode((int)SqlErrorCodes.Timeout)
            .WithErrorMessage("Database exception occured...")
            .Build();

            this.MassTerminateService.Setup(x => x.ProcessPayload(It.IsAny<Commands.DataloadRequest>(),
                It.IsAny<IDataFactory>())).Throws(sqlException);

            Commands.DataloadRequest message = GetPayload();


            var result = handler.HandleAsync(message, "TrackingId");
            Assert.IsTrue(result.Result == MessageHandlerResult.Retry);
        }

        private static Commands.DataloadRequest GetPayload()
        {
            var builder = new Commands.DataloadRequest.Builder();

            var message = builder
                        .SetBatchID(Guid.NewGuid().ToString())
                        .SetJobID(Guid.NewGuid().ToString())
                        .SetSource(Commands.SourceSystem.PS_LIVE)
                        .SetOrganisationId("426")
                        .SetRequestedBy("Projects")
                        .SetDataload(Commands.DataLoadTypes.MASS_TERMINATION)
                        .Build();
            return message;
        }
    }
}
