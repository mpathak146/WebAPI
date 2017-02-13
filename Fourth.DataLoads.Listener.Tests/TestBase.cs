using Fourth.DataLoads.Listener.Handlers;
using Fourth.Orchestration.Model.People;
using Moq;

namespace Fourth.DataLoads.Listener.Tests
{
    public abstract class TestBase
    {
        private Mock<IMassTerminationService<Commands.CreateAccount>> massterminationService;
        protected Mock<IMassTerminationService<Commands.CreateAccount>> MassTerminateService
        {
            get
            {
                return massterminationService;
            }
        }
                       
        protected TestBase()
        {
            massterminationService = new Mock<IMassTerminationService<Commands.CreateAccount>>();
        }
    }
}