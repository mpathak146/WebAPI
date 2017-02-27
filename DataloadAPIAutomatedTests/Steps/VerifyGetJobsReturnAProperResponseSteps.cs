using System;
using TechTalk.SpecFlow;
using RestSharp;
using System.Configuration;
using NUnit.Framework;
using System.Net;


namespace DataloadAPIAutomatedTests
{
    [Binding]
    public class VerifyGetJobsReturnAProperResponseSteps
    {
        [When(@"I send a get request for the end point jobs ""(.*)""")]
        public void WhenISendAGetRequestForTheEndPointJobs(string endpoint)
        {
            var request = new RestRequest(endpoint,Method.GET);
            var client = ScenarioContext.Current.Get<RestClient>("Client");
            var response = client.Execute(request);
            ScenarioContext.Current.Add("Response", response);
        }
    }
}
