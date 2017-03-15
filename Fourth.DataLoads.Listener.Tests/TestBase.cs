using Fourth.DataLoads.Listener.Handlers;
using Fourth.Orchestration.Model.People;
using Moq;

namespace Fourth.DataLoads.Listener.Tests
{
    public abstract class TestBase
    {
        protected Mock<IMassTerminationService<Commands.DataloadRequest>> MassTerminateService
        { get; }
        protected Mock<IMassRehireService<Commands.DataloadRequest>> MassRehireService
        { get; }
        protected TestBase()
        {
            MassTerminateService = new Mock<IMassTerminationService<Commands.DataloadRequest>>();
            MassRehireService = new Mock<IMassRehireService<Commands.DataloadRequest>>();
        }
    }
}