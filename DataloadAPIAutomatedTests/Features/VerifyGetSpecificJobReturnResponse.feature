Feature: VerifyGetSpecificJobReturnResponse
	In order to get a valid response
	As a user
	I want to request jobs api end point

@mytag
Scenario Outline: Get Processed Jobs from Jobs end point  
	Given I can access the Dataload API as an authenticated user 
	When I send a get request for the specific end point jobs "Dataload/Groups/<GroupID>/jobs/<JobID>"
	Then I should get the response status is ok 
	And I Should get a customerID "<CustomerID>"

	
	Examples: 
	| GroupID | JobID                                | CustomerID |
	| 76      | 9bc05034-95de-4e42-9c43-c19d1486d643 | WT324      |
	
	
