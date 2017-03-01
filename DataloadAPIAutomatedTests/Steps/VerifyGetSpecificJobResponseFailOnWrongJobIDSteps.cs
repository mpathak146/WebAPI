using System;
using TechTalk.SpecFlow;
using RestSharp;
using System.Configuration;
using NUnit.Framework;
using System.Net;


namespace DataloadAPIAutomatedTests
{
    [Binding]
    public partial class VerifyGetSpecificJobResponseFailOnWrongJobIDSteps
    {
        [Then(@"I Should get a failure for wrong JobID")]
        public void ThenIShouldGetAFailureForWrongJobID()
        {
            Assert.AreEqual(ScenarioContext.Current.Get<RestResponse>("Response").StatusCode, 
                HttpStatusCode.BadRequest);

        }


    }
}
