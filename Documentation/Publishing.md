# Publishing

The content of the game in its playable form is in `Builds` folder. To build I use this command:

```shell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o "Builds"
```
