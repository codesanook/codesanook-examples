# Connect to master database to restore a database
$connectionString = "Server=localhost; Database=master; User Id=SA; Password=12345Abc%"
Invoke-Sqlcmd -ConnectionString $ConnectionString -InputFile "./restore-database.sql" | Format-Table -AutoSize
