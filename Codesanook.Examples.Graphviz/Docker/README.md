## How to run the project
- Create .env file  
- Add `SA_PASSWORD=Your-sa-password` to .env file
- Run `docker-compose up --build`
- Connect to a server with SSMS
    - host name: localhost, 1444 
    - authentication: SQL Server Authentication
    - user: sa
    - password: as specified in .env file
- You will find `EventManagementSystem` database

## Read existing data
```
USE EventManagementSystem
GO

SELECT * FROM [Event]
SELECT * FROM [User]
SELECT * FROM [EventBooking]
SELECT * FROM [EventAttendee]
```

