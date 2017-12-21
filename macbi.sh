#!/bin/bash
# This is the mac build and install shell script.
#
# Notes:
#	MSBuild on OSX screws with the output file permissions
#	this script makes sure MSBuild runs as root to avoid problems with ILRepack
#	

sudo msbuild dmake.sln /p:Configuration=Release
sudo cp dmake/bin/Release/dmake.exe /usr/bin/dmake.exe
chmod 775 monolaunch.sh
sudo cp monolaunch.sh /usr/bin/dmake

echo dmake installed.
