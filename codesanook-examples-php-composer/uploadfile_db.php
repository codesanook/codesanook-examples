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

/*
//$conn= mysqli_connect ($serverName,$userName,$userPassword,$dbName);
//$sql = "INSERT INTO collectwork(title,txtMessage)
//VALUES ('".$_POST["title"]."','".$_POST["txtMessage"]."')";
$sql = "INSERT INTO collectwork

( file_path, title, message, username)

VALUES

('$newname', '$path_link', '$title', '$txtMessage', '$Username')
";
$result = mysql_db_query($database_myconnect, $sql) or die("Error in query: $sql " . mysql_error());
mysql_close();
if ($result) {
echo "<script type='text/javascript'>";
echo "alert('อัพโหลดสำเร็จ!');";
echo "window.location='form_upload.php';";
echo "</script>";
} else {
echo "<script type='text/javascript'>";
echo "window.location='form_upload.php';";
echo "</script>";
}
}
 */