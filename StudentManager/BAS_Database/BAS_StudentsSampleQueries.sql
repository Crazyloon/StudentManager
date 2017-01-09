-- SAMPLE QUERIES
USE BAS_Students
GO
-- Stored procedure to delete a course, given a course ID
CREATE PROCEDURE DeleteCourseWithID
@courseID int = null
AS
DELETE FROM Courses WHERE CourseID = @courseID
GO

--EXECUTE DeleteCourseWithID @courseID = 14
--GO

--Which students still need to complete (specific class)?
USE BAS_Students
GO
CREATE PROCEDURE StudentsWhoNeedToCompleteClass
@classNumber VARCHAR(10) = null
AS
SELECT Students.StudentID, RTCStudentID, FirstName, LastName, Phone, StudentEmail, PersonalEmail, Address, City, State, Zip, BirthDate, Gender, Students.Notes, StudentDocumentsLocation
FROM Students
INNER JOIN CourseEnrollment ON Students.StudentID = CourseEnrollment.StudentID
INNER JOIN Courses ON CourseEnrollment.CourseID = Courses.CourseID
WHERE EnrollmentStatus <> 3 AND CourseNumber = @classNumber
GROUP BY Students.StudentID, RTCStudentID, FirstName, LastName, Phone, StudentEmail, PersonalEmail, Address, City, State, Zip, BirthDate, Gender, Students.Notes, StudentDocumentsLocation

GO

--EXECUTE StudentsWhoNeedToCompleteClass @classNumber = 'CSI 182'
--GO

--Which students have completed a (specific class)?
USE BAS_Students
GO
CREATE PROCEDURE StudentsWhoHaveCompletedClass
@classNumber VARCHAR(10) = null
AS
SELECT Students.StudentID, RTCStudentID, FirstName, LastName, Phone, StudentEmail, PersonalEmail, Address, City, State, Zip, BirthDate, Gender, Students.Notes, StudentDocumentsLocation
FROM Students
INNER JOIN CourseEnrollment ON Students.StudentID = CourseEnrollment.StudentID
INNER JOIN Courses ON CourseEnrollment.CourseID = Courses.CourseID
WHERE EnrollmentStatus = 3 AND CourseNumber = @classNumber
GROUP BY Students.StudentID, RTCStudentID, FirstName, LastName, Phone, StudentEmail, PersonalEmail, Address, City, State, Zip, BirthDate, Gender, Students.Notes, StudentDocumentsLocation

GO

--EXECUTE StudentsWhoHaveCompletedClass @classNumber = 'CSI 101'
--GO

--- STORED PROCEDURE
USE BAS_Students
GO
CREATE PROCEDURE StudentsWithPreReqsLeft
AS 
DECLARE @requiredCoursesCount INTEGER
SET @requiredCoursesCount = (SELECT COUNT(Courses.CourseID) FROM Courses WHERE RequiredForBASAdmission = 1)
SELECT Students.StudentID, RTCStudentID, FirstName, LastName, Phone, StudentEmail, PersonalEmail, Address, City, State, Zip, BirthDate, Gender, Students.Notes, StudentDocumentsLocation
FROM Students
INNER JOIN CourseEnrollment ON Students.StudentID = CourseEnrollment.StudentID
INNER JOIN Courses ON CourseEnrollment.CourseID = Courses.CourseID
WHERE EnrollmentStatus = 3 AND Courses.RequiredForBASAdmission = 1
GROUP BY Students.StudentID, RTCStudentID, FirstName, LastName, Phone, StudentEmail, PersonalEmail, Address, City, State, Zip, BirthDate, Gender, Students.Notes, StudentDocumentsLocation
HAVING Count(CourseEnrollment.CourseID) < @requiredCoursesCount
GO

--EXECUTE StudentsWithPreReqsLeft
--GO


-- GET STUDENT BY ID NUMBER
CREATE PROCEDURE GetStudentByID @studentID INTEGER = NULL
AS
SELECT FirstName, LastName, BirthDate, Gender, Address, City, State, Zip, PersonalEmail, StudentEmail, RTCStudentID, Phone, Notes, StudentDocumentsLocation
FROM Students
WHERE StudentID = @studentID
GO

--EXECUTE GetStudentByID @studentID = 1
--GO

-- Get Student Enrollment Data by ID Number

CREATE PROCEDURE GetStudentEnrollmentData
@StudentID int = null
AS
SELECT c.CourseNumber, c.CourseName, c.CourseItemNumber, esl.EnrollmentStatus, EnrollmentDate, ConditionalAdmission, ce.TransferEquivalent, ce.Notes
FROM Courses c
INNER JOIN CourseEnrollment ce ON ce.CourseID = c.CourseID
INNER JOIN EnrollmentStatusLookup esl ON ce.EnrollmentStatus = esl.EnrollmentStatusID
INNER JOIN Students s ON ce.StudentID = s.StudentID
WHERE ce.StudentID = @StudentID
GO

--EXECUTE GetStudentEnrollmentData @StudentID = 1
--GO

-- Get Student Details
CREATE PROCEDURE GetLastUpdatedStudentDetails
@studentID int = null
AS
SELECT *
FROM StudentDetails
WHERE StudentID = @studentID AND DetailID = (SELECT Max(DetailID) FROM StudentDetails WHERE StudentID = @studentID)
GO

--EXECUTE GetLastUpdatedStudentDetails @studentID = 4


-- Return the total number of students:

SELECT Count(StudentID) FROM Students

SELECT * FROM STUDENTS

--Which students graduated (which year)? – may need to lookup multiple years
--Which students attended an info session? - may need to lookup multiple times of the year
--Which students applied, and were accepted? How many? Which cohort year?
--Which students applied, and were given conditional admission? How many? Which cohort year?



--Which students applied, and were not accepted(A)? How many(B)? Which cohort year(C)?
--(A) - stuck trying to figure out how I can grab names who have a status name 'Applied' and NOT 'Enrolled'
-- If the combination of StudentID and Applied exists, but StudentID and Enrolled does not, display their name.
SELECT Students.StudentID, FirstName, StatusName, DATEPART(YYYY, DateChanged) AS 'Year' 
FROM Students
INNER JOIN BAS_StatusChanged ON Students.StudentID = BAS_StatusChanged.StudentID
INNER JOIN BAS_Status ON BAS_StatusChanged.StatusID = BAS_Status.StatusID
WHERE StatusName = 'Applied' OR StatusName = 'Enrolled'
GROUP BY Students.StudentID, StatusName, FirstName, DATEPART(YYYY, DateChanged)
--(B)
--(C)


SELECT CourseName FROM Courses WHERE CourseID <> 1

--Which students still need to complete prereqs?
DECLARE @requiredCoursesCount AS INTEGER
SET @requiredCoursesCount = (SELECT COUNT(Courses.CourseID) FROM Courses WHERE RequiredForBASAdmission = 1)
SELECT FirstName, LastName, StudentEmail, PersonalEmail, CourseEnrollment.StudentID, Count(CourseEnrollment.CourseID) AS 'Completed Courses'
FROM Students
INNER JOIN CourseEnrollment ON Students.StudentID = CourseEnrollment.StudentID
INNER JOIN Courses ON CourseEnrollment.CourseID = Courses.CourseID
WHERE EnrollmentStatus = 3 AND Courses.RequiredForBASAdmission = 1
GROUP BY FirstName, Lastname, StudentEmail, PersonalEmail, CourseEnrollment.StudentID
HAVING Count(CourseEnrollment.CourseID) < @requiredCoursesCount
GO

--Which students have completed all of the required classes for admission?
DECLARE @requiredCoursesCount AS INTEGER
SET @requiredCoursesCount = (SELECT COUNT(Courses.CourseID) FROM Courses WHERE RequiredForBASAdmission = 1)
SELECT FirstName, LastName, StudentEmail, PersonalEmail, CourseEnrollment.StudentID, Count(CourseEnrollment.CourseID) AS 'Completed Courses'
FROM Students
INNER JOIN CourseEnrollment ON Students.StudentID = CourseEnrollment.StudentID
INNER JOIN Courses ON CourseEnrollment.CourseID = Courses.CourseID
WHERE EnrollmentStatus = 3 AND Courses.RequiredForBASAdmission = 1
GROUP BY FirstName, Lastname, StudentEmail, PersonalEmail, CourseEnrollment.StudentID
HAVING Count(CourseEnrollment.CourseID) = @requiredCoursesCount
GO



SELECT * FROM BAS_StatusLookup
SELECT * FROM PreferredContactLookup
SELECT * FROM StudentDetails
SELECT * FROM Colleges
SELECT * FROM CourseEnrollment
