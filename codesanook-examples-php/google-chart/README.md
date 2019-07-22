
### Install PHP with Chocolatey on Windows
Install Chocolatey if you haven't install it.
Launch PowerShell terminal with administration permission.
```
 choco install php
```

### Install Composer
```
    choco install composer -y
```

### Install packages
```
    composer install
```

### Run the project
To run with built-in PHP server with the following commands
```
    php -S localhost:9999
```
Open web browser and navigate to http://localhost:9999


### Install MySQL in case you don't have it in your machine
```
    choco install mysql -y
```

## To Connect to MySQL in PowerShell Terminal
```
    mysql -u=root
```
!!!Please note that this is just an example project and we use a default password value and use root as a user which is not suitable for a production environment.
Alter the password as root
```
    ALTER USER 'root'@'localhost' IDENTIFIED WITH mysql_native_password BY '1234';
```
### Create a test database and table
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
### Enable PHP SQLi extension

Uncomment mysqli extension in C:\tools\php73\php.ini as following
```
    extension=mysqli
```
Also set display_errors On which is useful for debugging
```
    display_errors = On
    file_uploads = On
```

### Useful links:
Getting start with PHP
https://www.codementor.io/jadjoubran/php-tutorial-getting-started-with-composer-8sbn6fb6t

### Credit
- https://www.eggslab.net/google-chart-with-php-and-mysqli-database-using-google-api/
