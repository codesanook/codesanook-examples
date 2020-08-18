-- With recovery and norecovery mode
-- https://dba.stackexchange.com/questions/31383/what-is-the-difference-between-norecovery-and-recovery-when-restoring-database/31384
-- https://www.sqlservercentral.com/blogs/difference-between-restore-with-recovery-restore-with-norecovery

-- Restore full backup
RESTORE DATABASE Codesanook
FROM DISK = 'CodesanookFullBackup.bak'
WITH 
    -- Prevent restoring to using database files
    -- MOVE 'Codesanook' TO '/var/opt/mssql/data/Codesanook.mdf',
    -- MOVE 'Codesanook_log' TO '/var/opt/mssql/log/Codesanook_log.ldf',
    REPLACE
GO

-------------------- Select all users ------------------
USE Codesanook
GO

SELECT * FROM Users
GO

/*
-------------------- Insert data and create log backup 1 ------------------
INSERT INTO Users 
(Email, FirstName, LastName, DateOfBirth)
VALUES
('phoung@realman.com', 'Phuong', 'Realman', '2020-01-20')
GO

BACKUP LOG Codesanook 
TO DISK = 'CodesanookLogBackup1.trn'
WITH FORMAT 
GO
-- Restore transaction log

-------------------- Insert data and create differential backup base on the full backup ------------------
INSERT INTO Users 
(Email, FirstName, LastName, DateOfBirth)
VALUES
('phoung@realman.com', 'Phuong', 'Realman', '2020-01-20')
GO

BACKUP DATABASE Codesanook  
TO DISK = 'CodesanookDifferentialBackup.bak'
WITH DIFFERENTIAL, FORMAT 
GO

-- Restore differential backup

-------------------- Insert data and create log backup 2 ------------------
INSERT INTO Users 
(Email, FirstName, LastName, DateOfBirth)
VALUES
('pong@realman.com', 'Pong', 'Realman', '2020-01-20')
GO

BACKUP LOG Codesanook 
TO DISK = 'CodesanookLogBackup2.trn'
WITH FORMAT 
GO

-- Restore transaction log backup

-------------------- Select all users ------------------
SELECT * FROM Users
GO

-------------------- Truncate Users table ------------------
TRUNCATE TABLE Users
GO
*/
