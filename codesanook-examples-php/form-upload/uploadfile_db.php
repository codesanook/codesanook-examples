<?php
header('Content-Type: text/html; charset=utf-8');
//Set ว/ด/ป เวลา ให้เป็นของประเทศไทย
date_default_timezone_set('Asia/Bangkok');

//รับชื่อไฟล์จากฟอร์ม
$title = $_POST['title'];
$message = $_POST['message'];
$username = $_POST['username'];
echo "$title, $message, $username";

if (!file_exists($_FILES['uploaded_file']['tmp_name'])) {
    throw new Exception("no uploaded file");
}

//สร้างตัวแปรวันที่เพื่อเอาไปตั้งชื่อไฟล์ที่อัพโหลด
$date = date("Ymd_his");
//สร้างตัวแปรสุ่มตัวเลขเพื่อเอาไปตั้งชื่อไฟล์ที่อัพโหลดไม่ให้ชื่อไฟล์ซ้ำกัน
$numRand = (mt_rand());

//โฟลเดอร์ที่เก็บไฟล์
$fileFolder = "uploaded-files";
//ตัวขื่อกับนามสกุลภาพออกจากกัน
$type = strrchr($_FILES['uploaded_file']['name'], ".");
//ตั้งชื่อไฟล์ใหม่เป็น สุ่มตัวเลข+วันที่
$newName = $numRand . $date . $type;
$filePath = $fileFolder . '/' . $newName;
move_uploaded_file($_FILES['uploaded_file']['tmp_name'], $filePath);

$connection = mysqli_connect("localhost", "root", "root", "work_db");
// Check connection
if (mysqli_connect_errno()) {
    echo "Failed to connect to MySQL: " . mysqli_connect_error();
}

$sql = <<<SQL
    INSERT INTO work(`username`, `title`, `message`, `file_path`)
    VALUES ('$username','$title', '$message', '$filePath');
SQL;

$result = mysqli_query($connection, $sql) or die("Error in query: $sql " . mysqli_error());
mysqli_close($connection);

if ($result) {
    echo "<script>";
    echo "alert('อัพโหลดสำเร็จ!');";
    echo "location='form_upload.php';";
    echo "</script>";
} else {
    echo "<script>";
    echo "alert('Error!!! อัพโหลดไม่สำเร็จ กรุณาลองใหม่')";
    echo "location='form_upload.php';";
    echo "</script>";
}