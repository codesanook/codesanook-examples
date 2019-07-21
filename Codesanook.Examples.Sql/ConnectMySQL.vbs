' To run the script in PowerShell, use cscript.exe .\ConnectMySQL.vbs
dim connection, recordSet
set connection = CreateObject("ADODB.Connection")
set recordSet = CreateObject("ADODB.Recordset")

' To list all MySQL ODBC drivers installed on your machine,
' Use PowerShell Get-OdbcDriver -Name "MySQL*"
connectionString = "Driver={MySQL ODBC 8.0 Unicode Driver};Server=localhost;" & _
    "Database=thailand-administrative;User=root;Password=change-to-your-password;"

connection.Open connectionString
recordSet.Open "SELECT * FROM provinces limit 0, 10", connection
recordSet.MoveFirst

while not recordSet.EOF
    paddingSpace = string(2 - Len(recordSet.Fields(0)), " ")
    value = paddingSpace & recordSet.Fields(0) & " " &  recordSet.Fields(3)  
    WScript.StdOut.WriteLine value 
    recordSet.MoveNext
wend

connection.Close