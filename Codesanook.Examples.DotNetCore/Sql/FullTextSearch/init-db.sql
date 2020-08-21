USE master
GO

-- https://docs.microsoft.com/en-us/sql/linux/tutorial-restore-backup-in-sql-server-container?view=sql-server-ver15
-- Restore full database backup
RESTORE DATABASE AdventureWorks2012 
-- /scripts/ is mapped to host example host folder, e.g. FullTextSearch folder
FROM DISK = '/scripts/AdventureWorks2012.bak'
WITH 
    REPLACE,
    MOVE 'AdventureWorks2012' TO '/var/opt/mssql/data/AdventureWorks2012_DATA_FILE.mdf',
    MOVE 'AdventureWorks2012_log' TO '/var/opt/mssql/log/AdventureWorks2012_LOG_FILE.ldf'

GO
