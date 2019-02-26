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


Install MySQL

```
choco install mysql -y
```

Connect to MySQL
In PowerShell Terminal
mysql -u=root
!!!Please note that this is just an example project and we use a default password value and use root as a user which is not for a production environment.

Alter the password as root

ALTER USER 'root'@'localhost' IDENTIFIED WITH mysql_native_password BY 'root';

Create a test database and table

```
CREATE DATABASE work_db;

-- ----------------------------
-- Table structure for `work` table
-- ----------------------------
CREATE TABLE `work` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `username` varchar(50) NOT NULL,
    `title` varchar(50) NOT NULL,
    `message` varchar(255) NOT NULL,
    `file_path` varchar(512) NOT NULL,
    PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

```
Enable PHP SQLi extension

Edit 
Uncomment mysqli extension in C:\tools\php73\php.ini as following
```
extension=mysqli
```

Also set display_errors On which is useful for debugging
display_errors = On




