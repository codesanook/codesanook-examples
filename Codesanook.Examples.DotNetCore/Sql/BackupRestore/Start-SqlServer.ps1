$env:HOST_DIRECTORY = Resolve-Path .

# Change directory
Set-Location -Path "../Docker"
docker-compose up 
