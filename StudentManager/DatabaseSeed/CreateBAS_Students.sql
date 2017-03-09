USE master
GO

IF EXISTS (SELECT name FROM master.sys.databases WHERE name = N'BAS_Students')
DROP DATABASE BAS_Students
GO

CREATE DATABASE BAS_Students
ON
PRIMARY
(NAME = BAS_Students,
-- C:\Program Files\Microsoft SQL Server\MSSQL12.DOWSQLSERVER\MSSQL\DATA -- HOME
-- C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA -- School
-- C:\Program Files\Microsoft SQL Server\MSSQL12.DOWSQL\MSSQL\DATA -- Laptop
FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.DOWSQLSERVER\MSSQL\DATA\BAS_Students.mdf',
SIZE = 4,
MAXSIZE = 100,
FILEGROWTH = 5%)

LOG ON
(NAME = BAS_Students_log,
-- C:\Program Files\Microsoft SQL Server\MSSQL12.DOWSQLSERVER\MSSQL\DATA -- HOME
-- C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA -- School
-- C:\Program Files\Microsoft SQL Server\MSSQL12.DOWSQL\MSSQL\DATA -- Laptop
FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.DOWSQLSERVER\MSSQL\DATA\BAS_Students_log.ldf',
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
