FROM microsoft/aspnetcore:1.1

WORKDIR /app
COPY src/DlnaCore.Server/publish/ /app

ENTRYPOINT ["dotnet", "DlnaCore.dll"]
