ECHO running
@IF "%SCM_TRACE_LEVEL%" NEQ "4" @ECHO off
:: ----------------------
:: KUDU Deployment Script
:: Version: 1.0.17
:: ----------------------

:: Prerequisites
:: -------------

:: Verify node.js installed

ECHO "Verifying Node.js"
where node 2>nul >nul
IF %ERRORLEVEL% NEQ 0 (
  ECHO Missing node.js executable, please install node.js, if already installed make sure it can be reached from current environment.
  GOTO error
)

ECHO "done Verifying Node.js"

:: Setup
:: -----

SETLOCAL ENABLEDELAYEDEXPANSION

::https://stackoverflow.com/a/10290765/1872200
:: %~dp0% refer to the current executed batch path
:: Set artifacts folder
SET ARTIFACTS=%~dp0%..\artifacts

:: Set deployment source folder
IF NOT DEFINED DEPLOYMENT_SOURCE (
  SET DEPLOYMENT_SOURCE=%~dp0%.
)

:: Set deployment source folder
IF NOT DEFINED DEPLOYMENT_TARGET (
  SET DEPLOYMENT_TARGET=%ARTIFACTS%\wwwroot
)

IF NOT DEFINED NEXT_MANIFEST_PATH (
  SET NEXT_MANIFEST_PATH=%ARTIFACTS%\manifest

  IF NOT DEFINED PREVIOUS_MANIFEST_PATH (
    SET PREVIOUS_MANIFEST_PATH=%ARTIFACTS%\manifest
  )
)

IF NOT DEFINED KUDU_SYNC_CMD (
  :: Install Kudu sync
  ECHO Installing Kudu Sync
  CALL npm install kudusync -g --silent
  IF !ERRORLEVEL! NEQ 0 GOTO error

  :: Locally just running "kuduSync" would also work
  SET KUDU_SYNC_CMD=%appdata%\npm\kuduSync.cmd
)

IF NOT DEFINED DEPLOYMENT_TEMP (
  SET DEPLOYMENT_TEMP=%temp%\___deployTemp%random%
  SET CLEAN_LOCAL_DEPLOYMENT_TEMP=true
)

IF DEFINED CLEAN_LOCAL_DEPLOYMENT_TEMP (
  IF EXIST "%DEPLOYMENT_TEMP%" rd /s /q "%DEPLOYMENT_TEMP%"
  MKDIR "%DEPLOYMENT_TEMP%"
)

:: Always set MSBUILD_PATH
SET MSBUILD_PATH=%ProgramFiles(x86)%\MSBuild-15.3.409.57025\MSBuild\15.0\Bin\MSBuild.exe

CALL :ExecuteCmd "%MSBUILD_PATH%" -version

GOTO Deployment

:: Utility Functions
:: -----------------
:SelectNodeVersion

IF DEFINED KUDU_SELECT_NODE_VERSION_CMD (
    :: The following are done only on Windows Azure Websites environment
    CALL %KUDU_SELECT_NODE_VERSION_CMD% "%DEPLOYMENT_SOURCE%" "%DEPLOYMENT_TARGET%" "%DEPLOYMENT_TEMP%"
    IF !ERRORLEVEL! NEQ 0 GOTO error

    IF EXIST "%DEPLOYMENT_TEMP%\__nodeVersion.tmp" (
		:: Set NODE_EXE from the first line in a file
        SET /p NODE_EXE=<"%DEPLOYMENT_TEMP%\__nodeVersion.tmp"
        IF !ERRORLEVEL! NEQ 0 GOTO error
    )
    
    IF EXIST "%DEPLOYMENT_TEMP%\__npmVersion.tmp" (
        SET /p NPM_JS_PATH=<"%DEPLOYMENT_TEMP%\__npmVersion.tmp"
        IF !ERRORLEVEL! NEQ 0 GOTO error
    )

    IF NOT DEFINED NODE_EXE (
        SET NODE_EXE=node
    )

	:: Set NPM_CMD
    SET NPM_CMD="!NODE_EXE!" "!NPM_JS_PATH!"

) ELSE (
    SET NPM_CMD=npm
    SET NODE_EXE=node
)

ECHO Finished setting NODE and NPM
GOTO :EOF

:Deployment
::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Deployment
:: ----------
ECHO Handling .NET Web Application deployment.

:: 1. Restore NuGet packages
CALL :ExecuteCmd nuget restore "%DEPLOYMENT_SOURCE%\CodeSanook.Examples.CSharp.sln"
IF !ERRORLEVEL! NEQ 0 GOTO error



::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
GOTO end

:: Execute command routine that will echo out when error
:ExecuteCmd
SETLOCAL
:: set all parameters to _CMD_
SET _CMD_=%*
CALL %_CMD_%
IF "%ERRORLEVEL%" NEQ "0" ECHO Failed exitCode=%ERRORLEVEL%, command=%_CMD_%
EXIT /b %ERRORLEVEL%

:error
ENDLOCAL

ECHO An error has occurred during web site deployment.
CALL :exitSetErrorLevel
CALL :exitFromFunction 2>nul

:exitSetErrorLevel
::exit batch file with set error code to 1
EXIT /b 1

:exitFromFunction
()

:end
ENDLOCAL
ECHO Finished successfully.
