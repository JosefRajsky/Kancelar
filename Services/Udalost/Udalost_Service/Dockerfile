#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Udalost/Udalost_Service/Udalost_Service.csproj", "Services/Udalost/Udalost_Service/"]
RUN dotnet restore "Services/Udalost/Udalost_Service/Udalost_Service.csproj"
COPY . .
WORKDIR "/src/Services/Udalost/Udalost_Service"
RUN dotnet build "Udalost_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Udalost_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Udalost_Service.dll"]