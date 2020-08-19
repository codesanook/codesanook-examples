-- With recovery and norecovery mode
-- https://dba.stackexchange.com/questions/31383/what-is-the-difference-between-norecovery-and-recovery-when-restoring-database/31384
-- https://www.sqlservercentral.com/blogs/difference-between-restore-with-recovery-restore-with-norecovery

-------------------- Restore with full backup file ------------------
USE master 
GO

RESTORE DATABASE Codesanook
FROM DISK = 'CodesanookFullBackup.bak'
WITH 
    -- Prevent restoring to using database files
    -- MOVE 'Codesanook' TO '/var/opt/mssql/data/Codesanook.mdf',
    -- MOVE 'Codesanook_log' TO '/var/opt/mssql/log/Codesanook_log.ldf',
    REPLACE
GO

USE Codesanook
GO

SELECT * FROM Users
GO

-------------------- Restore with log backup file ------------------
WAITFOR DELAY '00:00:02' -- Wait for two seconds before start a new operation

USE master 
GO

RESTORE DATABASE Codesanook
FROM DISK = 'CodesanookFullBackup.bak'
WITH 
    NORECOVERY,
    REPLACE
GO

RESTORE Log Codesanook
FROM DISK = 'CodesanookLogBackup1.trn'
WITH 
    RECOVERY,
    REPLACE
GO

USE Codesanook
GO

SELECT * FROM Users
GO

-------------------- Restore with differential backup file ------------------
WAITFOR DELAY '00:00:02' -- Wait for two seconds before start a new operation

USE master 
GO

RESTORE DATABASE Codesanook
FROM DISK = 'CodesanookFullBackup.bak'
WITH 
    NORECOVERY,
    REPLACE
GO

RESTORE DATABASE Codesanook
FROM DISK = 'CodesanookDifferentialBackup.bak'
WITH 
    RECOVERY,
    REPLACE
GO

USE Codesanook
GO

SELECT * FROM Users
GO

-------------------- Restore with multiple transaction log backup files ------------------
WAITFOR DELAY '00:00:02' -- Wait for two seconds before start a new operation

USE master 
GO

RESTORE DATABASE Codesanook
FROM DISK = 'CodesanookFullBackup.bak'
WITH 
    NORECOVERY,
    REPLACE
GO

RESTORE LOG Codesanook
FROM DISK = 'CodesanookLogBackup1.trn'
WITH 
    NORECOVERY,
    REPLACE
GO

RESTORE LOG Codesanook
FROM DISK = 'CodesanookLogBackup2.trn'
WITH 
    RECOVERY,
    REPLACE
GO

USE Codesanook
GO

SELECT * FROM Users
GO

-------------------- Restore with full, differential and log backup files ------------------
WAITFOR DELAY '00:00:02' -- Wait for two seconds before start a new operation

USE master 
GO

RESTORE DATABASE Codesanook
FROM DISK = 'CodesanookFullBackup.bak'
WITH 
    NORECOVERY,
    REPLACE
GO

RESTORE DATABASE Codesanook
FROM DISK = 'CodesanookDifferentialBackup.bak'
WITH 
    NORECOVERY,
    REPLACE
GO

RESTORE LOG Codesanook
FROM DISK = 'CodesanookLogBackup2.trn'
WITH 
    RECOVERY,
    REPLACE
GO

USE Codesanook
GO

SELECT * FROM Users
GO

