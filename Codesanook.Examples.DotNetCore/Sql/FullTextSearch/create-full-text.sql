-- Full text search SQL example SQL Query

-- Create view with SCHEMABINDING
-- What is SCHEMABINDING https://blog.sqlauthority.com/2019/10/06/what-is-schemabinding-in-sql-server-views-interview-question-of-the-week-245/
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
GO

-- Create a unique index
CREATE UNIQUE CLUSTERED INDEX UX_PersonPhoneNumbers_BusinessEntityID
ON dbo.PersonPhoneNumbers (BusinessEntityID)
GO

-- Create a full text catalogue
CREATE FULLTEXT CATALOG AW2008FullTextDemoCatalog
GO

-- Create a full text index
CREATE FULLTEXT INDEX ON dbo.PersonPhoneNumbers (
    PhoneNumber,
    FirstName
)
KEY INDEX UX_PersonPhoneNumbers_BusinessEntityID 
ON (AW2008FullTextDemoCatalog)
WITH (CHANGE_TRACKING AUTO)
GO

-- Optionally Add column to full text index
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
