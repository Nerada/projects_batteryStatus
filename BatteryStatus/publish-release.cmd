dotnet publish -c Release -r win-x64 BatteryStatus -p:PublishTrimmed=true
:: dotnet publish -c Release -r win-x64 App -p:Version=0.X.0.0 -p:PublishTrimmed=true
pause