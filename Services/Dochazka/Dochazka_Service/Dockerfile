#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Dochazka/Dochazka_Service/Dochazka_Service.csproj", "Services/Dochazka/Dochazka_Service/"]
RUN dotnet restore "Services/Dochazka/Dochazka_Service/Dochazka_Service.csproj"
COPY . .
WORKDIR "/src/Services/Dochazka/Dochazka_Service"
RUN dotnet build "Dochazka_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dochazka_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dochazka_Service.dll"]