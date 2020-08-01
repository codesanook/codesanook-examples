# Build Docker project with build Docker file
```
    docker-compose up --build
```


# Remove Docker project with SQL volumes
```
    docker-compose down --volumes
```

# Convert CRLF to LF (Unix line feed)
```
    $path = ".\sa_password.secret"
    (Get-Content $path -Raw).Replace("`r`n", "`n") | Set-Content $Path -NoNewline -Force

```
