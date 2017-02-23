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
        [Given(@"I can access the Dataload API as an authenticated user")]
        public void GivenICanAccessTheDataloadAPIAsAnAuthenticatedUser()
        {
            var client = new RestClient(ConfigurationManager.AppSettings["RootUrl"]);
            ScenarioContext.Current.Add("Client", client);


        }
        
        [Then(@"I should get the response status is ok")]
        public void ThenIShouldGetTheResponseStatusIsOk()
        {
            
            Assert.AreEqual(HttpStatusCode.OK,ScenarioContext.Current.Get<RestResponse>("Response").StatusCode);
        }


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
