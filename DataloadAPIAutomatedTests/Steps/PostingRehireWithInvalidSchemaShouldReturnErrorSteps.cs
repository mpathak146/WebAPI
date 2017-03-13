using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace DataloadAPIAutomatedTests.Steps
{
    [Binding]
    public sealed class PostingRehireWithInvalidSchemaShouldReturnErrorSteps
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef


        [Given(@"I setup invalid schema to post Rehire")]
        public void GivenISetupInvalidSchemaToPostRehire()
        {
            var request = ScenarioContext.Current.Get<RestRequest>("Request");
            string json = "[  {\"WrongNumber\": \"ckl12\",\"RehireDate\": \"2015-10-10\"}]";
            request.AddParameter("application/json", json, ParameterType.RequestBody);

        }


    }
}
