Import-Module Pester
Import-Module .\Npgsql.psm1 -DisableNameChecking -Force

Describe "Using Ngsql module" {
    $connectionString = "Server=localhost;Port=5432;Database=membership;User Id=postgres;Password=root"
    $dllPath = $PSScriptRoot + "\Npgsql.dll"
    Load-Driver $dllPath 

    # define a primary key in PostgreSQL
    # https://chartio.com/resources/tutorials/how-to-define-an-auto-increment-primary-key-in-postgresql/
    It "should create a book table" {
        Run-UnitOfWork {
            # normal queries
            Invoke-Query -Void '
                CREATE TABLE books (
                id              SERIAL PRIMARY KEY,
                title           VARCHAR(100) NOT NULL
                );'
        } -ConnectionString $connectionString `
            -Debug `
            -AutoCommit # not roll back the transaction when done.
    }

    It "should insert a book record" {
        $value = Run-UnitOfWork {
            # normal queries
            Invoke-Query -Void '
                INSERT INTO books 
                (title) 
                VALUES 
                (:bookTitle)' @{
                bookTitle = 'Mastering Java'
            } -ReturnId "books_id_seq"
        } -ConnectionString $connectionString `
            -Debug `
            -AutoCommit # not roll back the transaction when done.

        $($value[1]) | Should -GT 0 
    }

    It "should return from select statement" {
        $result = Run-UnitOfWork {
            Invoke-Query ' select id, title from books'
        } -ConnectionString $connectionString `
            -Debug `
            -AutoCommit # not roll back the transaction when done.
        $result.Length | Should -GT 1  
    }
}