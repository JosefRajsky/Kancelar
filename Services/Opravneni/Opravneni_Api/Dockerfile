#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Opravneni/Opravneni_Api/Opravneni_Api.csproj", "Services/Opravneni/Opravneni_Api/"]
COPY ["RabbitHandlers/CommandHandler/CommandHandler.csproj", "RabbitHandlers/CommandHandler/"]
RUN dotnet restore "Services/Opravneni/Opravneni_Api/Opravneni_Api.csproj"
COPY . .
WORKDIR "/src/Services/Opravneni/Opravneni_Api"
RUN dotnet build "Opravneni_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Opravneni_Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Opravneni_Api.dll"]