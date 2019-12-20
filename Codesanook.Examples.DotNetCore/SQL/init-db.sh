#!/bin/bash
echo "***** init db *****"
echo  $SA_PASSWORD

# Wait for the SQL Server to come up
sleep 10s
/opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -i create-db.sql -d master &>/dev/null