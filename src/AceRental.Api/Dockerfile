# Étape 1 : Image de build (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base
# WORKDIR /src
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copier les fichiers .csproj et restaurer les dépendances
COPY ["AceRental.Api/AceRental.Api.csproj", "AceRental.Api/"]
COPY ["AceRental.Domain/AceRental.Domain.csproj", "AceRental.Domain/"]
COPY ["AceRental.Infrastructure/AceRental.Infrastructure.csproj", "AceRental.Infrastructure/"]
COPY ["AceRental.Application/AceRental.Application.csproj", "AceRental.Application/"]
RUN dotnet restore "AceRental.Api/AceRental.Api.csproj"

# Copier tout le reste et builder
COPY . .
WORKDIR "/src/AceRental.Api"
RUN dotnet build "AceRental.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Étape 2 : Publication
FROM build AS publish
RUN dotnet publish "AceRental.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Étape 3 : Image finale (Runtime - plus légère)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AceRental.Api.dll"]