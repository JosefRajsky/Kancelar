#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/EventStore/EventStore_Service/EventStore_Service.csproj", "Services/EventStore/EventStore_Service/"]
COPY ["RabbitHandlers/CommandHandler/CommandHandler.csproj", "RabbitHandlers/CommandHandler/"]
COPY ["Library/EventLibrary/EventLibrary.csproj", "Library/EventLibrary/"]
RUN dotnet restore "Services/EventStore/EventStore_Service/EventStore_Service.csproj"
COPY . .
WORKDIR "/src/Services/EventStore/EventStore_Service"
RUN dotnet build "EventStore_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventStore_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventStore_Service.dll"]