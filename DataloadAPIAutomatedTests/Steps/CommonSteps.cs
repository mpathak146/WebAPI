using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using TechTalk.SpecFlow;

namespace DataloadAPIAutomatedTests.Steps
{
    [Binding]
    public sealed class CommonSteps
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef

        [Given(@"I setup POST ""(.*)"" request with ""(.*)"" and ""(.*)""")]
        public void GivenISetupPOSTRequestWithAnd(string endpoint, int groupID, string userID)
        {

            var request = new RestRequest(endpoint, Method.POST);
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-Fourth-Org", groupID.ToString());
            request.AddHeader("X-Fourth-UserID", userID);
            request.AddHeader("Accept", "application/json");
            var client = ScenarioContext.Current.Get<RestClient>("Client");
            ScenarioContext.Current.Add("Request", request);
        }
        [Given(@"I have the client initialized")]
        public void GivenIHaveTheClientInitialized()
        {
            var client = ScenarioContext.Current.Get<RestClient>("Client");
            ScenarioContext.Current.Clear();
            ScenarioContext.Current.Add("Client", client);
        }

        [Given(@"I can access the Dataload API as an authenticated user")]
        public void GivenICanAccessTheDataloadAPIAsAnAuthenticatedUser()
        {
            var client = new RestClient(ConfigurationManager.AppSettings["RootUrl"]);
            ScenarioContext.Current.Add("Client", client);
        }


        [When(@"I Post Mass Termination Request to")]
        public void WhenIPostMassTerminationRequestTo()
        {
            var client = ScenarioContext.Current.Get<RestClient>("Client");
            IRestResponse response =
                client.Execute(ScenarioContext.Current.Get<RestRequest>("Request"));
            ScenarioContext.Current.Add("Response", response);
        }

        [Then(@"I should get the response status is ok")]
        public void ThenIShouldGetTheResponseStatusIsOk()
        {
            Assert.AreEqual(HttpStatusCode.OK, ScenarioContext.Current.Get<RestResponse>("Response").StatusCode);
        }

        [Then(@"I should get error as the response status")]
        public void ThenIShouldGetErrorAsTheResponseStatus()
        {
            Assert.AreEqual(HttpStatusCode.InternalServerError, ScenarioContext.Current.Get<RestResponse>("Response").StatusCode);
        }


    }
}
