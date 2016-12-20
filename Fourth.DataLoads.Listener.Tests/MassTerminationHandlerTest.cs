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
        public MassTerminationHandlerTest()
        {
            this.MassTerminateService.Setup(x => x.ProcessPayload(It.IsAny<Commands.CreateAccount>(), It.IsAny<IDataFactory>())).ReturnsAsync(true).Verifiable();

        }
        [TestMethod]
        public async Task OnException_MassTerminationHandler_ShouldReturn_Fatal()
        {
            this.MassTerminateService.Setup(x => x.ProcessPayload(It.IsAny<Commands.CreateAccount>(), It.IsAny<IDataFactory>())).Throws(new Exception());

            Commands.CreateAccount message = GetPayload();

            var handler = new MassTerminationHandler(this.MassTerminateService.Object, It.IsAny<IDataFactory>());

            var result = await handler.HandleAsync(message, "TrackingId");
            Assert.IsTrue(result == MessageHandlerResult.Fatal);
        }
        [TestMethod]

        public async Task OnVerifiablePayload_MassTerminationHandler_Should_Process()
        {
            Commands.CreateAccount message = GetPayload();

            var handler = new MassTerminationHandler(this.MassTerminateService.Object, It.IsAny<IDataFactory>());

            var result = await handler.HandleAsync(message, "TrackingId");

            this.MassTerminateService.Verify(m => m.ProcessPayload(It.IsAny<Commands.CreateAccount>(), It.IsAny<IDataFactory>()));
        }
        private static Commands.CreateAccount GetPayload()
        {
            var builder = new Commands.CreateAccount.Builder();

            var message = builder
                        .SetEmailAddress("abc@fourth.com")
                        .SetFirstName(Guid.NewGuid().ToString())
                        .SetLastName(Guid.NewGuid().ToString())
                        .SetInternalId("1234")
                        .SetSource(Commands.SourceSystem.PS_LIVE)
                        .SetCustomerId("426")
                        .Build();
            return message;
        }
    }
}
