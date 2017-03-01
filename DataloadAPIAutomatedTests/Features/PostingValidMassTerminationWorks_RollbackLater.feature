Feature: PostingValidMassTerminationWorks_RollbackLater
	In order to ensure MassTermination Works
	As an Informatica user/system
	I want to post a valid request and verify if it terminates	

@mytag
Scenario Outline: Post To Mass Terminate and rollback
	Given I can access the Dataload API as an authenticated user
	And I have the client initialized 
	And I setup POST <Endpoint> request with <GroupID> and <UserID>
	And I setup employee with <EmployeeNumber> data to post
	When I Post Mass Termination Request
	And I pause for sometime
	And I verify the user <EmployeeNumber> of Group <GroupID> is indeed terminated
	Then The employee must be verified terminated
	But Rollback <EmployeeNumber> on <GroupID> to previous state

	Examples: 
	| GroupID	| UserID			| Endpoint									| EmployeeNumber|
	| "76"      | "Informatica"		| "Dataload/Groups/76/MassTerminate"		| 575			|   