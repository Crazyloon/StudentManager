USE BAS_Students
GO

CREATE TABLE Students
(
	StudentID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	RTCStudentID VARCHAR(10),
	FirstName VARCHAR(25) NOT NULL,
	LastName VARCHAR(25) NOT NULL,
	Phone VARCHAR(10), -- NOT NULL CHECK (Phone LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	StudentEmail VARCHAR(30),
	PersonalEmail VARCHAR(30) NOT NULL,
	Address VARCHAR(50),
	City VARCHAR(20),
	State VARCHAR(2),
	Zip VARCHAR(5), -- CHECK (Zip LIKE '[0-9][0-9][0-9][0-9][0-9]' OR Zip = NULL),
	BirthDate DATE,	
	Gender BIT NOT NULL,	
	Notes VARCHAR(1000),
	StudentDocumentsLocation VARCHAR(255)
)

CREATE TABLE Employers
(
	EmployerID INT IDENTITY(1,1) PRIMARY KEY,
	EmployerName VARCHAR(50)	
)

CREATE TABLE EmploymentStatus
(
	EmploymentStatusID INT IDENTITY(1,1) PRIMARY KEY,
	EmployerID INT NOT NULL,
	StudentID INT NOT NULL,
	Status INT NOT NULL, -- part time, full time
	JobTitleID INT NOT NULL,
	StatusChangedDate DATE NOT NULL

	--CONSTRAINT PK_EmploymentStatus PRIMARY KEY CLUSTERED(EmployerID, StudentID)
)

CREATE TABLE JobTitleLookUp
(
	JobTitleID INT IDENTITY(1,1) PRIMARY KEY,
	JobTitle VARCHAR(55) NOT NULL
)

INSERT INTO JobTitleLookUp VALUES('Web Administrator')
INSERT INTO JobTitleLookUp VALUES('Database Administrator')
INSERT INTO JobTitleLookUp VALUES('Sales Agent')
INSERT INTO JobTitleLookUp VALUES('Application Developer')
INSERT INTO JobTitleLookUp VALUES('Manager')

CREATE TABLE EmploymentStatusLookUp
(
	EmploymentStatusID INT NOT NULL PRIMARY KEY,
	EmploymentStatus VARCHAR(10)
)

INSERT INTO EmploymentStatusLookUp VALUES (1, 'Full Time')
INSERT INTO EmploymentStatusLookUp VALUES (2, 'Part Time')
INSERT INTO EmploymentStatusLookUp VALUES (3, 'Internship')


CREATE TABLE CourseEnrollment
(
	EnrollmentID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	StudentID INT NOT NULL, -- FK to Students (StudentID)
	CourseID INT NOT NULL,
	EnrollmentStatus INT NOT NULL, --fk to EnrollmentStatusLookup
	EnrollmentDate DATE NOT NULL,
	CompletionDate DATE NULL,
	ConditionalAdmission BIT NOT NULL,
	TransferEquivalent BIT NOT NULL,
	Notes VARCHAR(255)

	--CONSTRAINT PK_CourseEnrollment PRIMARY KEY CLUSTERED(StudentID, CourseID)
)

CREATE TABLE EnrollmentStatusLookUp
(
	EnrollmentStatusID INT NOT NULL PRIMARY KEY,
	EnrollmentStatus VARCHAR(12) NOT NULL, -- not enrolled, enrolled, completed, failed, dropped
)

INSERT INTO EnrollmentStatusLookup VALUES(1, 'Not Enrolled')
INSERT INTO EnrollmentStatusLookup VALUES(2, 'Enrolled')
INSERT INTO EnrollmentStatusLookup VALUES(3, 'Completed')
INSERT INTO EnrollmentStatusLookup VALUES(4, 'Failed')
INSERT INTO EnrollmentStatusLookup VALUES(5, 'Dropped')

CREATE TABLE Courses
(
	CourseID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CourseNumber VARCHAR(10) NOT NULL,
	CourseItemNumber VARCHAR(10) NOT NULL, -- What is a CourseItemNumber like? LIKE [A-Z][0-9]?
	CourseName VARCHAR(80) NOT NULL,
	CourseTypeID INT NOT NULL, -- BAS, AAS, GenEd
	CourseDescription VARCHAR(2000),
	Credits INT NOT NULL,
	RequiredForBASAdmission BIT NOT NULL,
	RequiredForBASCompletion BIT NOT NULL,
	CreditSection INT NOT NULL,
)

CREATE TABLE CreditSectionLookUp
(
	CreditSectionID INT NOT NULL PRIMARY KEY,
	CreditSection VARCHAR(30) NOT NULL
)

INSERT INTO CreditSectionLookUp VALUES(1, 'AAS YEAR 1')
INSERT INTO CreditSectionLookUp VALUES(2, 'AAS YEAR 2')
INSERT INTO CreditSectionLookUp VALUES(3, 'AAS REQUIRED GEN ED')
INSERT INTO CreditSectionLookUp VALUES(4, 'AAS OPTION GROUP')
INSERT INTO CreditSectionLookUp VALUES(5, 'AAST REQUIRED GEN ED')
INSERT INTO CreditSectionLookUp VALUES(6, 'AAST SOCIAL SCI OPTION GROUP')
INSERT INTO CreditSectionLookUp VALUES(7, 'AAST HUMANITIES OPTION GROUP')
INSERT INTO CreditSectionLookUp VALUES(8, 'BAS YEAR 1')
INSERT INTO CreditSectionLookUp VALUES(9, 'BAS YEAR 2')
INSERT INTO CreditSectionLookUp VALUES(10, 'BAS SUBSTITUTION')
INSERT INTO CreditSectionLookUp VALUES(11, 'BAS REQUIRED GEN ED')
INSERT INTO CreditSectionLookUp VALUES(12, 'OTHER')

CREATE TABLE CourseTypeLookUp
(
	CourseTypeID INT PRIMARY KEY NOT NULL,
	CourseType VARCHAR(5) NOT NULL
)

INSERT INTO CourseTypeLookUp VALUES(1, 'BAS')
INSERT INTO CourseTypeLookUp VALUES(2, 'AAS')
INSERT INTO CourseTypeLookUp VALUES(3, 'AAS-T')
INSERT INTO CourseTypeLookUp VALUES(4, 'GenEd')

CREATE TABLE StudentDetails
(
	DetailID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	StudentID INT NOT NULL, -- FK To students
	EduBackground INT NOT NULL,
	ReferralType INT NOT NULL,
	PreferredContactMethod INT NOT NULL, 
	AttendedInfoSession BIT NOT NULL,
	RunningStartParticipant BIT NOT NULL,
	BAS_Status INT NOT NULL, 
	LastUpdated DATE NOT NULL,
	Notes VARCHAR(500)
)

CREATE TABLE EduBackgroundLookup
(
	EduID INT NOT NULL PRIMARY KEY,
	EduBackground VARCHAR(16) NOT NULL -- middleschool, highschool, transfer student, college, returning adult
)

INSERT INTO EduBackgroundLookup VALUES(1, 'Middle School')
INSERT INTO EduBackgroundLookup VALUES(2, 'High School')
INSERT INTO EduBackgroundLookup VALUES(3, 'Transfer Student')
INSERT INTO EduBackgroundLookup VALUES(4, 'Some College')
INSERT INTO EduBackgroundLookup VALUES(5, 'Returning Adult')
INSERT INTO EduBackgroundLookup VALUES(6, 'Unknown')

CREATE TABLE ReferralTypeLookup
(
	ReferralID INT NOT NULL PRIMARY KEY,
	ReferralType VARCHAR(15) NOT NULL, --FK to ReferralTypes -- Friend/Family, Advisor, Instructor, RTC Website, flyer/brochure, internet search, open house, info session, other
)

INSERT INTO ReferralTypeLookup VALUES(1, 'Friend/Family')
INSERT INTO ReferralTypeLookup VALUES(2, 'Advisor')
INSERT INTO ReferralTypeLookup VALUES(3, 'Instructor')
INSERT INTO ReferralTypeLookup VALUES(4, 'RTC Website')
INSERT INTO ReferralTypeLookup VALUES(5, 'Flyer/Brochure')
INSERT INTO ReferralTypeLookup VALUES(6, 'Internet Search')
INSERT INTO ReferralTypeLookup VALUES(7, 'Open House')
INSERT INTO ReferralTypeLookup VALUES(8, 'Info Session')
INSERT INTO ReferralTypeLookup VALUES(9, 'Unknown')


CREATE TABLE PreferredContactLookup
(
	-- Phone, Email, Mail
	ContactTypeID INT NOT NULL PRIMARY KEY,
	ContactMethod VARCHAR(12) NOT NULL, -- Phone, Email, Mail, Text Message
)

INSERT INTO PreferredContactLookup VALUES(1, 'Phone')
INSERT INTO PreferredContactLookup VALUES(2, 'Email')
INSERT INTO PreferredContactLookup VALUES(3, 'Mail')
INSERT INTO PreferredContactLookup VALUES(4, 'Text Message')

CREATE TABLE BAS_StatusLookup
(
	StatusID INT NOT NULL PRIMARY KEY,
	StatusType VARCHAR(15) NOT NULL -- Prospective, Disinterested, Applied, Enrolled, Dropped, Graduated
)

INSERT INTO BAS_StatusLookup VALUES (1, 'Prospective')
INSERT INTO BAS_StatusLookup VALUES (2, 'Disinterested')
INSERT INTO BAS_StatusLookup VALUES (3, 'Applied')
INSERT INTO BAS_StatusLookup VALUES (4, 'Enrolled')
INSERT INTO BAS_StatusLookup VALUES (5, 'Dropped')
INSERT INTO BAS_StatusLookup VALUES (6, 'Graduated')

CREATE TABLE Colleges
(
	CollegeID INT IDENTITY(1,1) PRIMARY KEY,
	CollegeName VARCHAR(40)
)

CREATE TABLE AttendedColleges
(
	CollegeID INT NOT NULL,
	StudentID INT NOT NULL,
	CurrentlyAttending BIT NOT NULL,
	LastYearAttended INT NOT NULL CHECK (LastYearAttended LIKE '[0-9][0-9][0-9][0-9]'),
	GPA FLOAT NOT NULL CHECK(GPA BETWEEN 0 AND 4)

	CONSTRAINT PK_AttendedColleges PRIMARY KEY CLUSTERED(CollegeID, StudentID)
)

CREATE TABLE TechInterests
(
	--InterestID INT PRIMARY KEY,
	StudentID INT PRIMARY KEY,
	DatabaseDevelopment BIT,
	SoftwareDevelopment BIT,
	WebDevelopment BIT,
	MobileDevelopment BIT,
	DatabaseAnalysis BIT,
	SoftwareAnalysis BIT,
	InterfaceDesign BIT
)

-- Foreign Key Syntax --- Consider Cascasding
--ALTER TABLE TableName
--ADD 
----CONSTRAINT FKNAME FOREIGN KEY (Table_FieldName) REFERENCES OtherTableName(OtherTable_FieldName)
------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE StudentDetails
ADD
	CONSTRAINT FK_StudentDetails_PreferredContactLU FOREIGN KEY(PreferredContactMethod) REFERENCES PreferredContactLookup(ContactTypeID)

ALTER TABLE StudentDetails
ADD
	CONSTRAINT FK_StudentDetails_ReferralTypeLU FOREIGN KEY(ReferralType) REFERENCES ReferralTypeLookup(ReferralID)

ALTER TABLE StudentDetails
ADD
	CONSTRAINT FK_StudentDetails_EduBackgroundLU FOREIGN KEY(EduBackground) REFERENCES EduBackgroundLookup(EduID)

ALTER TABLE StudentDetails
ADD
	CONSTRAINT FK_StudentDetails_BAS_StatusLU FOREIGN KEY(BAS_Status) REFERENCES BAS_StatusLookup(StatusID)

ALTER TABLE StudentDetails
ADD
	CONSTRAINT FK_StudentDetails_Students FOREIGN KEY(StudentID) REFERENCES Students(StudentID)
------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE EmploymentStatus
ADD
	CONSTRAINT FK_EmploymentStatus_Students FOREIGN KEY(StudentID) REFERENCES Students(StudentID)

ALTER TABLE EmploymentStatus
ADD
	CONSTRAINT FK_EmploymentStatus_Employers FOREIGN KEY(EmployerID) REFERENCES Employers(EmployerID)

ALTER TABLE EmploymentStatus
ADD
	CONSTRAINT FK_EmploymentStatus_JobTitleLU FOREIGN KEY(JobTitleID) REFERENCES JobTitleLookUp(JobTitleID)
------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE CourseEnrollment
ADD
	CONSTRAINT FK_CourseEnrollment_Students FOREIGN KEY(StudentID) REFERENCES Students(StudentID)

ALTER TABLE CourseEnrollment
ADD
	CONSTRAINT FK_CourseEnrollment_Courses FOREIGN KEY(CourseID) REFERENCES Courses(CourseID)

ALTER TABLE CourseEnrollment
ADD
	CONSTRAINT FK_CourseEnrollment_EnrollmentStatusLU FOREIGN KEY(EnrollmentStatus) REFERENCES EnrollmentStatusLookup(EnrollmentStatusID)
------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE AttendedColleges
ADD
	CONSTRAINT FK_AttendedColleges_Students FOREIGN KEY(StudentID) REFERENCES Students(StudentID)

ALTER TABLE AttendedColleges
ADD
	CONSTRAINT FK_AttendedColleges_Colleges FOREIGN KEY(CollegeID) REFERENCES Colleges(CollegeID)
------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE TechInterests
ADD
	CONSTRAINT FK_TechInterests_Students FOREIGN KEY(StudentID) REFERENCES Students(StudentID)
------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE EmploymentStatus
ADD
	CONSTRAINT FK_EmploymentStatus_EmploymentStatusLU FOREIGN KEY(Status) REFERENCES EmploymentStatusLookUp(EmploymentStatusID) 
------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE Courses
ADD
	CONSTRAINT FK_Courses_CourseTypeLU FOREIGN KEY(CourseTypeID) REFERENCES CourseTypeLookUp(CourseTypeID)
	
ALTER TABLE Courses
ADD
	CONSTRAINT FK_Courses_CreditSectionLU FOREIGN KEY(CreditSection) REFERENCES CreditSectionLookUp(CreditSectionID) 
------------------------------------------------------------------------------------------------------------------------------------------------
-- OLD TABLES KEEP FOR REFERENCE

--CREATE TABLE BAS_Status
--(
--	StatusID INT IDENTITY(1,1) PRIMARY KEY,
--	BAS_Status VARCHAR(12) -- not applied, applied, enrolled, dropped, graduated
--)

--CREATE TABLE BAS_StatusChanged
--(
--	StatusID INT NOT NULL,
--	StudentID INT NOT NULL,
--	DateChanged DATETIME NOT NULL,
--	Notes VARCHAR(200),

--	CONSTRAINT PK_StatusChanged PRIMARY KEY CLUSTERED(StatusID, StudentID)
--)

--CREATE TABLE BAS_Applications
--(
--	ApplicationID INT NOT NULL PRIMARY KEY,
--	ApplicationStatus VARCHAR(12) NOT NULL, -- not applied, applied, admitted, denied
--	ApplicationStatusUpdate DATETIME NOT NULL, -- Auto update on Update or Insert
--	ApplicationStatusNotes VARCHAR(200),
--)

--CREATE TABLE StudentDetailChange
--(
--	DetailChangeID INT IDENTITY(1,1) PRIMARY KEY,
--	--DetailID INT NOT NULL,
--	StudentID INT NOT NULL,
--	DateChanged DATETIME NOT NULL

--	--CONSTRAINT PK_StudentDetailChange PRIMARY KEY CLUSTERED(DetailID, StudentID)
--)