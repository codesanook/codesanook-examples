## Install Composer on Windows
Install Chocolatey if you haven't install it.

Launch PowerShell terminal with administration permission.
```
choco install composer -y
```

## Initialize project
Launch a new PowerShell terminal
```
composer init
```

Open a project with VS Code

create index.php

add the following code to index.php to test if we setup PHP correctly.

```
<?php
    phpinfo();
?>

```
Save an run with built-in PHP server with the following commands

```
php -S localhost:9999
```

Open web browser and navigate to http://localhost:9999



