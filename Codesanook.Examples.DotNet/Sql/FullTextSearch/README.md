# SQL Server full text search example

## How to run the project.

-   Create `sa_password.secret` file in `FullTextSearch` folder and put your preferred SQL Sever password in the file.

-   Launch a terminal with PowerShell session and start SQL Server Docker with a following command:

```
    .\Start-SqlServer.ps1

```

-   Wait for a while, when it finish, you will see the following output:

```
    Is full text search enabled: 1

```

This will create SQL Server Linux Docker with full text search feature.

-   To create full text search catelog, index and run example, launch a new PowerShell session.
    Then use the following command:

```
    .\Invoke-CreateFullTextSearch.ps1
```

-   To stop a SQL Server Docker container, press CTRL+C and wait for a while it will remove container, images and volumes.

-   Full text search SQL scripts are in `create-full-text-search.sql`.
-   The connection string is in `Invoke-CreateFullTextSearch.ps1`

## Other useful tips

### If you have problems with CRLF in .sh, .secret file, use the following command to convert CRLF to LF (Unix line feed)

```
    $path = ".\sa_password.secret"
    (Get-Content $path -Raw).Replace("`r`n", "`n") | Set-Content $Path -NoNewline -Force

```
