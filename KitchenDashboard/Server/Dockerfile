### Build stage ###
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy solution and project files
COPY KitchenDashboard.sln ./

COPY KitchenDashboard/Client/KitchenDashboard.Client.csproj ./KitchenDashboard/Client/
COPY KitchenDashboard/Server/KitchenDashboard.Server.csproj ./KitchenDashboard/Server/
COPY KitchenDashboard/Shared/KitchenDashboard.Shared.csproj ./KitchenDashboard/Shared/

# restore everything
RUN dotnet restore

# copy the rest of the source
COPY . .

# publish the Server app
WORKDIR /src/KitchenDashboard/Server
RUN dotnet publish -c Release -o /app/publish

### Runtime stage ###
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# bring in the published output
COPY --from=build /app/publish ./

EXPOSE 80
ENTRYPOINT ["dotnet", "KitchenDashboard.Server.dll"]