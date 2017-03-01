using System;
using TechTalk.SpecFlow;
using RestSharp;
using System.Configuration;
using NUnit.Framework;
using System.Net;


namespace DataloadAPIAutomatedTests
{
    [Binding]
    public partial class VerifyGetSpecificJobLooksForCorrectResponseSteps
    {
        [Then(@"I Should not get a customerID ""(.*)""")]
        public void ThenIShouldNotGetACustomerID(string customerID)
        {
            Assert.IsFalse(ScenarioContext.Current.Get<RestResponse>("Response").Content.Contains(customerID));
        }

    }
}
