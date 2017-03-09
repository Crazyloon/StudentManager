-- I want to see all the students who have an enrolment status of 3 for courses 1 thru 13
-- selects students who have completed App Developer program
-- SELECT Students who have completed gen eds for AAS

SELECT DISTINCT s.* FROM Students s
INNER JOIN CourseEnrollment ce ON ce.StudentID = s.StudentID
WHERE ce.StudentID IN (SELECT ce.StudentID FROM CourseEnrollment ce WHERE ce.CourseID = 25 AND ce.EnrollmentStatus = 3) AND
ce.StudentID IN (SELECT ce.StudentID FROM CourseEnrollment ce WHERE ce.CourseID = 26 AND ce.EnrollmentStatus = 3) AND
ce.StudentID IN (SELECT ce.StudentID FROM CourseEnrollment ce WHERE ce.CourseID = 29 AND ce.EnrollmentStatus = 3) AND
(ce.StudentID IN (SELECT ce.StudentID FROM CourseEnrollment ce WHERE ce.CourseID = 27 AND ce.EnrollmentStatus = 3) OR ce.StudentID IN (SELECT ce.StudentID FROM CourseEnrollment ce WHERE ce.CourseID = 28 AND ce.EnrollmentStatus = 3))


-- Select Students who have not completed courses for AAS Year 1
SELECT * FROM Students WHERE StudentID NOT IN (
      SELECT s.StudentID FROM Students s 
        JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
        JOIN Courses c ON ce.CourseID = c.CourseID
        WHERE ce.EnrollmentStatus = 3 AND c.CreditSection = 1
        GROUP BY s.StudentID
        HAVING COUNT(*) = 12
      )

SELECT * FROM Students WHERE StudentID NOT IN (
	SELECT s.StudentID FROM Students s 
	JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
	JOIN Courses c ON ce.CourseID = c.CourseID
	WHERE ce.EnrollmentStatus = 3 AND (c.CreditSection = 3 OR c.CreditSection = 4)
	GROUP BY s.StudentID
	HAVING COUNT(*) >= 4)

SELECT * FROM Students WHERE StudentID NOT IN (
	SELECT s.StudentID FROM Students s 
	JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
	JOIN Courses c ON ce.CourseID = c.CourseID
	WHERE ce.EnrollmentStatus = 3 AND c.CreditSection = 3
	GROUP BY s.StudentID
	HAVING COUNT(*) >= 3)
AND StudentID NOT IN (
	SELECT s.StudentID FROM Students s 
	JOIN CourseEnrollment ce ON s.StudentID = ce.StudentID
	JOIN Courses c ON ce.CourseID = c.CourseID
	WHERE ce.EnrollmentStatus = 3 AND c.CreditSection = 4
	GROUP BY s.StudentID
	HAVING COUNT(*) >= 1)