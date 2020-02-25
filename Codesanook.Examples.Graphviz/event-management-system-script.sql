--- event-management-system.sqlite

CREATE TABLE IF NOT EXISTS Event (
    Id INTEGER PRIMARY KEY, --   this column is the alias of the rowid column.
    Name TEXT NOT NULL,
    Description TEXT NOT NULL,
    Location TEXT NOT NULL,
    StartDateTime INTEGER NOT NULL,
    EndDateTime INTEGER NOT NULL,
    MaximumNumberOfAttendees INTEGER NOT NULL
) 

CREATE TABLE IF NOT EXISTS User (
    Id INTEGER PRIMARY KEY,
    Username TEXT NOT NULL,
    Email TEXT NOT NULL,
    MobilePhoneNumber TEXT NOT NULL
) 

CREATE TABLE IF NOT EXISTS EventBooking (
    Id INTEGER PRIMARY KEY,
    EventId INTEGER NOT NULL,
    UserId INTEGER NOT NULL,
    BookingDateTime INTEGER NOT NULL,
    NumberOfTickets INTEGER NOT NULL,
    EventStatus INTEGER NOT NULL,
    PaidDateTime INTEGER NOT NULL,
    PaymentConfirmationAttachement TEXT NOT NULL
) 

CREATE TABLE IF NOT EXISTS EventAttendee (
    Id INTEGER PRIMARY KEY,
    EventBookingId INTEGER NOT NULL,
    FirstName Text NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT NOT NULL,
    MobilePhoneNumber TEXT NOT NULL,
    OrganizationName TEXT NOT NULL
) 
