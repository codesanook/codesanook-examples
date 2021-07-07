-- https://www.sqlshack.com/understanding-sql-server-backup-types/
-- https://blog.sqlauthority.com/2018/03/07/sql-server-tail-log-backups/
-- Should I use semicolon https://stackoverflow.com/a/710697/1872200

-------------------- Truncate Users table, make a clean state ------------------
USE Codesanook
GO

TRUNCATE TABLE Users
GO

-------------------- Insert data and create full backup ------------------
USE Codesanook
GO

INSERT INTO Users 
(Email, FirstName, LastName, DateOfBirth)
VALUES
('jose@realman.com', 'Jose', 'Realman', '2020-01-20')
GO

-- Using WITH FORMAT to overwrite any existing backups and create a new media set.
-- Backup to a default location /var/opt/mssql/backup/Codesanook.bak
USE master
GO

BACKUP DATABASE Codesanook
TO DISK = 'CodesanookFullBackup.bak'   
WITH FORMAT 
GO  

-------------------- Insert data and create log backup 1 ------------------
USE Codesanook
GO

INSERT INTO Users 
(Email, FirstName, LastName, DateOfBirth)
VALUES
('phuong@realman.com', 'Phuong', 'Realman', '2020-01-20')
GO

-- Backup to the default log location /var/opt/mssql/log/Codesanook_log.ldf
USE master
GO

BACKUP LOG Codesanook 
TO DISK = 'CodesanookLogBackup1.trn'
WITH FORMAT 
GO

-------------------- Insert data and create differential backup base on the full backup ------------------
USE Codesanook
GO

INSERT INTO Users 
(Email, FirstName, LastName, DateOfBirth)
VALUES
('you@realman.com', 'You', 'Realman', '2020-01-20')
GO

USE master
GO

BACKUP DATABASE Codesanook  
TO DISK = 'CodesanookDifferentialBackup.bak'
WITH DIFFERENTIAL, FORMAT 
GO

-------------------- Insert data and create log backup 2 ------------------
USE Codesanook
GO

INSERT INTO Users 
(Email, FirstName, LastName, DateOfBirth)
VALUES
('pong@realman.com', 'Pong', 'Realman', '2020-01-20')
GO

USE master
GO

BACKUP LOG Codesanook 
TO DISK = 'CodesanookLogBackup2.trn'
WITH FORMAT 
GO

USE Codesanook
GO

SELECT * FROM Users
GO

