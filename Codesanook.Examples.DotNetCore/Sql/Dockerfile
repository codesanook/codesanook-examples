# mssql-agent-fts-ha-tools
# Maintainers: Microsoft Corporation (LuisBosquez and twright-msft on GitHub)
# GitRepo: https://github.com/Microsoft/mssql-docker

# Base OS layer: Latest Ubuntu LTS + mssql-server-linux (SQL Server engine + tools)
#FROM mcr.microsoft.com/mssql/server:2017-CU14-ubuntu	
FROM mcr.microsoft.com/mssql/server:2017-latest

#Install curl since it is needed to get repo config
# Get official Microsoft repository configuration
RUN export DEBIAN_FRONTEND=noninteractive && \
    apt-get update && \
    apt-get install -y curl  && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/16.04/mssql-server-2017.list | tee /etc/apt/sources.list.d/mssql-server.list && \
    apt-get update

# Install optional packages.  Comment out the ones you don't need
# RUN apt-get install -y mssql-server-agent
# RUN apt-get install -y mssql-server-ha
RUN apt-get install -y mssql-server-fts

# Download the Microsoft repository GPG keys
RUN wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb

# Register the Microsoft repository GPG keys
RUN dpkg -i packages-microsoft-prod.deb

# Update the list of products
RUN apt-get update

# Install PowerShell
RUN apt-get install -y powershell

# Start PowerShell
RUN pwsh --version

# Default SQL Server TCP/Port. 1433 has been export in base image
# https://github.com/microsoft/mssql-docker/blob/master/linux/mssql-server-linux/Dockerfile
COPY ./create-db.sql /
COPY ./restore-db.sql /

# Set a local variable
# https://stackoverflow.com/a/33979066/1872200
RUN backupPath="/var/opt/mssql/backup" && echo $backupPath
RUN mkdir /var/opt/mssql/backup
COPY ./AdventureWorks2012.bak /var/opt/mssql/backup

COPY ./init-db.sh /
RUN chmod +x ./init-db.sh

# Run init-db.sh and SQL Server process
CMD ./init-db.sh & /opt/mssql/bin/sqlservr
