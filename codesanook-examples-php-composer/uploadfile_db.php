<?php 
	require_once('Connections/myconnect.php');
	//Set ว/ด/ป เวลา ให้เป็นของประเทศไทย
	date_default_timezone_set('Asia/Bangkok');
	//สร้างตัวแปรวันที่เพื่อเอาไปตั้งชื่อไฟล์ที่อัพโหลด
	$date1 = date("Ymd_his");
	//สร้างตัวแปรสุ่มตัวเลขเพื่อเอาไปตั้งชื่อไฟล์ที่อัพโหลดไม่ให้ชื่อไฟล์ซ้ำกัน
	$numrand = (mt_rand());
	//รับชื่อไฟล์จากฟอร์ม 
	$title = $_POST['title'];
	$txtMessage = $_POST['txtMessage'];
	$Username = $_POST['Username'];
	$img_file = (isset($_REQUEST['img_file']) ? $_REQUEST['img_file'] : '');
	
	$upload=$_FILES['img_file'];
	if($upload <> '') { 
 
	//โฟลเดอร์ที่เก็บไฟล์
	$path="img/";
	//ตัวขื่อกับนามสกุลภาพออกจากกัน
	$type = strrchr($_FILES['img_file']['name'],".");
	//ตั้งชื่อไฟล์ใหม่เป็น สุ่มตัวเลข+วันที่
	$newname =$numrand.$date1.$type;
 
	$path_copy=$path.$newname;
	$path_link="img/".$newname;
	//คัดลอกไฟล์ไปยังโฟลเดอร์
	//$serverName="localhost";
	//$userName="root";
	//$userPassword="";
	//$dbName="UTWcollectwork";

	//$conn= mysqli_connect ($serverName,$userName,$userPassword,$dbName);
	//$sql = "INSERT INTO collectwork(title,txtMessage)
	//VALUES ('".$_POST["title"]."','".$_POST["txtMessage"]."')";
	move_uploaded_file($_FILES['img_file']['tmp_name'],$path_copy);  
	$sql = "INSERT INTO collectwork

		(img_file, path_link, title, txtMessage, Username) 

		VALUES

		('$newname', '$path_link', '$title', '$txtMessage', '$Username')
	";
	$result = mysql_db_query($database_myconnect, $sql) or die ("Error in query: $sql " . mysql_error());
	mysql_close();
	if($result){
		echo "<script type='text/javascript'>";
		echo  "alert('อัพโหลดสำเร็จ!');";
		echo "window.location='form_upload.php';";
		echo "</script>";
	} else{
		echo "<script type='text/javascript'>";
		echo "window.location='form_upload.php';";
		echo "</script>";
	}
?>