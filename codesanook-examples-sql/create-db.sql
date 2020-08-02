USE master
GO

SELECT @@VERSION
GO

IF DB_ID('Codesanook') IS NULL
BEGIN
    PRINT 'DB does not exist, create new'
    CREATE DATABASE Codesanook
END
ELSE
BEGIN
    PRINT 'DB exists, not create a new one'
END
GO

-- Switch to Codesanook database
USE Codesanook
GO

-- U is a user-defined table
-- https://docs.microsoft.com/en-us/sql/relational-databases/system-catalog-views/sys-objects-transact-sql?view=sql-server-ver15
IF OBJECT_ID('Users', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Users]
    (
        [Id] [INT] NOT NULL IDENTITY(1,1),
        [Email] [nvarchar](64) NOT NULL,
        [FirstName] [nvarchar](64) NOT NULL,
        [LastName] [nvarchar](64) NOT NULL,
        [DateOfBirth] [datetime] NULL,
        CONSTRAINT [PK_Users_Id] PRIMARY KEY CLUSTERED ([ID] ASC) -- Create a table with custom primary name
    )
END
GO

