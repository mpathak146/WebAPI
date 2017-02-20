using Fourth.DataLoads.Listener.Handlers;
using Fourth.Orchestration.Model.People;
using Moq;

namespace Fourth.DataLoads.Listener.Tests
{
    public abstract class TestBase
    {
        private Mock<IMassTerminationService<Commands.DataloadRequest>> massterminationService;
        protected Mock<IMassTerminationService<Commands.DataloadRequest>> MassTerminateService
        {
            get
            {
                return massterminationService;
            }
        }
                       
        protected TestBase()
        {
            massterminationService = new Mock<IMassTerminationService<Commands.DataloadRequest>>();
        }
    }
}