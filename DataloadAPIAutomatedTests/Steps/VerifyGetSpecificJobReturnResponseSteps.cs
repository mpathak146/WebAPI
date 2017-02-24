using System;
using TechTalk.SpecFlow;
using RestSharp;
using System.Configuration;
using NUnit.Framework;
using System.Net;


namespace DataloadAPIAutomatedTests
{
    [Binding]
    public partial class VerifyGetSpecificJobReturnResponseSteps
    {
        [When(@"I send a get request for the specific end point jobs ""(.*)""")]
        public void WhenISendAGetRequestForTheEndPointJobs(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.GET);
            var client = ScenarioContext.Current.Get<RestClient>("Client");
            var response = client.Execute(request);
            ScenarioContext.Current.Add("Response", response);
        }

        [Then(@"I Should get a customerID ""(.*)""")]
        public void ThenIShouldGetACustomerID(string p0)
        {
            Assert.True(ScenarioContext.Current.Get<RestResponse>("Response").Content.Contains(p0));
        }

    }
}
