using System;
using System.Threading;
using TechTalk.SpecFlow;
using Fourth.DataLoads.Data;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Data.SqlServer;
using NUnit.Framework;

namespace DataloadAPIAutomatedTests.Steps
{
    [Binding]
    public class PostingValidMassTerminationWorks_RollbackLaterSteps
    {
        [When(@"I pause for sometime")]
        public void WhenIPauseForSometime()
        {
            Thread.Sleep(10000);
        }
        
        [When(@"I verify the user is indeed terminated")]
        public void WhenIVerifyTheUserIsIndeedTerminated()
        {
            SqlDataFactory dataFactory = new SqlDataFactory();
            var portal=dataFactory.GetPortalVerificationRepository();
            Assert.AreEqual(false, portal.IsValidEmployee("1234"));
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should rollback my changes to previous state")]
        public void ThenIShouldRollbackMyChangesToPreviousState()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
