USE master
GO

CREATE DATABASE Codesanook
GO

USE Codesanook
GO

-- Reference SQL Server index and constraint naming convention
-- https://www.codesanook.com/sql-server-index-and-constraint-naming-convention

CREATE TABLE Branch (
    Id INT NOT NULL IDENTITY(1, 1),
    Name NVARCHAR(256) NOT NULL
    CONSTRAINT PK_Branch_Id PRIMARY KEY CLUSTERED (Id ASC),
)

CREATE TABLE Office (
    Id INT NOT NULL IDENTITY(1, 1),
    Name NVARCHAR(256) NOT NULL,
    Code NVARCHAR(32) NOT NULL,
    Address NVARCHAR(MAX) NOT NULL,
    BranchId INT NULL,
    CreatedDateUtc DATETIME NOT NULL
    CONSTRAINT PK_Office_Id PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Office_BranchId FOREIGN KEY (BranchId) REFERENCES Branch(Id)
)

CREATE NONCLUSTERED INDEX IX_Office_BranchId ON Office(BranchId ASC)
GO

INSERT INTO Branch VALUES ('Bangkok')
DECLARE @newId INT = SCOPE_IDENTITY()

-- Insert a record
INSERT INTO Office VALUES ('Office A', '12345', 'Bangkok, Thailand', @newId, GETUTCDATE())
GO

-- Create a stored procedure
CREATE PROCEDURE spGetOfficesWithBranchName
AS
BEGIN

SELECT o.Id, o.Name, o.Code, o.Address, b.Name as BranchName, o.CreatedDateUtc FROM Office o 
LEFT OUTER JOIN Branch b 
ON o.BranchId = b.Id

END

