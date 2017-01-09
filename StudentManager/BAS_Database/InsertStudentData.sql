USE BAS_Students
GO

INSERT INTO Students VALUES(809221891, 'Russell', 'Dow', '2063066808', 'rjdow@student.rtc.edu', 'russdow1@hotmail.com', '24245 183rd AVE SE', 'Covington', 'WA', '98042', '1988-08-15', 0, 'Russell is a student', 'C:\Users\Administrator\Desktop\TestLocation')
INSERT INTO Students VALUES(809221478, 'Borys', 'Karagmanov', '2063041892', 'bkkaragmanov@student.rtc.edu', 'bkk@gmail.com', '21845 42nd AVE S', 'Maple Valley', 'WA', '98044', '1986-04-21', 0, 'Borys is a student', NULL)
INSERT INTO Students VALUES(809221442, 'Yulia', 'Pryymak', '2063344818', 'yjpryymak@student.rtc.edu', 'yulday@gmail.com', '1148 32nd ST SE', 'Renton', 'WA', '98023', '1990-12-12', 1, 'Yulia is a student', NULL)
INSERT INTO Students VALUES(809221387, 'Jack', 'Talbert', '2064421184', 'jdtalbert@student.rtc.edu', 'jdtalbert@talbertcorp.com', '1700 Grant AVE S', 'Renton', 'WA', '98028', '1968-03-04', 0, 'Jack is a student', NULL)
INSERT INTO Students VALUES(null, 'Jet', 'Lee', '5555555555', null, 'jlee@gmail.com', '1213 Hollywood Blvd', 'Hollywood', 'CA', '90219', '1961-03-04', 0, 'Jet Lee is an actor, not a student.', NULL)
INSERT INTO Students VALUES(null, 'John', 'Doe', null, null, 'johnDoe@hotmail.com', null, null, null, null, null, 0, null, null)
INSERT INTO Students VALUES(950566188, 'Daniel', 'Corman', null, null, 'dannycorman@gmail.com', '2216 Harrington Pl NE', 'Renton', 'WA', '98056', null, 0, 'CSI 156, 351, 381', null)
INSERT INTO Students VALUES(809236034, 'Michael', 'Diep', null, null, 'mikediep@gmail.com', '19989 101st Ave SE', 'Renton', 'WA', '98055', null, 0, 'CSI 351, 381', null)
INSERT INTO Students VALUES(809113423, 'Sherefedin', 'Getahun', null, null, 'smgetahun@gmail.com', '3721 S 180th St, Apt #A220', 'Seatac', 'WA', '98188', null, 0, 'ENGL& 235, CSI 351, 381', null) 
INSERT INTO Students VALUES(809237857, 'Dimitrii', 'Karanin', null, null, 'dmitrii.karanin@live.com', '270 Bronson Way, Apt #33', 'Renton', 'WA', '98056', null, 0, 'NOT ATTENDING - Needs to take ENGL& 235, CSI 351, 381', null) 

SELECT * FROM Students

INSERT INTO StudentDetails VALUES(1, 4, 3, 2, 0, 0, 1, GETDATE(), '5-2-15 - Russell wants an info packet sent to his address')
INSERT INTO StudentDetails VALUES(2, 2, 1, 1, 0, 0, 1, GETDATE(), NULL)
INSERT INTO StudentDetails VALUES(3, 2, 2, 1, 0, 0, 1, GETDATE(), NULL)
INSERT INTO StudentDetails VALUES(4, 5, 4, 3, 0, 0, 1, GETDATE(), NULL)

SELECT * FROM StudentDetails

INSERT INTO TechInterests VALUES(1, 1, 1, 0, 1, 0, 1, 0)
INSERT INTO TechInterests VALUES(2, 0, 1, 1, 1, 0, 0, 1)
INSERT INTO TechInterests VALUES(3, 0, 1, 1, 1, 0, 0, 1)
INSERT INTO TechInterests VALUES(4, 1, 1, 1, 1, 1, 1, 1)

SELECT * FROM TechInterests

-- OTHER:
INSERT INTO Courses VALUES('COL 101', 'X113','College Success', 4 ,NULL, 3, 0, 0, 12)
-- 1st Year:
INSERT INTO Courses VALUES('CSI 101', 'L501','PC Hardware and Networking', 2,'This is an introductory course for students with little or no experience with computers. The students become familiar with the different hardware components  comprising  an  IBM compatible personal computer. They learn how to assemble a computer from the basic components and install and configure a Windows operating system. Students learn installation and administration of hardware and software to create a local area network using Microsoft WindowsServer software. Students learn cabling, network interface cards, workstation configuration and basic computer and networking troubleshooting. Students complete a hands-on network installation and administration project in which they create users, assign rights, create directory structures and implement user-level security. Emphasis is on troubleshooting and maintenance skills.', 6, 1, 1, 1)
INSERT INTO Courses VALUES('CSI 102', 'L503','Office Applications and Business Communications', 2, 'This is an introductory course for students having little or no experience with computers. Students learn the basic operation of the Microsoft Windows operating system. They also learn to use Microsoft Word, Excel, PowerPoint and Access. This class gives students the basic knowledge to use these applications in a  typical office environment, and to create printed documents, spreadsheets, presentations and a small database.', 6, 1, 1, 1)
INSERT INTO Courses VALUES('CSI 145', 'L505','Web Design', 2, 'This course introduces students to Hyper text Markup Language, or HTML. It covers basic html tags, links, lists, text formatting, images and multimedia, tables, and frames.  A quick introduction to cascading style sheets and javascript is covered to create dynamic and stylish web pages.', 7, 1, 1, 1)
INSERT INTO Courses VALUES('CSI 147', 'L507','Web Graphics', 2, 'This course is an introduction to digital photography and image manipulation and covers image capturing, editing, creating animation and producing web documentation. Students learn differences between "bmp", "gif", "jpg", "avi" and "mpg" file formats. The students create static image files as well as "flash" and "pdf " files for use on web pages. Topics include file resolution and download times with respect to web pages.', 7, 1, 1, 1)
INSERT INTO Courses VALUES('CSI 152', 'L509','Introduction to Programming', 2, NULL, 6, 1, 1, 1)
INSERT INTO Courses VALUES('CSI 154', 'L511','Introduction to C# Programming', 2, NULL, 7, 1, 1, 1)
INSERT INTO Courses VALUES('CSI 155', 'L513','Object-Oriented Programming with C#', 2, NULL, 7, 1, 1, 1)
INSERT INTO Courses VALUES('CSI 156', 'L515','Introduction to Database Theory and Design', 2, NULL, 7, 1, 1, 1)
INSERT INTO Courses VALUES('CSI 159', 'L517','Applied Database Development', 2, NULL, 7, 1, 1, 1)
INSERT INTO Courses VALUES('CSI 171', 'L519','Software Testing', 2, NULL, 4, 1, 1, 1)
INSERT INTO Courses VALUES('AMATH 174J', 'L523','Computer Mathematics', 2, NULL, 4, 1, 1, 1)
INSERT INTO Courses VALUES('CSI 182', 'L521','Leadership and Teamwork in Systems Analysis', 2, NULL, 4, 1, 1, 1)
-- 2nd Year:
-- INSERT INTO Courses VALUES('CourseNum', 'ITEM','TITLE', TYPE , 'DESCRIPTION', CREDITS, REQ.Admission, REQ.Completion)
INSERT INTO Courses VALUES('CSI 220', 'L601','IT Project Management and Team Building', 2 , NULL, 7, 1, 1, 2)
INSERT INTO Courses VALUES('CSI 245', 'L603','Java for C# Programmers', 2 , NULL, 7, 1, 1, 2)
INSERT INTO Courses VALUES('CSI 250', 'L605','Rich Internet Applications', 2 , NULL, 7, 1, 1, 2)
INSERT INTO Courses VALUES('CSI 253', 'L607','Client-Server Development with ADO.NET', 2 , NULL, 7, 1, 1, 2)
INSERT INTO Courses VALUES('CSI 256', 'L609','Advanced Programming Concepts with C#', 2 , NULL, 7, 1, 1, 2)
INSERT INTO Courses VALUES('CSI 258', 'L611','SQL Server Development and Administration', 2 , NULL, 7, 1, 1, 2)
INSERT INTO Courses VALUES('CSI 277', 'L613','IT Industry Reserach and Writing', 2 , NULL, 7, 1, 1, 2)
INSERT INTO Courses VALUES('CSI 281', 'L615','E-Commerce and Business Finance', 2 , NULL, 7, 1, 1, 2)
INSERT INTO Courses VALUES('CSI 291', 'L617','Developing Web Applications with ASP.NET', 2 , NULL, 7, 1, 1, 2)
INSERT INTO Courses VALUES('CSI 293', 'L619','Capstone Design and Development Project', 2 , NULL, 8, 1, 1, 2)
INSERT INTO Courses VALUES('CSI 294', 'L621','Cooperative Education/Internship (optional)', 2 , NULL, 17, 1, 1, 2)
-- Required Gen Eds
INSERT INTO Courses VALUES('AMATH 185', 'G','Applied Algebra for Business and Industry', 4 , NULL, 5, 1, 1, 3)
INSERT INTO Courses VALUES('CMST& 101', 'G','Introduction to Communication', 4 , NULL, 5, 1, 1, 3)
INSERT INTO Courses VALUES('COMP 100', 'G','Applied Composition', 4 , NULL, 5, 1, 1, 4)
INSERT INTO Courses VALUES('ENGL& 101', 'G','English Composition', 4 , NULL, 5, 1, 1, 4)
INSERT INTO Courses VALUES('PSYC& 100', 'G','General Psychology', 4 , NULL, 5, 1, 1, 3)
-- BAS Track:
INSERT INTO Courses VALUES('MATH& 141', 'G','Pre-Calculus', 4 , NULL, 5, 0, 1, 5)
-- choose 1 of 4:
INSERT INTO Courses VALUES('ECON& 202', 'G','Macro Economics', 4 , NULL, 5, 0, 1, 6)
INSERT INTO Courses VALUES('POLS 150', 'G','Contemporary World Issues', 4 , NULL, 5, 0, 1, 6)
INSERT INTO Courses VALUES('SOC& 101', 'G','Survey of Sociology', 4 , NULL, 5, 0, 1, 6)
INSERT INTO Courses VALUES('PSYC& 200', 'G','Developmental Psychology', 4 , NULL, 5, 0, 1, 6)
-- choose 1 of 3:
INSERT INTO Courses VALUES('PHIL& 101', 'G','Introduction to Philosophy', 4 , NULL, 5, 0, 1, 7)
INSERT INTO Courses VALUES('CMST& 220', 'G','Public Speaking', 4 , NULL, 5, 0, 1, 7)
INSERT INTO Courses VALUES('HIST 110', 'G','Survey of American History', 4 , NULL, 5, 0, 1, 7)
-- BAS Courses
INSERT INTO Courses VALUES('CSI 331', 'L801','Securing and Managing Data', 1, NULL, 5, 0, 1, 9)
INSERT INTO Courses VALUES('CSI 332', 'L803','Data Mining & Business Analytics', 1, NULL, 5, 0, 1, 8)
INSERT INTO Courses VALUES('CSI 341', 'L805','Web Development', 1, NULL, 5, 0, 1, 8)
INSERT INTO Courses VALUES('CSI 342', 'L807','Mobile Application Development', 1, NULL, 5, 0, 1, 8)
INSERT INTO Courses VALUES('CSI 351', 'L809','Systems Analysis and Design', 1, NULL, 5, 0, 1, 8)
INSERT INTO Courses VALUES('CSI 352', 'L811','Software Application Development', 1, NULL, 5, 1, 1, 8)
INSERT INTO Courses VALUES('CSI 381', 'L813','Principles of Human-Computer Interaction/GUI Design', 1, NULL, 5, 0, 1, 8)
INSERT INTO Courses VALUES('CSI 434', 'L815','Business Intelligence', 1, NULL, 5, 0, 1, 9)
INSERT INTO Courses VALUES('CSI 443', 'L817','Web Programming for Mobile Devices', 1, NULL, 5, 0, 1, 9)
INSERT INTO Courses VALUES('CSI 453', 'L819','Software Application Testing and Deployment', 1, NULL, 5, 0, 1, 9)
INSERT INTO Courses VALUES('CSI 483', 'L821','IT Project Management', 1, NULL, 5, 0, 1, 9)
INSERT INTO Courses VALUES('CSI 493', 'L823','Senior Capstone Project', 1, NULL, 5, 0, 1, 9)
INSERT INTO Courses VALUES('CSI 494', 'L825','Cooperative Education/Internship (optional)', 1, NULL, 5, 0, 1, 10)
-- BAS GEN EDS
INSERT INTO Courses VALUES('ECON& 201', 'G','Micro Economics', 4, NULL, 5, 0, 1, 11)
INSERT INTO Courses VALUES('ENGL& 235', 'G','Technical Writing', 4, NULL, 5, 0, 1, 11)
INSERT INTO Courses VALUES('MATH& 146', 'G','Introduction to Statistics', 4, NULL, 5, 0, 1, 11)
INSERT INTO Courses VALUES('PHIL 481', 'G','Legal and Ethical Aspects of IT', 4, NULL, 5, 0, 1, 11)
INSERT INTO Courses VALUES('PHYS& 114', 'G','Physics I', 4, NULL, 5, 0, 1, 11)
INSERT INTO Courses VALUES('POLS& 202', 'G','American Government', 4, NULL, 5, 0, 1, 11)



SELECT * FROM Courses

--Russell
-- YR 1
INSERT INTO CourseEnrollment VALUES(1, 1, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 2, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 3, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 4, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 5, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 6, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 7, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 8, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 9, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 10, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 11, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 12, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 13, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
-- GEN ED YR1
INSERT INTO CourseEnrollment VALUES(1, 25, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 26, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 29, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
-- OPTIONAL GEN END YR1
INSERT INTO CourseEnrollment VALUES(1, 28, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
-- YR 2
INSERT INTO CourseEnrollment VALUES(1, 14, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 15, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 16, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 17, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 18, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 19, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 20, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 21, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 22, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 23, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 24, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
-- YR 2 GEN ED
INSERT INTO CourseEnrollment VALUES(1, 30, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
-- BAS
INSERT INTO CourseEnrollment VALUES(1, 38, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 39, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 40, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 41, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 42, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 43, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 44, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 45, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 46, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 47, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 48, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 49, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
-- AAST GEN ED
INSERT INTO CourseEnrollment VALUES(1, 33, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 34, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 35, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
-- BAS GEN ED
INSERT INTO CourseEnrollment VALUES(1, 51, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 52, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 53, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 54, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 55, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(1, 56, 3, '2015-01-06', '2015-01-06', 0, 0, NULL)
SELECT * FROM Courses
-- Borys
INSERT INTO CourseEnrollment VALUES(2, 1, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 2, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 3, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 4, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 5, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 6, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 7, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 8, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 9, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 10, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 11, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 12, 3, '2013-01-06', '2014-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(2, 13, 3, '2014-01-06', '2014-01-06', 0, 0, NULL)
-- Yulia
INSERT INTO CourseEnrollment VALUES(3, 1, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 2, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 3, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 4, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 5, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 6, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 7, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 8, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 9, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 10, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 11, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 12, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(3, 13, 5, '2014-01-06', NULL, 0, 0, NULL)
-- Jack
INSERT INTO CourseEnrollment VALUES(4, 1, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 2, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 3, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 4, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 5, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 6, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 7, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 8, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 9, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 10, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 11, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 12, 3, '2014-01-06', '2015-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(4, 13, 5, '2014-01-06', NULL, 0, 0, NULL)

-- Michael Diep

INSERT INTO CourseEnrollment VALUES(8, 1, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 2, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 3, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 4, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 5, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 6, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 7, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 8, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 9, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 10, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 11, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 12, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 13, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
-- GEN ED YR1
INSERT INTO CourseEnrollment VALUES(8, 25, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 26, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 29, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
-- OPTIONAL GEN END YR1
INSERT INTO CourseEnrollment VALUES(8, 28, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
-- YR 2
INSERT INTO CourseEnrollment VALUES(8, 14, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 15, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 16, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 17, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 18, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 19, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 20, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 21, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 22, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 23, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 24, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)

-- AAST GEN ED
INSERT INTO CourseEnrollment VALUES(8, 30, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 33, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 34, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 35, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
-- BAS
INSERT INTO CourseEnrollment VALUES(8, 38, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 39, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 40, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 41, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 42, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 43, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 44, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 45, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 46, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 47, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 48, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 49, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)

-- BAS GEN ED
INSERT INTO CourseEnrollment VALUES(8, 51, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 52, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 53, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 54, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 55, 3, '2012-01-06', '2013-01-06', 0, 0, NULL)
INSERT INTO CourseEnrollment VALUES(8, 56, 4, '2012-01-06', NULL, 0, 0, NULL)

SELECT * FROM CourseEnrollment

INSERT INTO Colleges VALUES('Green River Community College')
INSERT INTO Colleges VALUES('Bates Technical College')
INSERT INTO Colleges VALUES('South Seattle Community College')
INSERT INTO Colleges VALUES('North Seattle Community College')
INSERT INTO Colleges VALUES('Highline Community College')
INSERT INTO Colleges VALUES('Eastern Washington University')
INSERT INTO Colleges VALUES('University of Washington')

SELECT * FROM Colleges

INSERT INTO Employers VALUES('Expeditors International of Washington')
INSERT INTO Employers VALUES('Washington Mutual Insurance Company')
INSERT INTO Employers VALUES('Geico')
INSERT INTO Employers VALUES('Wizards of the Coast')
INSERT INTO Employers VALUES('Hazbro')
INSERT INTO Employers VALUES('Renton Technical College')
INSERT INTO Employers VALUES('Brighter Day Boquet')
INSERT INTO Employers VALUES('Massage Envy & Spa')
INSERT INTO Employers VALUES('PAR Electrical Contractors Inc.')

SELECT * FROM Employers

INSERT INTO EmploymentStatus VALUES(7, 3, 1, 4, GETDATE())
INSERT INTO EmploymentStatus VALUES(8, 2, 1, 2, GETDATE())
INSERT INTO EmploymentStatus VALUES(9, 4, 2, 2, GETDATE())

SELECT * FROM EmploymentStatus

INSERT INTO AttendedColleges VALUES(1, 1, 0, 2012, 4)
INSERT INTO AttendedColleges VALUES(2, 2, 0, 2013, 3.4)
INSERT INTO AttendedColleges VALUES(3, 2, 0, 2010, 3.6)
INSERT INTO AttendedColleges VALUES(5, 3, 0, 2012, 2.8)
INSERT INTO AttendedColleges VALUES(1, 4, 0, 2010, 4)
INSERT INTO AttendedColleges VALUES(5, 1, 0, 2011, 3.8)

SELECT * FROM AttendedColleges





