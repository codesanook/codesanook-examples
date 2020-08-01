FROM microsoft/mssql-server-linux:latest

# Define the environment environment which can be accessed to all .sh files
ENV MSSQL_SA_PASSWORD=

EXPOSE 1433
COPY ./entrypoint.sh /
COPY ./init-db.sh /
COPY ./create-db.sql /

RUN whoami

RUN chmod +x /entrypoint.sh
RUN chmod +x /init-db.sh

ENTRYPOINT ["/entrypoint.sh"]

