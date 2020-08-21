## To start Docker
### Tested with Docker Windows Desktop and Linux Container   
- Run the following commands.

Start your PowerShell session then
```
docker-compose up
```

- Connect with SSMS to localhost, 1433
- username: sa
- password: 12345Abc%
- To learn full text search, you can execute content of `./full-text-search/create-full-text-search.sql`

## Optional to execute inside a Docker container
- Start docker again with detach mode

```
docker-compose up -d
# Wait for 30 seconds
docker exec -it sql-db bash

```
- Then you will have interactive bash shell
- To use Sqlcmd command prompt
```
/opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD 
```
- After that, you can test with SQL command 
```
SELECT @@VERSION
GO
```
- You will get the result looks like
```
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Microsoft SQL Server 2017 (RTM-CU18) (KB4527377) - 14.0.3257.3 (X64)
        Nov 16 2019 01:14:50
        Copyright (C) 2017 Microsoft Corporation
        Developer Edition (64-bit) on Linux (Ubuntu 16.04.6 LTS)                                       

(1 rows affected)
```
