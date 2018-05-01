use ComplianceTrainer;

drop table if exists UserQuizQuestionAnswers;
drop table if exists Answers;
drop table if exists Questions;
drop table if exists Quizes;
drop table if exists Groups;
drop table if exists Users;

create table Users(
	id int identity primary key,
	firstName varchar(50) not null,
	lastName varchar(50) not null,
	email varchar(100) not  null,
	userGroups varchar(max),
	SAMAccountName varchar(100) not null,
	manager varchar(50) not  null
);

create table Groups(
	id int identity primary key,
	name varchar(100) not null
);

create table Quizes(
	id int identity primary key,
	groupId int not null,
	title varchar(100) not null,
	description varchar(max),
	media varchar(max),
	foreign key (groupId) references Groups(id)
);

create table Questions(
	id int identity primary key,
	quizId int not null,
	questionText varchar(max) not null,
	questionType varchar(50) not null,
	foreign key (quizId) references Quizes(id)
);

create table Answers(
	id int identity primary key,
	questionId int not null,
	answerText varchar(500) null,
	isCorrect bit not null,
	foreign key (questionId) references Questions(id)
);

create table UserQuizQuestionAnswers(
	id int identity primary key,
	userId int not null,
	quizId int not null,
	questionId int not null,
	answerId int not null,
	text varchar(max) null,
	isChecked bit,
	isApproved bit,
	foreign key (userId) references Users(id),
	foreign key (quizId) references Quizes(id),
	foreign key (questionId) references Questions(id),
	foreign key (answerId) references Answers(id)
);

insert into Groups Values('HRGroup'),('managers'),('users'),('User');

insert into Users(firstName,lastName, email, userGroups,SAMAccountName, manager) 
values('Sean','Leroy','sleroy18@jcu.edu','Administrator,HRGroup','Administrator', 'Administrator'),
		('DoNotTestMe1F', 'DONotTestMe1L', 'DBTEST1@jcu.edu', 'managers,users','DoNotTest1','Administrator'),
		('DoNotTestMe2F', 'DoNotTestMe2L', 'DBTEST2@jcu.edu', 'users', 'DoNotTest2','DoNotTest1');

SET IDENTITY_INSERT Quizes ON;

INSERT INTO Quizes (id,groupId, title, description)
VALUES (1, 1, 'Test Quiz 1', 'This quiz is for testing');

SET IDENTITY_INSERT Quizes OFF;
SET IDENTITY_INSERT Questions ON;

INSERT INTO Questions (id, quizId, questionText, questionType)
VALUES (1, 1, 'What is 2+2?', 'multipleChoice');
INSERT INTO Questions (id, quizId, questionText, questionType)
VALUES (2, 1, '2+2 equals 4', 'trueFalse');
INSERT INTO Questions (id, quizId, questionText, questionType)
VALUES (3, 1, 'Why does 2 + 2 equal 4', 'shortAnswer');

SET IDENTITY_INSERT Questions OFF;

INSERT INTO Answers (questionId, answerText, isCorrect)
VALUES (1, '2', 1);
INSERT INTO Answers (questionId, answerText, isCorrect)
VALUES (1, '4', 0);
INSERT INTO Answers (questionId, answerText, isCorrect)
VALUES (1, '0', 1);
INSERT INTO Answers (questionId, answerText, isCorrect)
VALUES (1, '8', 1);

INSERT INTO Answers (questionId, answerText, isCorrect)
VALUES (2, 'True', 0);
INSERT INTO Answers (questionId, answerText, isCorrect)
VALUES (2, 'False', 1);

select * from Users;
select * from Quizes;
select * from Questions;
select * from Answers;
select * from UserQuizQuestionAnswers;