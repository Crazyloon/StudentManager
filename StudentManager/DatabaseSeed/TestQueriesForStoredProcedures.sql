USE BAS_Students
GO

-- list the courses this student has completed toward their 1st year - application dev certificate.
SELECT c.CourseName, ce.*, c.Credits
FROM CourseEnrollment ce
INNER JOIN Courses c ON ce.CourseID = c.CourseID
WHERE (CreditSection = 1 OR CreditSection = 2 OR CreditSection = 3) AND EnrollmentStatus = 3 AND StudentID = 7


-- Get total completed credits for Year 1 Course Related Classes for a given student (non gen ed)
DECLARE @StudentID int = 1
SELECT SUM(Credits)
FROM Courses c
INNER JOIN CourseEnrollment ce ON c.CourseID = ce.CourseID
WHERE c.CreditSection = 1 AND ce.EnrollmentStatus = 3 AND ce.StudentID = @StudentID

-- Number of credits to complete CompSci Course work
SELECT SUM(Credits)
FROM Courses c
WHERE c.CreditSection = 1
-- Number of credits to complete CompSci classes and GenEds, excluding choice gen eds.
SELECT SUM(Credits)
FROM Courses c
WHERE (c.CreditSection = 1 OR c.CreditSection = 2)
-- Number of credits to complete AAS Degree year 1 (Application Developer Cert) 
DECLARE @choiceGroupCredits int = 5 -- Require 5 credits of english, either COMP 100 or ENG& 101
SELECT SUM(Credits) + @choiceGroupCredits
FROM Courses c
WHERE (c.CreditSection = 1 OR c.CreditSection = 2 OR c.CreditSection = 3) 

-- Get total completed credits for Year 1 electives


-- Get total completed credits for Year 2 Required Classes
SELECT SUM(Credits)
FROM Courses
INNER JOIN CourseEnrollment ON Courses.CourseID = CourseEnrollment.CourseID
WHERE CourseEnrollment.StudentID = 1 AND Courses.CreditSection = 2 AND EnrollmentStatus = 3

-- get students who have not completed a given course
DECLARE @CourseID int = 13 -- any course id can be used
SELECT * FROM Students
WHERE
Students.StudentID NOT IN(SELECT StudentID FROM CourseEnrollment WHERE CourseID = @CourseID AND EnrollmentStatus = 3)

SELECT * FROM Courses
SELECT * FROM CourseEnrollment
SELECT * FROM CreditSectionLookUp

-- GET All students who have completed the required classes for App Dev Courses
SELECT * FROM Students
WHERE Students.StudentID IN 
(SELECT ce.StudentID FROM CourseEnrollment ce
INNER JOIN Courses c ON ce.CourseID = c.CourseID
INNER JOIN Students ON c.CourseID = Students.StudentID
WHERE EnrollmentStatus = 3 AND (SELECT SUM(Credits) FROM Courses c2 WHERE c2.CreditSection = 1 ) >= 72)


SELECT * FROM Students
WHERE Students.StudentID IN 
(SELECT Students.StudentID FROM CourseEnrollment ce
INNER JOIN Courses c ON ce.CourseID = c.CourseID
INNER JOIN Students ON c.CourseID = Students.StudentID
WHERE EnrollmentStatus = 3 AND c.CreditSection = 1 AND (SELECT SUM(Credits)
FROM Courses c
WHERE c.CreditSection = 1) >= 72)

 

-- list studentIDs with 72+ credits in Credit section 1
SELECT StudentID FROM CourseEnrollment
INNER JOIN Courses ON CourseEnrollment.CourseID = Courses.CourseID
WHERE EnrollmentStatus = 3 AND (SELECT SUM(Credits) FROM Courses WHERE CreditSection = 1) 

SELECT * FROM CourseEnrollment CE
INNER JOIN Courses C ON CE.CourseID = C.CourseID
INNER JOIN Students S ON C.CourseID = S.StudentID
 WHERE EnrollmentStatus = 3

 SELECT * FROM Students S
 INNER JOIN CourseEnrollment CE ON S.StudentID = CE.StudentID
 INNER JOIN Courses C ON CE.CourseID = C.CourseID
 WHERE EnrollmentStatus = 3
 GROUP BY S.StudentID

 SELECT s.StudentID, FirstName, CourseID, EnrollmentID FROM Students s
 LEFT JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID

 -- Number of completed AA Yr1 courses each student has completed --
 SELECT StudentID, COUNT(CourseID) AS 'Number of completed courses'
 FROM CourseEnrollment 
 WHERE EnrollmentStatus = 3 AND CourseID BETWEEN 1 AND 13
 GROUP BY StudentID
 -------------------------------------------------------------------

SELECT * FROM Students 
WHERE (SELECT * FROM Courses c RIGHT JOIN CourseEnrollment ce ON c.CourseID = ce.CourseID WHERE EnrollmentStatus = 3 and c.CourseID BETWEEN 1 AND 13) >= 13




SELECT * FROM COURSES

SELECT * FROM CreditSectionLookUp


