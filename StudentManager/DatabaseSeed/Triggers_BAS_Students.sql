-- TRIGGERS
USE BAS_Students
GO

CREATE TRIGGER GiveNewStudentDefaultEnrollment
ON Students
AFTER INSERT
AS
BEGIN
DECLARE @TEST INT = (SELECT StudentID FROM inserted)
DECLARE @StudentID INT = (SELECT StudentID FROM Students WHERE StudentID = (SELECT MAX(StudentID) FROM Students))
-- COL 101
INSERT INTO CourseEnrollment VALUES(@StudentID, 1, 1, GETDATE(), 0, 0, NULL)
-- Year 1
INSERT INTO CourseEnrollment VALUES(@StudentID, 2, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 3, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 4, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 5, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 6, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 7, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 8, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 9, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 10, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 11, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 12, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 13, 1, GETDATE(), 0, 0, NULL)
-- Year 2
INSERT INTO CourseEnrollment VALUES(@StudentID, 14, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 15, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 16, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 17, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 18, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 19, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 20, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 21, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 22, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 23, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 24, 1, GETDATE(), 0, 0, NULL)
-- Year 2 Required Gen Ed
INSERT INTO CourseEnrollment VALUES(@StudentID, 25, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 26, 1, GETDATE(), 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(@StudentID, 30, 1, GETDATE(), 0, 0, NULL)
-- 
END
GO
-- TEST:

INSERT INTO Students VALUES(NULL, 'Jason', 'Bourne', '7784498484', 'jbourne@student.rtc.edu', 'jbourne@hotmail.com', '21318 41st ST SE', 'Fairwood', 'WA', '98033', '1986-07-5', 0, 'Jason is a fictional character', 'C:\Users\Administrator\Desktop\TestLocation')

SELECT * FROM CourseEnrollment WHERE StudentID = 1--(SELECT MAX(StudentID) FROM Students)
SELECT * FROM Students
SELECT * FROM Courses
SELECT * FROM CreditSectionLookUp