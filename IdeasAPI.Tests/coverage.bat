@echo off

dotnet clean
dotnet build /p:DebugType=Full
dotnet minicover instrument --workdir ../ --assemblies Ideasapi.tests/**/bin/**/*.dll --sources Ideasapi/**/*.cs --exclude-sources Ideasapi/Migrations/**/*.cs --exclude-sources Ideasapi/*.cs --exclude-sources Ideasapi\Domain\IdeasDbContext.cs

dotnet minicover reset --workdir ../

dotnet test --no-build
dotnet minicover uninstrument --workdir ../
dotnet minicover report --workdir ../ --threshold 70