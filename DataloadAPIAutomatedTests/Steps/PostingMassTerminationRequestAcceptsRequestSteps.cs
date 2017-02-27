using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using TechTalk.SpecFlow;
using Json;

namespace DataloadAPIAutomatedTests.Steps
{
    [Binding]
    public class PostingMassTerminationRequestAcceptsRequestSteps
    {

        [Given(@"I have the headers setup")]
        public void GivenIHaveTheHeadersSetup()
        {
            var client = ScenarioContext.Current.Get<RestClient>("Client");
            ScenarioContext.Current.Clear();
            ScenarioContext.Current.Add("Client", client);
        }


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
        [Given(@"I setup valid data to post")]
        public void GivenISetupValidDataToPost()
        {
            var request = ScenarioContext.Current.Get<RestRequest>("Request");
            string json = "[  {\"EmployeeNumber\": \"ckl12\",\"TerminationDate\": \"2016-12-15\",\"TerminationReason\": \"Dismissed\"}]";
            request.AddParameter("application/json", json, ParameterType.RequestBody);
        }


        [When(@"I Post Mass Termination Request to")]
        public void WhenIPostMassTerminationRequestTo()
        {
            var client = ScenarioContext.Current.Get<RestClient>("Client");
            IRestResponse response = 
                client.Execute(ScenarioContext.Current.Get<RestRequest>("Request"));
            ScenarioContext.Current.Add("Response", response);
        }

    }
}
