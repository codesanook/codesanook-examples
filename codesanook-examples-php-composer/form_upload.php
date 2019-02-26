<?php
//initialize the session
if (!isset($_SESSION)) {
    session_start();
    $_SESSION['MM_Username'] = "real man";
}
?>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>UTW collect work</title>
    <!-- Bootstrap -->
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"
        integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">
</head>

<body>
    <div class="container">
        <div class="row">
            <h3>Work form</h3>
            <div class="col-xs-12">
                <form action="uploadfile_db.php" method="post" enctype="multipart/form-data">
                    <div class="form-group">
                        <label>หัวเรื่อง</label>
                        <input name="title" class="form-control" />
                    </div>
                    <div class="form-group">
                        <label>รายละเอียด</label>
                        <input name="message" class="form-control" />
                    </div>
                    <div class="form-group">
                        <label>ไฟล์</label>
                        <input type="file" name="uploaded_file" class="form-control-file">
                    </div>
                    <input name="Username" type="hidden" value="<?php echo $_SESSION['MM_Username']; ?>" />
                    <button type="submit" class="btn btn-primary">บันทึก</button>
                </form>
            </div>
        </div>
    </div>
</body>

</html>