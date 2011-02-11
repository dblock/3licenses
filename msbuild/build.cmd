@echo off
setlocal
set BUILD_TARGET=%~1
set BUILD_CONFIGURATION=%~2
if "%BUILD_TARGET%"=="" set BUILD_TARGET=build
if "%BUILD_CONFIGURATION%"=="" set BUILD_CONFIGURATION=Release
if "%BUILD_CONFIGURATION%"=="debug" set BUILD_CONFIGURATION=Debug
if "%BUILD_CONFIGURATION%"=="release" set BUILD_CONFIGURATION=Release
if "%FrameworkVersion%"=="" set FrameworkVersion=v3.5
if "%FrameworkDir%"=="" set FrameworkDir=%SystemRoot%\Microsoft.NET\Framework
echo Framework: %FrameworkDir%
echo Framework version: %FrameworkVersion%
%FrameworkDir%\%FrameworkVersion%\msbuild.exe LicensesCollector.sln /t:%BUILD_TARGET% /p:Configuration=%BUILD_CONFIGURATION%
endlocal
