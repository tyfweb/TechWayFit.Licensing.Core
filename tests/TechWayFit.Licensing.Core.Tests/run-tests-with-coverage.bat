@echo off

:: Clean previous results
if exist .\TestResults rmdir /s /q .\TestResults

:: Restore .NET tools
dotnet tool restore

:: Run tests with coverage
dotnet test ^
  --settings Tests.runsettings ^
  --collect:"XPlat Code Coverage" ^
  --results-directory:.\TestResults

:: Generate HTML report
dotnet reportgenerator -reports:".\TestResults\**\coverage.cobertura.xml" -targetdir:".\TestResults\CoverageReport" -reporttypes:Html

echo.
echo Coverage report generated at .\TestResults\CoverageReport\index.html
