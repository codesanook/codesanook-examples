#!/bin/bash

# Intercept set SA_PASSWORD from Docker secret MSSQL_SA_PASSWORD_FILE
# https://stackoverflow.com/a/13864829/1872200
if [! -z ${MSSQL_SA_PASSWORD_FILE}]
then
    # Set as a variable
    MSSQL_SA_PASSWORD=$(cat $MSSQL_SA_PASSWORD_FILE)
fi

# Run init-db.sh and run SQL Server
/init-db.sh & /opt/mssql/bin/sqlservr

