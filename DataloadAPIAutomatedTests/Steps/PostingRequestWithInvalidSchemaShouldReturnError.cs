using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace DataloadAPIAutomatedTests.Steps
{
    [Binding]
    public sealed class PostingRequestWithInvalidSchemaShouldReturnError
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef
        [Given(@"I setup invalid schema to post")]
        public void GivenISetupInvalidSchemaToPost()
        {
            var request = ScenarioContext.Current.Get<RestRequest>("Request");
            string json = "[  {\"WrongNumber\": \"ckl12\",\"TerminationDate\": \"2015-10-10\",\"TerminationReason\": \"Dismissed\"}]";
            request.AddParameter("application/json", json, ParameterType.RequestBody);
        }

    }
}
