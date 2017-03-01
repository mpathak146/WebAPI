Feature: VerifyGetSpecificJobResponseFailOnWrongJobID
	In order to get a valid response
	As a user
	I want to request jobs api end point

@mytag
Scenario Outline: Get Processed Jobs from Jobs end point  
	Given I can access the Dataload API as an authenticated user 
	When I send a get request for the specific end point jobs "Dataload/Groups/<GroupID>/jobs/<JobID>"
	Then I Should get a failure for wrong JobID 
	

	
	Examples: 
	| GroupID | JobID        |
	| 76      | InvalidJobID |
	
	
