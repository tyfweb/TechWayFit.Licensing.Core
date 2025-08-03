#!/bin/bash

# Clean previous results
rm -rf ./TestResults

# Restore .NET tools
dotnet tool restore

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage" --results-directory:./TestResults

# Generate HTML report
dotnet reportgenerator \
  -reports:"./TestResults/**/coverage.cobertura.xml" \
  -targetdir:"./TestResults/CoverageReport" \
  -reporttypes:Html

echo "\nCoverage report generated at ./TestResults/CoverageReport/index.html"
