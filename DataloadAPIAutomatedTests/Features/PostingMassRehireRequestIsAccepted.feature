Feature: PostingMassRehireRequestIsAccepted
	In order to ensure a response from MassRehireRequest
	As an Informatica user/system
	I want to post a request and check if I receive a response

@mytag
Scenario Outline: Post To Mass Rehire
	Given I can access the Dataload API as an authenticated user
	And I have the client initialized 
	And I setup POST <Endpoint> request with <GroupID> and <UserID>
	And I setup valid <MassRehire> data to post on Mass Rehire
	When I Post Request
	Then I should get the response status is ok 

	Examples: 
	| GroupID | UserID        | Endpoint							| MassRehire		|
	| "76"    | "Informatica" | "Dataload/Groups/76/MassRehire"		| "2015-10-10"		|            