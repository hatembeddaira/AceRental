# Consultez https://aka.ms/customizecontainer pour savoir comment personnaliser votre conteneur de débogage et comment Visual Studio utilise ce Dockerfile pour générer vos images afin d’accélérer le débogage.

# Cet index est utilisé lors de l’exécution à partir de VS en mode rapide (par défaut pour la configuration de débogage)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80


# Cette phase est utilisée pour générer le projet de service
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
RUN apt-get update && apt-get upgrade -y
ARG BUILD_CONFIGURATION=Release

RUN curl -sL https://deb.nodesource.com/setup_20.x | bash -
RUN apt-get install nodejs -y
RUN npm install -g @angular/cli@21.1.4

WORKDIR /src
COPY ./ClientApp/package.json ./ClientApp/
COPY ./ClientApp/package-lock.json ./ClientApp/

COPY ["AceRental.FrontEnd.csproj", "."]
RUN dotnet restore "./AceRental.FrontEnd.csproj"
COPY . .

# Build Angular app first
WORKDIR "/src/ClientApp"
RUN npm install
RUN npm run build -- --configuration development || npm run build

WORKDIR "/src"
RUN dotnet build "./AceRental.FrontEnd.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Cette étape permet de publier le projet de service à copier dans la phase finale
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AceRental.FrontEnd.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Copy compiled Angular app to wwwroot
RUN cp -r /src/ClientApp/dist/* /app/publish/wwwroot/ || true

# Cette phase est utilisée en production ou lors de l’exécution à partir de VS en mode normal (par défaut quand la configuration de débogage n’est pas utilisée)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AceRental.FrontEnd.dll"]