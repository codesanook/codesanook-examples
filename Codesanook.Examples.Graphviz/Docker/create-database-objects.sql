SELECT @@VERSION
GO

CREATE DATABASE EventManagementSystem
GO

USE EventManagementSystem
GO

CREATE TABLE [Event]
(
    Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(64) NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    Location NVARCHAR(255) NOT NULL,
    StartDateTime DATETIME NOT NULL,
    EndDateTime DATETIME NOT NULL,
    MaximumNumberOfAttendees INT NOT NULL,
) 
GO

CREATE TABLE [User]
(
    Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(64) NOT NULL,
    Email NVARCHAR(64) NOT NULL,
    MobilePhoneNumber CHAR(10) NOT NULL,
) 
GO

CREATE TABLE [EventBooking]
(
    Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    UserId INT NOT NULL,
    BookingDateTime DATETIME NOT NULL,
    NumberOfTickets INT NOT NULL,
    EventStatus INT NOT NULL,
    PaidDateTime DATETIME NULL,
    PaymentConfirmationAttachement VARCHAR(2048) NULL,
) 
GO

CREATE TABLE [EventAttendee]
(
    Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    EventBookingId INT NOT NULL,
    FirstName NVARCHAR(64) NOT NULL,
    LastName NVARCHAR(64) NOT NULL,
    Email NVARCHAR(64) NOT NULL,
    MobilePhoneNumber CHAR(10) NOT NULL,
    OrganizationName NVARCHAR(64) NOT NULL,
) 
GO


INSERT INTO [User]
VALUES
    ('ThepPong',
        'pong@codesanook.com',
        '0812345678'
);

INSERT INTO [Event]
VALUES
    ('.NET Conf',
        '.NET Conf 2020',
        'BKK',
        '2020-02-14',
        '2020-02-14',
        30
);

INSERT INTO [EventBooking]
VALUES
    (1,
        1,
        '2020-02-14',
        2,
        1,
        '2020-02-14',
        '/codesanook.com/slip.jpg'
);

INSERT INTO [EventAttendee]
VALUES
    (1,
        'Pong',
        'Codesanook',
        'Pong@codesanook.com',
        '0812345678',
        'Codesanook'
);

INSERT INTO [EventAttendee]
VALUES
    (1,
        'Aaron',
        'Codesanook',
        'aaron@codesanook.com',
        '0812345678',
        'Codesanook'
);

GO