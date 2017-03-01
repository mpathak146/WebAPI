using System;
using TechTalk.SpecFlow;
using RestSharp;
using System.Configuration;
using NUnit.Framework;
using System.Net;


namespace DataloadAPIAutomatedTests
{
    [Binding]
    public partial class VerifyGetSpecificJobSuccessOnValidButIncorrectGuidSteps
    {
        [Then(@"I should get an empty response json from API")]
        public void ThenIShouldGetAnEmptyResponseJsonFromAPI()
        {
            Assert.AreEqual(ScenarioContext.Current.Get<RestResponse>("Response").Content, "[]");

        }



    }
}
