#!/bin/bash

# Run dotnet publish
dotnet publish

# Define the source and destination paths
SOURCE_PATH="./bin/Release/net8.0/linux-x64/publish/yunfetch"
DESTINATION_PATH="/usr/bin/yunfetch"

# Copy the binary to the destination with the override flag
sudo cp -f $SOURCE_PATH $DESTINATION_PATH

echo "yunfetch has been successfully published and copied to $DESTINATION_PATH"
