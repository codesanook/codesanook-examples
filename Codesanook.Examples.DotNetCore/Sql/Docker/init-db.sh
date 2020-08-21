#!/bin/bash
echo "***** Initialize a database *****"
echo  "MSSQL_SA_PASSWORD '$MSSQL_SA_PASSWORD'"

# Wait for the SQL Server starting up and is ready to use
sleep 10s

# Use i to set input SQL script file
# https://docs.microsoft.com/en-us/sql/tools/sqlcmd-utility?view=sql-server-ver15#syntax
# /scripts/ map to host directory
/opt/mssql-tools/bin/sqlcmd -U sa -P $MSSQL_SA_PASSWORD -d master -i /scripts/init-db.sql 

