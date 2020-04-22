#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Ukol/Ukol_Api/Ukol_Api.csproj", "Services/Ukol/Ukol_Api/"]
RUN dotnet restore "Services/Ukol/Ukol_Api/Ukol_Api.csproj"
COPY . .
WORKDIR "/src/Services/Ukol/Ukol_Api"
RUN dotnet build "Ukol_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ukol_Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ukol_Api.dll"]