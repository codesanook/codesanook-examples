USE master
GO

-- https://docs.microsoft.com/en-us/sql/linux/tutorial-restore-backup-in-sql-server-container?view=sql-server-ver15
RESTORE DATABASE AdventureWorks2012 
FROM DISK = '/var/opt/mssql/backup/AdventureWorks2012.bak'

WITH REPLACE,
MOVE 'AdventureWorks2012' TO '/var/opt/mssql/data/AdventureWorks2012_DATA_FILE.mdf',
MOVE 'AdventureWorks2012_log' TO '/var/opt/mssql/data/AdventureWorks2012_LOG_FILE.ldf'

GO