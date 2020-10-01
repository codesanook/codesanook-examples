$ErrorActionPreference = "stop"
& .\node-batch.cmd
# & node standard-error.js 2>&1 | error.txt
# & node standard-error.js 2>&1 | Out-String 
# $a = (& node standard-error.js 2>&1)

"next operation"
$LASTEXITCODE
"next operation"
