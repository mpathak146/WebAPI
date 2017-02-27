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

        [Given(@"I can access the Dataload API as an authenticated user")]
        public void GivenICanAccessTheDataloadAPIAsAnAuthenticatedUser()
        {
            var client = new RestClient(ConfigurationManager.AppSettings["RootUrl"]);
            ScenarioContext.Current.Add("Client", client);
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
