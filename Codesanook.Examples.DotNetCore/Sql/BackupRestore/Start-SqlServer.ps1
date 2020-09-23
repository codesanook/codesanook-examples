try {
    # Bind the current directory to scripts folder in Docker container 
    $env:HOST_DIRECTORY = Resolve-Path .
    $env:DOCKER_FILE = "Dockerfile"

    # Save current working directory and change to shared Dock folder
    Push-Location -Path "../SqlServerDocker"

    # Launch Docker container
    docker-compose up --build 
} finally {
    # Back to home directory of the example
    # Only one CTRL+C to kill the process and then it will remove container automatically 
    docker-compose down --rmi 'all' --volumes
    Pop-Location
}
