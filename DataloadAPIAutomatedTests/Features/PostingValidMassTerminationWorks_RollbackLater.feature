﻿Feature: PostingValidMassTerminationWorks_RollbackLater
	In order to ensure MassTermination Works
	As an Informatica user/system
	I want to post a valid request and verify if it terminates	

@mytag
Scenario Outline: Post To Mass Terminate
	Given I can access the Dataload API as an authenticated user
	And I have the client initialized 
	And I setup POST <Endpoint> request with <GroupID> and <UserID>
	And I setup valid data to post
	When I Post Mass Termination Request to 
	Then I should get the response status is ok 

	Examples: 
	| GroupID | UserID			| Endpoint								|
	| "76"      | "Informatica" | "Dataload/Groups/76/MassTerminate"	|