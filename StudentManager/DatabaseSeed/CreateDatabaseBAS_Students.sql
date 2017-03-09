USE master
GO

IF EXISTS (SELECT name FROM master.sys.databases WHERE name = N'BAS_Students')
DROP DATABASE BAS_Students
GO
--C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA
CREATE DATABASE BAS_Students
ON
PRIMARY
(NAME = BAS_Students,
--FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.DOWSQLSERVER\MSSQL\DATA\BAS_Students.mdf', -- home
FILENAME = 'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\BAS_Students.mdf', -- school
--FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.DOWSQL\MSSQL\DATA\BAS_Students.mdf', -- laptop
SIZE = 4,
MAXSIZE = 100,
FILEGROWTH = 5%)

LOG ON
(NAME = BAS_Students_log,
--FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.DOWSQLSERVER\MSSQL\DATA\BAS_Students_log.ldf',
FILENAME = 'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\BAS_Students_log.ldf',
--FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.DOWSQL\MSSQL\DATA\BAS_Students_log.ldf',
SIZE = 5,
MAXSIZE = 10,
FILEGROWTH = 5%)
GO

ALTER DATABASE BAS_Students SET RECOVERY FULL
GO


sp_helpdb BAS_Students

ALTER DATABASE BAS_Students
MODIFY FILE(name = 'BAS_Students_log',
MAXSIZE = 10)
