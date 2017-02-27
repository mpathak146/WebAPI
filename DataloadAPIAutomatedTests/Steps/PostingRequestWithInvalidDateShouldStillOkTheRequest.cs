using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace DataloadAPIAutomatedTests.Steps
{
    [Binding]
    public sealed class PostingRequestWithInvalidDateShouldStillOkTheRequest
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef
        [Given(@"I setup invalid data to post")]
        public void GivenISetupInvalidDataToPost()
        {
            var request = ScenarioContext.Current.Get<RestRequest>("Request");
            string json = "[  {\"EmployeeNumber\": \"ckl12\",\"TerminationDate\": \"InvalidDate\",\"TerminationReason\": \"Dismissed\"}]";
            request.AddParameter("application/json", json, ParameterType.RequestBody);
        }

    }
}
