## To lauch PHP SQL Server Docker compose 

- Open a terminal CD to codesanook-examples-php folder
- Create sa_password.scret with LF end of line
- Add your SA password to sa_password.scret and save
- Run `docker compose-up`


## Connect to SQL Server with SSML or Azure data studio
- Use the following values: 
    - Server: localhost, 1433 
    - Authentiction type: SQL Login
    - User name: SA
    - Password: use a password that you set in sa_password.secret file

## Useful SQL server

Create a new database with a default configuration
```
USE master
GO

CREATE DATABASE [database-name]
GO
```
