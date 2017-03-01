using System;
using System.Threading;
using TechTalk.SpecFlow;
using Fourth.DataLoads.Data;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Data.SqlServer;
using NUnit.Framework;
using RestSharp;

namespace DataloadAPIAutomatedTests.Steps
{
    [Binding]
    public class PostingValidMassTerminationWorks_RollbackLaterSteps
    {
        SqlDataFactory dataFactory = new SqlDataFactory();
        [When(@"I pause for sometime")]
        public void WhenIPauseForSometime()
        {
            Thread.Sleep(100);
        }
        [Given(@"I setup employee with (.*) data to post")]
        public void GivenISetupEmployeeWithDataToPost(string data)
        {
            var request = ScenarioContext.Current.Get<RestRequest>("Request");
            string json = "[  {\"EmployeeNumber\": \"" + data + "\",\"TerminationDate\": \"2015-10-10\",\"TerminationReason\": \"Dismissed\"}]";
            request.AddParameter("application/json", json, ParameterType.RequestBody);
        }
        [When(@"I verify the user (.*) of Group ""(.*)"" is indeed terminated")]
        public void WhenIVerifyTheUserOfGroupIsIndeedTerminated(string employee, string group)
        {
            var portal = dataFactory.GetPortalVerificationRepository();
            ScenarioContext.Current.Add("IsValidEmployee", portal.IsValidEmployee(group, employee));
        }

        [Then(@"The employee must be verified terminated")]
        public void ThenTheEmployeeMustBeVerifiedTerminated()
        {
            Assert.AreEqual(
                ScenarioContext.Current.Get<bool>("IsValidEmployee"),
                false);
        }

        [Then(@"Rollback all changes")]
        public void ThenRollbackAllChanges()
        {

        }
        [Then(@"Rollback (.*) on ""(.*)"" to previous state")]
        public void ThenRollbackOnToPreviousState(string employeeNumber, string groupID)
        {
            var portal = dataFactory.GetPortalVerificationRepository();
            portal.RollbackEmployee(groupID, employeeNumber);
        }


    }
}
