Feature: PostingRequestWithInvalidDateShouldStillOkTheRequest
	In order to ensure a response from MassTerminationRequest when invalid data is supplied
	As an Informatica user/system
	I want to post an invalid date request and check if I receive a response

@mytag
Scenario Outline: Post To Mass Terminate With InvalidDate
	Given I can access the Dataload API as an authenticated user
	And I have the headers setup 
	And I setup POST <Endpoint> request with <GroupID> and <UserID>
	And I setup invalid data to post
	When I Post Mass Termination Request to 
	Then I should get the response status is ok 

	Examples: 
	| GroupID | UserID			| Endpoint								|
	| "76"      | "Informatica" | "Dataload/Groups/76/MassTerminate"	|