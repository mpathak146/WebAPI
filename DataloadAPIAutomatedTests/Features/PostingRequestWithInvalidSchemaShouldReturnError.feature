Feature: PostingRequestWithInvalidSchemaShouldReturnError
	In order to ensure an error response from MassTerminationRequest when invalid Schema is supplied
	As an Informatica user/system
	I want to post an invalid schema request and check if I receive an error response

@mytag
Scenario Outline: Post To Mass Terminate With Invalid Schema
	Given I can access the Dataload API as an authenticated user
	And I have the headers setup 
	And I setup POST <Endpoint> request with <GroupID> and <UserID>
	And I setup invalid schema to post
	When I Post Mass Termination Request to 
	Then I should get error as the response status

	Examples: 
	| GroupID | UserID			| Endpoint								|
	| "76"      | "Informatica" | "Dataload/Groups/76/MassTerminate"	|