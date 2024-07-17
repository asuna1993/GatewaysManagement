#FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
#WORKDIR /src
#COPY . .
#WORKDIR /src/GatewaysManagement
#RUN dotnet restore /ignoreprojectextensions:.dcproj
#WORKDIR /src/GatewaysManagement/GatewaysManagement.API
#RUN dotnet publish GatewaysManagement.API.csproj -c Release -o /app
#
#FROM mcr.microsoft.com/dotnet/aspnet:3.1
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#COPY --from=build /app .
#ENTRYPOINT ["dotnet", "GatewaysManagement.API.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS publish
WORKDIR /src
COPY . .
RUN dotnet restore /ignoreprojectextensions:.dcproj
WORKDIR /src/GatewaysManagement.API
RUN dotnet publish GatewaysManagement.API.csproj -c Release -o /app
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GatewaysManagement.API.dll"]