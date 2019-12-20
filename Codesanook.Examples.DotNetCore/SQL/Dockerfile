# Base OS layer: Latest Ubuntu LTS + mssql-server-linux (SQL Server engine + tools)
FROM mcr.microsoft.com/mssql/server:2017-CU3-ubuntu

# Install curl since it is needed to get repo config
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

RUN whoami
EXPOSE 1433

RUN mkdir ./data

# Copy back file to the container
COPY ./AdventureWorks2012.bak /var/opt/mssql/data

COPY ./init-db.sh /
COPY ./create-db.sql /
RUN chmod +x ./init-db.sh

# Run SQL Server process
CMD ./init-db.sh & /opt/mssql/bin/sqlservr

