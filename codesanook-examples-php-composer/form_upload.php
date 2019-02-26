<?php
//initialize the session
if (!isset($_SESSION)) {
  session_start();
}

// ** Logout the current user. **
$logoutAction = $_SERVER['PHP_SELF']."?doLogout=true";
if ((isset($_SERVER['QUERY_STRING'])) && ($_SERVER['QUERY_STRING'] != "")){
  $logoutAction .="&". htmlentities($_SERVER['QUERY_STRING']);
}

if ((isset($_GET['doLogout'])) &&($_GET['doLogout']=="true")){
  //to fully log out a visitor we need to clear the session varialbles
  $_SESSION['MM_Username'] = NULL;
  $_SESSION['MM_UserGroup'] = NULL;
  $_SESSION['PrevUrl'] = NULL;
  unset($_SESSION['MM_Username']);
  unset($_SESSION['MM_UserGroup']);
  unset($_SESSION['PrevUrl']);
	
  $logoutGoTo = "index.php";
  if ($logoutGoTo) {
    header("Location: $logoutGoTo");
    exit;
  }
}
?>
<!DOCTYPE html
    PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>UTWcollectwork</title>

    <!-- Bootstrap -->
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"
        integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap-theme.min.css"
        integrity="sha384-fLW2N01lMqjakBkx3l/M9EahuwpSfeNvV63J5ezn3uZzapT0u7EYsXMjQV+0En5r" crossorigin="anonymous">

    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"
        integrity="sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS" crossorigin="anonymous">
    </script>
</head>

<body>
    <tr>
        <td class="text-right"> หัวเรื่อง </td>
        <td><input name="title" type="text" id="title" class="form-control" placeholder="" required></td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td class="text-right">รายละเอียด </td>
        <td><input name="txtMessage" type="text" id="txtMessage" class="form-control" placeholder="" required></td>
        <td></td>
        <td></td>
    </tr>
    <form action="uploadfile_db.php" method="post" enctype="multipart/form-data" name="form1" id="form1">
        <p>upload image :
            <label>
                <input type="file" name="img_file" id="img_file" />
            </label>

            <input type="submit" name="save" id="save" class="btn btn-info btn-sm" value="บันทึก"><strong></strong>

            <input name="Username" type="hidden" value="<?php echo $_SESSION['MM_Username']; ?>" />
        </p>
        <tr>
            <td class="text-right"> ยินดีต้อนรับคุณ <?php echo $_SESSION['MM_Username']; ?> </td>

            <td></td>
            <td></td>
        </tr>
        <a href="<?php echo $logoutAction ?>">ออกจากระบบ</a>

    </form>
</body>

</html>