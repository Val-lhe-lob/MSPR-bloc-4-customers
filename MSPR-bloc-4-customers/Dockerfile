# Utilise l'image officielle .NET 8 SDK pour la phase de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MSPR-bloc-4-customers.csproj", "."]
RUN dotnet restore "MSPR-bloc-4-customers.csproj"
COPY . .
RUN dotnet publish "MSPR-bloc-4-customers.csproj" -c Release -o /app/publish

# Utilise l'image ASP.NET Core Runtime pour l'exécution
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MSPR-bloc-4-customers.dll"]
