@echo off
pushd ".."
setlocal EnableDelayedExpansion

set NUNIT="packages\NUnit.Console.3.0.1\tools\nunit3-console.exe"
set OPENCOVER="packages\OpenCover.4.6.166\tools\OpenCover.Console.exe"
set REPORTGEN="packages\ReportGenerator.2.4.0.0\tools\ReportGenerator.exe"
set REPORTS=bin\Reports
set COVERAGE="%REPORTS%\Coverage.xml"

if not exist %REPORTS% md %REPORTS% 

for %%P in ("."; "Net40"; "Net45"; "PCL-Net45"; "Extensions"; "Net40\Extensions") do (
	for %%T in ("%%P\bin\Release\*Tests.dll") do (
		set TESTLIBS=!TESTLIBS! %%T
))

echo:
echo:Running tests with coverage. Results are collected in %COVERAGE% . . .
echo:
echo:from assemblies: %TESTLIBS%
echo: 

%OPENCOVER%^
 -register:user^
 -target:%NUNIT%^
 -targetargs:"%TESTLIBS%"^
 -filter:"+[*]* -[*Test*]* -[Microsoft*]* -[xunit*]* -[NetCore*]*"^
 -excludebyattribute:*.ExcludeFromCodeCoverageAttribute^
 -hideskipped:all^
 -output:%COVERAGE%

echo:
echo:Generating HTML coverage report in "%REPORTS%" . . .
echo: 

%REPORTGEN%^
 -reports:%COVERAGE%^
 -targetdir:%REPORTS%^
 -reporttypes:Html;HtmlSummary;Badges^
 -assemblyfilters:-*Test*^
 -classfilters:-DryIoc.Arg

rem start %REPORTS%\index.htm

echo:
echo:Succeeded.
endlocal
popd

if not "%1"=="-nopause" pause