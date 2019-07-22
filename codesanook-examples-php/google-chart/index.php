<?php
/*
Credit https://www.eggslab.net/google-chart-with-php-and-mysqli-database-using-google-api/
-- Create a database
CREATE DATABASE company

-- Create a table
CREATE TABLE `employees` (
`id` int(11) NOT NULL AUTO_INCREMENT,
`department` varchar(20) NOT NULL,
`employees` int(11) NOT NULL,
PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Insert some rows
INSERT INTO `employees` VALUES (NULL, 'HR', '10');
INSERT INTO `employees` VALUES (NULL, 'Engineering', '30');
INSERT INTO `employees` VALUES (NULL, 'Estimation', '25');
INSERT INTO `employees` VALUES (NULL, 'Accounts', '7');
INSERT INTO `employees` VALUES (NULL, 'IT', '9');
 */

//Connect a database and query data
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '1234');
define('DB_DATABASE', 'company');
$db = mysqli_connect(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_DATABASE);
$query = mysqli_query($db, 'SELECT * FROM employees');
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>PHP Google Chart</title>

    <!--Load the AJAX API-->
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
    // Load the Visualization API and the corechart package.
    google.charts.load('current', {
        'packages': ['corechart']
    });

    // Set a callback to run when the Google Visualization API is loaded.
    // Use callback to populate a data table and instantiates the pie chart.
    google.charts.setOnLoadCallback(()=>{

        const data = google.visualization.arrayToDataTable([
            ['Department', 'No. of Employees'], //column name
            <?php
$query = mysqli_query($db, 'SELECT * FROM employees');
while ($row = mysqli_fetch_array($query)) {
    echo "['" . $row['department'] . "'," . $row['employees'] . "],";
}
?>
        ]);

        // Set chart options
        const options = {
            'title': '% of Employees in each department',
            'width': 600,
            'height': 500
        };

        // Instantiate and draw our chart, passing in some options.
        const chart = new google.visualization.PieChart(document.getElementById('chart'));
        chart.draw(data, options);
    });
    </script>
</head>
</body>
    <!--Div that will hold a chart-->
    <div id="chart"></div>
</html>