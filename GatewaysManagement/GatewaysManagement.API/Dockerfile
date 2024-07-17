#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GatewaysManagement.API/GatewaysManagement.API.csproj", "GatewaysManagement.API/"]
COPY ["GatewaysManagement.Data/GatewaysManagement.Data.csproj", "GatewaysManagement.Data/"]
COPY ["GatewaysManagement.Common/GatewaysManagement.Common.csproj", "GatewaysManagement.Common/"]
COPY ["GatewaysManagement.Services/GatewaysManagement.Services.csproj", "GatewaysManagement.Services/"]
RUN dotnet restore "GatewaysManagement.API/GatewaysManagement.API.csproj"
COPY . .
WORKDIR "/src/GatewaysManagement.API"
RUN dotnet build "GatewaysManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GatewaysManagement.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GatewaysManagement.API.dll"]