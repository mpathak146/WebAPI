using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using TechTalk.SpecFlow;
using Json;

namespace DataloadAPIAutomatedTests.Steps
{
    [Binding]
    public class PostingMassRehireRequestIsAcceptedSteps
    {

        [Given(@"I setup valid ""(.*)"" data to post on Mass Rehire")]
        public void GivenISetupValidDataToPostOnMassRehire(string date)
        {
            var request = ScenarioContext.Current.Get<RestRequest>("Request");
            string json = "[  {\"EmployeeNumber\": \"ckl12\",\"RehireDate\": \"" + date + "\"}]";
            request.AddParameter("application/json", json, ParameterType.RequestBody);
        }


    }
}
