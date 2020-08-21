try {
    # Bind the current directory to scripts folder in Docker container 
    $env:HOST_DIRECTORY = Resolve-Path .

    # Save current working directory and change to shared Dock folder
    Push-Location -Path "../Docker"

    # Launch Docker container
    docker-compose up
} finally {
    # Back to home directory of the example
    Pop-Location
}
