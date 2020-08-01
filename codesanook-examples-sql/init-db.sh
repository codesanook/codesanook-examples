#!/bin/bash
echo "***** init db *****"
echo  "MSSQL_SA_PASSWORD '$MSSQL_SA_PASSWORD'"

# Wait for the SQL Server starting up 
sleep 10s
/opt/mssql-tools/bin/sqlcmd -U sa -P $MSSQL_SA_PASSWORD -i create-db.sql -d master

