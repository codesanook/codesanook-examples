#!/bin/bash
echo "***** init db *****"
echo  $SA_PASSWORD

# Wait for the SQL Server to come up
sleep 30s

echo "Start restoring a database"
/opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -i create-db.sql -d master &>/dev/null
/opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -i restore-db.sql -d master

# Check if full text search enable
/opt/mssql-tools/bin/sqlcmd \
-U sa \
-P $SA_PASSWORD \
-q "SELECT FULLTEXTSERVICEPROPERTY('IsFullTextInstalled')" \
-d master
