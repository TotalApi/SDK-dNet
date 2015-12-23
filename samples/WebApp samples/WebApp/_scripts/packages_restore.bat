@echo off

set SlnPath=SampleWebApp.sln

set Tools="C:\Program Files\Microsoft Team Foundation Server 12.0\Tools"

%Tools%\nuget.exe restore %SlnPath% -MSBuildVersion 14 -PackagesDirectory ..\..\..\packages
