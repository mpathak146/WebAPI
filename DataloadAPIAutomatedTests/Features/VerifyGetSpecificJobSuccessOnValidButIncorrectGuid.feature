Feature: VerifyGetSpecificJobSuccessOnValidButIncorrectGuid
	In order to check a valid empty json response on supplied valid GUID
	As a user
	I want to request jobs api end point with valid JobID

@mytag
Scenario Outline: Get Processed Jobs from Jobs end point  
	Given I can access the Dataload API as an authenticated user 
	When I send a get request for the specific end point jobs "Dataload/Groups/<GroupID>/jobs/<JobID>"
	Then I should get an empty response json from API
	

	
	Examples: 
	| GroupID | JobID									|
	| 76      | 9bc05034-95de-4e42-9c43-c19d1486d644	|
	
	
