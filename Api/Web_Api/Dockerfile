#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Api/Web_Api/Web_Api.csproj", "Api/Web_Api/"]
COPY ["Library/EventLibrary/EventLibrary.csproj", "Library/EventLibrary/"]
COPY ["RabbitHandlers/CommandHandler/CommandHandler.csproj", "RabbitHandlers/CommandHandler/"]
RUN dotnet restore "Api/Web_Api/Web_Api.csproj"
COPY . .
WORKDIR "/src/Api/Web_Api"
RUN dotnet build "Web_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web_Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web_Api.dll"]