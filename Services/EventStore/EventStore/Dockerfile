#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/EventStore/EventStore/EventStore.csproj", "Services/EventStore/EventStore/"]
COPY ["RabbitHandlers/CommandHandler/CommandHandler.csproj", "RabbitHandlers/CommandHandler/"]
RUN dotnet restore "Services/EventStore/EventStore/EventStore.csproj"
COPY . .
WORKDIR "/src/Services/EventStore/EventStore"
RUN dotnet build "EventStore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventStore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventStore.dll"]