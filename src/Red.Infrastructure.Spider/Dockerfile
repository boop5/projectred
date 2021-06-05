#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Red.Infrastructure.Spider/Red.Infrastructure.Spider.csproj", "src/Red.Infrastructure.Spider/"]
RUN dotnet restore "src/Red.Infrastructure.Spider/Red.Infrastructure.Spider.csproj"
COPY . .
WORKDIR "/src/src/Red.Infrastructure.Spider"
RUN dotnet build "Red.Infrastructure.Spider.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Red.Infrastructure.Spider.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Red.Infrastructure.Spider.dll"]