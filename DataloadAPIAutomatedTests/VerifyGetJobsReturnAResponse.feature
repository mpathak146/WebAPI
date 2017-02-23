Feature: VerifyGetJobsReturnAProperResponse
	In order to get a valid response
	As a user
	I want to request jobs api end point

@mytag
Scenario Outline: Get Processed Jobs from Jobs end point  
	Given I can access the Dataload API as an authenticated user 
	When I send a get request for the end point jobs "Dataload/Groups/<GroupID>/jobs"
	Then I should get the response status is ok 

	
	Examples: 
	| GroupID |
	| 76 |
