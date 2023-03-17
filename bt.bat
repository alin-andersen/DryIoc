@echo off
setlocal EnableDelayedExpansion

echo:
echo:# Build and run TestRunner (.NET 6 only)
echo:

dotnet build -c Release test/DryIoc.TestRunner/DryIoc.TestRunner.csproj
if %ERRORLEVEL% neq 0 goto :error

dotnet run --no-build -c Release --project test/DryIoc.TestRunner/DryIoc.TestRunner.csproj
if %ERRORLEVEL% neq 0 goto :error

echo:
echo:## ALL Successful ##
exit /b 0

:error
echo:
echo:## Build is failed :-(
exit /b 1
