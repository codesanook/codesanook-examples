## How to run the project

-   Create `sa_password.secret` file and add your preferred SQL Sever password.

-   Launch a terminal with PowerShell session

```
    docker-compose up
```

-   To backup a database

```
    .\Invoke-BackupDatabase.ps1
```

-   To restore a database

```
    .\Invoke-RestoreDatabase.ps1
```

-   Backup and restore scripts are in `backup-database.sql` and `restore-database.sql`.

## Other useful tips

## Build Docker project with build Docker file

```
    docker-compose up --build
```

## Remove Docker project with SQL volumes

```
    docker-compose down --volumes
```

### Convert CRLF to LF (Convert to Unix line feed)

```
    $path = ".\sa_password.secret"
    (Get-Content $path -Raw).Replace("`r`n", "`n") | Set-Content $Path -NoNewline -Force

```
