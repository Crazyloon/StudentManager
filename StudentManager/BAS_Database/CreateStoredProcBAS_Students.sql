-- SAMPLE QUERIES
USE BAS_Students
GO
--Which students still need to complete (specific class)?
USE BAS_Students
GO
CREATE PROCEDURE StudentsWhoNeedToCompleteClass
@courseID int = null
AS
SELECT Students.StudentID, RTCStudentID, FirstName, LastName, Phone, StudentEmail, PersonalEmail, Address, City, State, Zip, BirthDate, Gender, Students.Notes, StudentDocumentsLocation
FROM Students
INNER JOIN CourseEnrollment ON Students.StudentID = CourseEnrollment.StudentID
INNER JOIN Courses ON CourseEnrollment.CourseID = Courses.CourseID
WHERE EnrollmentStatus <> 3 AND CourseNumber = @courseID
GROUP BY Students.StudentID, RTCStudentID, FirstName, LastName, Phone, StudentEmail, PersonalEmail, Address, City, State, Zip, BirthDate, Gender, Students.Notes, StudentDocumentsLocation

GO

--EXECUTE StudentsWhoNeedToCompleteClass @classNumber = 'CSI 182'
--GO

--Which students have completed a (specific class)?
USE BAS_Students
GO
CREATE PROCEDURE StudentsWhoHaveCompletedClass
@courseID int = null
AS
SELECT Students.StudentID, RTCStudentID, FirstName, LastName, Phone, StudentEmail, PersonalEmail, Address, City, State, Zip, BirthDate, Gender, Students.Notes, StudentDocumentsLocation
FROM Students
INNER JOIN CourseEnrollment ON Students.StudentID = CourseEnrollment.StudentID
INNER JOIN Courses ON CourseEnrollment.CourseID = Courses.CourseID
WHERE EnrollmentStatus = 3 AND CourseNumber = @courseID
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
SELECT ce.EnrollmentID, c.CourseNumber, ce.CompletionDate, c.CourseName, c.CourseItemNumber, esl.EnrollmentStatus, EnrollmentDate, ConditionalAdmission, ce.TransferEquivalent, ce.Notes
FROM Courses c
INNER JOIN CourseEnrollment ce ON ce.CourseID = c.CourseID
INNER JOIN EnrollmentStatusLookup esl ON ce.EnrollmentStatus = esl.EnrollmentStatusID
INNER JOIN Students s ON ce.StudentID = s.StudentID
WHERE ce.StudentID = @StudentID
ORDER BY c.CourseID
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
--GO

-- Delete College
CREATE PROCEDURE DeleteCollegeWithID
@CollegeID int = null
AS
DELETE FROM Colleges WHERE CollegeID = @CollegeID AND
NOT EXISTS(SELECT CollegeID FROM AttendedColleges WHERE CollegeID = @CollegeID)
GO

--SELECT CollegeID FROM AttendedColleges WHERE CollegeID = 6
--EXECUTE DeleteCollegeWithID @CollegeID = 6
--GO
--SELECT * FROM Colleges
--SELECT * FROM AttendedColleges
--GO

CREATE PROCEDURE DeleteEmployerWithID
@EmployerID int = null
AS
DELETE FROM Employers WHERE EmployerID = @EmployerID AND
NOT EXISTS(SELECT EmployerID FROM EmploymentStatus WHERE EmployerID = @EmployerID)
GO

--EXECUTE DeleteEmployerWithID @EmployerID = 10
--GO
--SELECT EmployerID FROM EmploymentStatus WHERE EmployerID = 10
--SELECT * FROM Employers
--SELECT * FROM EmploymentStatus
--GO

CREATE PROCEDURE DeleteJobTitleWithID
@JobTitleID int = null
AS
DELETE FROM JobTitleLookUp WHERE JobTitleID = @JobTitleID AND
NOT EXISTS(SELECT JobTitleID FROM EmploymentStatus WHERE JobTitleID = @JobTitleID)
GO

--EXECUTE DeleteJobTitleWithID @JobTitleID = 5
--GO
--SELECT JobTitleID FROM EmploymentStatus WHERE JobTitleID = 2
--SELECT * FROM JobTitleLookUp
--SELECT * FROM EmploymentStatus
--GO

CREATE PROCEDURE DeleteCourseWithID
@CourseID int = null
AS
DELETE FROM Courses WHERE CourseID = @CourseID AND
NOT EXISTS(SELECT CourseID FROM CourseEnrollment WHERE CourseID = @CourseID)
GO

--SELECT CourseID FROM CourseEnrollment WHERE CourseID = 1

CREATE PROCEDURE GetBASGraduates
@YearStart int = 2015,
@YearEnd int = 2015
AS
SELECT * FROM Students WHERE StudentID IN
(SELECT ce.StudentID FROM CourseEnrollment ce
WHERE ce.StudentID IN (
    SELECT s.StudentID FROM Students s 
    JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
    JOIN Courses c ON ce.CourseID = c.CourseID
    WHERE ce.EnrollmentStatus = 3 AND c.CreditSection = 1
    GROUP BY s.StudentID
    HAVING COUNT(*) >= 12
) AND
ce.StudentID IN(
    SELECT s.StudentID FROM Students s 
    JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
    JOIN Courses c ON ce.CourseID = c.CourseID
    WHERE ce.EnrollmentStatus = 3 AND c.CreditSection = 2
    GROUP BY s.StudentID
    HAVING COUNT(*) >= 10
) AND
ce.StudentID IN(
    SELECT s.StudentID FROM Students s 
    JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
    JOIN Courses c ON ce.CourseID = c.CourseID
    WHERE ce.EnrollmentStatus = 3 AND c.CreditSection = 3
    GROUP BY s.StudentID
    HAVING COUNT(*) >= 3
) AND
ce.StudentID IN(
    SELECT s.StudentID FROM Students s 
    JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
    JOIN Courses c ON ce.CourseID = c.CourseID
    WHERE ce.EnrollmentStatus = 3 AND c.CreditSection = 4
    GROUP BY s.StudentID
    HAVING COUNT(*) >= 1
) AND
ce.StudentID IN(
    SELECT s.StudentID FROM Students s 
    JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
    JOIN Courses c ON ce.CourseID = c.CourseID
    WHERE ce.EnrollmentStatus = 3 AND c.CreditSection = 5
    GROUP BY s.StudentID
    HAVING COUNT(*) >= 1
) AND
ce.StudentID IN(
    SELECT s.StudentID FROM Students s 
    JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
    JOIN Courses c ON ce.CourseID = c.CourseID
    WHERE ce.EnrollmentStatus = 3 AND c.CreditSection = 8
    GROUP BY s.StudentID
    HAVING COUNT(*) >= 6
) AND
ce.StudentID IN(
    SELECT s.StudentID FROM Students s 
    JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
    JOIN Courses c ON ce.CourseID = c.CourseID
    WHERE ce.EnrollmentStatus = 3 AND (c.CreditSection = 9 OR c.CreditSection = 10)
    GROUP BY s.StudentID
    HAVING COUNT(*) >= 6
) AND
ce.StudentID IN(
    SELECT s.StudentID FROM Students s 
    JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
    JOIN Courses c ON ce.CourseID = c.CourseID
    WHERE ce.EnrollmentStatus = 3 AND c.CreditSection = 11
    GROUP BY s.StudentID
    HAVING COUNT(*) >= 6
) AND
ce.StudentID IN(
    SELECT s.StudentID FROM Students s 
    JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
    JOIN Courses c ON ce.CourseID = c.CourseID
    WHERE ce.EnrollmentStatus = 3 AND c.CreditSection = 12
    GROUP BY s.StudentID
    HAVING COUNT(*) >= 1
)
GROUP BY ce.StudentID
HAVING MAX(DATEPART(yyyy, CompletionDate)) BETWEEN @YearStart AND @YearEnd)

 

SELECT * FROM CourseEnrollment WHERE StudentID = 8


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
