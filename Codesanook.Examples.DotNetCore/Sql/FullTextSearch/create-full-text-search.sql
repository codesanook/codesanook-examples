USE AdventureWorks2012
GO
-- Full text search SQL example SQL Query
-- Create view with SCHEMABINDING because we want to create an index to this view.
-- What is SCHEMABINDING https://blog.sqlauthority.com/2019/10/06/what-is-schemabinding-in-sql-server-views-interview-question-of-the-week-245/

IF OBJECT_ID('PersonPhoneNumbers', 'V') IS NULL
BEGIN
    -- Create a view that joins multiple table with SCHEMABINDING so that we can create index on it.
    EXEC ('
        CREATE view PersonPhoneNumbers 
        WITH SCHEMABINDING
        AS

        SELECT 
            p.BusinessEntityID, 
            f.PhoneNumber, 
            p.FirstName 
        FROM 
            Person.Person p
            INNER JOIN Person.PersonPhone f
            ON p.BusinessEntityID = f.BusinessEntityID

    ')
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_PersonPhoneNumbers_BusinessEntityID')
BEGIN
    -- Create a unique index, one column on a view object
    CREATE UNIQUE CLUSTERED INDEX UX_PersonPhoneNumbers_BusinessEntityID
    ON dbo.PersonPhoneNumbers (BusinessEntityID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PersonPhoneNumbers_FullName')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_PersonPhoneNumbers_FullName]
    ON [dbo].[PersonPhoneNumbers] ([FirstName])
    INCLUDE ([PhoneNumber])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = 'AW2008FullTextDemoCatalog')
BEGIN
    -- Create a full text catalog
    CREATE FULLTEXT CATALOG AW2008FullTextDemoCatalog
END
GO

IF NOT EXISTS (
    SELECT 1 
    FROM sys.fulltext_indexes 
    WHERE object_id = object_id('PersonPhoneNumbers') --table/view name 
)
BEGIN
    -- Create a full text index
    CREATE FULLTEXT INDEX ON dbo.PersonPhoneNumbers (
        PhoneNumber,
        FirstName
    )
    KEY INDEX UX_PersonPhoneNumbers_BusinessEntityID ON (AW2008FullTextDemoCatalog) 
    WITH (CHANGE_TRACKING AUTO)
END
GO

-- Optionally how to add column to full text index
-- ALTER FULLTEXT INDEX ON dbo.PersonPhoneNumbers ADD (PhoneNumber)
-- Query full text
SELECT * 
FROM dbo.PersonPhoneNumbers
WHERE freetext ((PhoneNumber, FirstName), 'ken')
GO

SELECT * 
FROM dbo.PersonPhoneNumbers
WHERE freetext ((PhoneNumber, FirstName), '149')
GO

SELECT * 
FROM dbo.PersonPhoneNumbers
WHERE CONTAINS ((PhoneNumber, FirstName), '949 OR ken')
GO

SELECT * 
FROM dbo.PersonPhoneNumbers
WHERE FREETEXT ((PhoneNumber, FirstName), '949 ken')
GO

-- '"xx*"' acts the same as like xx%
SELECT * 
FROM dbo.PersonPhoneNumbers
WHERE CONTAINS ( FirstName, '"ke*"')
GO

-- Like start with
SELECT * 
FROM dbo.PersonPhoneNumbers
WHERE FirstName LIKE 'ke%'
GO

-- Like contains
SELECT * 
FROM dbo.PersonPhoneNumbers
WHERE FirstName LIKE '%ke%'
GO

-- On a table and existing index
SELECT LastName FROM Person.Person WHERE LastName LIKE 'ch%' 

SELECT LastName FROM Person.Person WHERE LastName LIKE '%ch%'

