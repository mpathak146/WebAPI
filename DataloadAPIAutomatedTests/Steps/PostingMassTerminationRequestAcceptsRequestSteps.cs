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
        [Given(@"I setup valid (.*) data to post")]
        public void GivenISetupValidDataToPost(string date)
        {
            var request = ScenarioContext.Current.Get<RestRequest>("Request");
            string json = "[  {\"EmployeeNumber\": \"ckl12\",\"TerminationDate\": \""+ date + "\",\"TerminationReason\": \"Dismissed\"}]" ;
            request.AddParameter("application/json", json, ParameterType.RequestBody);
        }
    }
}
