using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Fourth.DataLoads.ApiEndPoint.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        [TestCategory("IntegrationTests")]
        public void GetDataByJobTitle_WithData_ReturnObject()
        {
            //ARRANGE

            string connString = ConfigurationManager.ConnectionStrings["TRGManagementContext"].ConnectionString;

            var contextFactory = new Data.SqlServer.PortalDBContextFactory(connString);

            var repository = new Data.SqlServer.DefaultHolidayAllowanceRepository(contextFactory);

            // ACT
            var response = repository.GetDataAsync(426, 259);

            // ASSERT
            //check that the

            Assert.IsNotNull(response);
        }

        [TestMethod]
        [TestCategory("IntegrationTests")]
        public void GetDataByJobTitle_WithNoData_ReturnObject()
        {
            //ARRANEG
            string connString = ConfigurationManager.ConnectionStrings["TRGManagementContext"].ConnectionString;
            var contextFactory = new Data.SqlServer.SqlDataFactory(connString);
            var repository = contextFactory.GetDefaultHolidayAllowanceRepository();//new Data.SqlServer.SqlDefaultHolidayAllowanceRepository(contextFactory);
            // ACT
            var response = repository.GetDataAsync(426, 1000, 11);
            // ASSERT
            //check that the
            Assert.IsNull(response);
        }
    }
}