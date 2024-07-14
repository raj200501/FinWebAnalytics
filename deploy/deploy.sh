#!/bin/bash

# Build the project
dotnet build

# Run the tests
dotnet test

# Deploy the project (this script can be extended based on the specific deployment requirements)
echo "Deploying FinWebAnalytics application..."
