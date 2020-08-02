Use master
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

