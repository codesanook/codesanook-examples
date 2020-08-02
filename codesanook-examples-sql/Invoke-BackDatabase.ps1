$connectionString = "Server=localhost; Database=Codesanook; User Id=SA; Password=12345Abc%"
Invoke-Sqlcmd -ConnectionString $ConnectionString -InputFile "./backup-database.sql" | Format-Table
