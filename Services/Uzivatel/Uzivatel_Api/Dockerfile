#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Uzivatel/Uzivatel_Api/Uzivatel_Api.csproj", "Services/Uzivatel/Uzivatel_Api/"]
COPY ["RabbitHandlers/CommandHandler/CommandHandler.csproj", "RabbitHandlers/CommandHandler/"]
RUN dotnet restore "Services/Uzivatel/Uzivatel_Api/Uzivatel_Api.csproj"
COPY . .
WORKDIR "/src/Services/Uzivatel/Uzivatel_Api"
RUN dotnet build "Uzivatel_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Uzivatel_Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Uzivatel_Api.dll"]