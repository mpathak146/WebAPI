Feature: PostingMassRehireWithInvalidSchemaShouldReturnError
	In order to ensure an error response from MassTerminationRequest when invalid Schema is supplied
	As an Informatica user/system
	I want to post an invalid schema request and check if I receive an error response

@mytag
Scenario Outline: Post To Mass Rehire With Invalid Schema
	Given I can access the Dataload API as an authenticated user
	And I have the client initialized
	And I setup POST <Endpoint> request with <GroupID> and <UserID>
	And I setup invalid schema to post Rehire
	When I Post Request
	Then I should get error as the response status

	Examples: 
	| GroupID | UserID			| Endpoint							|
	| "76"      | "Informatica" | "Dataload/Groups/76/MassRehire"	|