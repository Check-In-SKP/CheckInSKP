cd ../src/CheckInAPI

dotnet restore
dotnet build --no-restore
dotnet publish -o ../../deploy
