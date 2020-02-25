FROM mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04

EXPOSE 1433
COPY ./init-db.sh /
COPY ./create-database-objects.sql /

RUN whoami
USER root
RUN chmod +x ./init-db.sh
USER mssql

CMD  ./init-db.sh & /opt/mssql/bin/sqlservr
