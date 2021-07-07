$service = Get-WmiObject -Class Win32_Service -Filter "Name='MySQL'"
$service.Delete()