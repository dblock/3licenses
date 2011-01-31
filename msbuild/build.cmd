@echo off
setlocal
set BUILD_TARGET=%~1
set BUILD_CONFIGURATION=%~2
:: you can specify your own svn binary directory by setting the BUILD_SVNDIR environment variable
if NOT EXIST "%BUILD_SVNDIR%" (set BUILD_SVNDIR=%ProgramFiles%\Subversion)
if "%BUILD_TARGET%"=="" set BUILD_TARGET=all
if "%BUILD_CONFIGURATION%"=="" set BUILD_CONFIGURATION=Debug
if "%BUILD_CONFIGURATION%"=="debug" set BUILD_CONFIGURATION=Debug
if "%BUILD_CONFIGURATION%"=="release" set BUILD_CONFIGURATION=Release
if "%FrameworkVersion%"=="" set FrameworkVersion=v3.5
if "%FrameworkDir%"=="" set FrameworkDir=%SystemRoot%\Microsoft.NET\Framework
echo Framework: %FrameworkDir%
PATH=%PATH%;%BUILD_SVNDIR%;%FrameworkDir%\%FrameworkVersion%;%ProgramFiles%\Microsoft Visual Studio 8\VC\BIN;%ProgramFiles%\Microsoft Visual Studio 8\Common7\Tools;%ProgramFiles%\NUnit 2.4.1\bin;%ProgramFiles%\doxygen\bin
echo %PATH%
set PYTHON_INSTALL=C:\Python25
echo Using python from %PYTHON_INSTALL%
msbuild.exe LicensesCollector.proj /t:%BUILD_TARGET% /p:Configuration=%BUILD_CONFIGURATION%
endlocal
