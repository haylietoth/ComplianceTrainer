# ComplianceTrainer

This project is made to be able to track the trainings of a company's employees without having to email HR.
The main purposes of this project will be for a company to be able to:
  1. Have its managers be able to view their employee training progresses.
  2. Have HR create and manage trainings.
  3. Have employees be able to view trainings, download training files, and take quizzes.

The solution currently has 2 versions of code in multiple places. In places that are in between comments that say //sean uncomment start
and sean uncomment end are those which use the ADSearcher code. Places that are in between the comments //group uncomment start and
//group uncomment end are places that does not use ADSearcher and instead strictly uses users in the database that were created from 
active directory. 

*** Update to this above
	There is a validate functino in the validation helper class which is only used by local (non AD) testing
	There is a block of code which looks like the above explanation, in the training controller. This is only 1 method but does need 		to be uncommented in order for Active Driectry code to work properly.
	In the Home controller there are several items:
		The Login method is commented out and replaced with a simple method instead of actual validation
		There are 2 peices of code which have comments like the above description as well. These are not full methods
			but instead peices of the Index method which calls different functions depanding if you are connected
			to active directory or not. 

Road Map:

The bulk of our code lays in the 'Views' and 'Scripts' folders. The 'Controllers' and 'helpers' folders also hold some significant code. 

Views: 

	In this folder you will find all the html files that make up the webpages. The 'Home' folder has the index.cshtml, which is the home page. The 'Shared' folder has the layout.cshtml which is are overall layout for the application used with all pages and the 'Error.cshtml' file which aids in error handling. The 'Training' folder has all the other webpages. Below is the structure of the pages.

	index.cshtml
	myTraining.cshtml
		-Quiz.cshtml (not live)
	manageEmployees.cshtml
		-EmployeeQuizes.cshtml
			-gradeQuiz.cshtml (not live)
	EditTraining.cshtml
		-updateTraining.cshtml (not live)
		-addTraining.cshtml

Scripts:

	In this folder you will find all the javascript files needed to complete functionality. Pay attention the any that relate to names in the Views folder. 

AKA:

	-addTraining.js --> addTraining.cshtml

	-EditTraining.js --> EditTraining.cshtml

	-employeesQuizes --> EmployeeQuizes.cshtml

	-gradingQuiz -->	gradeQuiz.html

	-manageEmployees.js --> manageEmployees.cshtml

	-myTraining.js --> myTraining.cshtml

	-Quiz.js --> Quiz.cshtml

	-updateTraining.js --> updateTraining.cshtml

Controllers:

	This folder contains the controllers we use to create operations that can be performed on the data. 
	One is the 'HomeController.cs' which is used on the home page and sets up the application based on 
	the access given to the user (so whether its an HR person, manager, or user). The other is 'TrainingController.cs'
	which is used on all the other webpages. There is documentation within the code that can explain to you what is 
	going on in more detail.

helpers:

	This folder contains code relating to user access and Acitve Directory. Pay attention to 'Queries.cs'. This file grabs all the data we use in the controllers, which is then used in the views. 











