FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and project files
COPY ["Poliedro.EDS.sln", "."]
COPY ["Poliedro.Eds.Api/Poliedro.Eds.Api.csproj", "Poliedro.Eds.Api/"]
COPY ["Amazon/Amazon.csproj", "Amazon/"]
COPY ["Poliedro.Eds.Application/Poliedro.Eds.Application.csproj", "Poliedro.Eds.Application/"]
COPY ["Poliedro.Eds.Domain/Poliedro.Eds.Domain.csproj", "Poliedro.Eds.Domain/"]
COPY ["Poliedro.Eds.Infraestructure.External.Plemsi/Poliedro.Eds.Infraestructure.External.Plemsi.csproj", "Poliedro.Eds.Infraestructure.External.Plemsi/"]
COPY ["Poliedro.Eds.Infraestructure.External.Keycloak/Poliedro.Eds.Infraestructure.External.Keycloak.csproj", "Poliedro.Eds.Infraestructure.External.Keycloak/"]
COPY ["Poliedro.Eds.Infraestructure.Persistence.Mysql/Poliedro.Eds.Infraestructure.Persistence.Mysql.csproj", "Poliedro.Eds.Infraestructure.Persistence.Mysql/"]
COPY ["Poliedro.External.HealthCheck/Poliedro.External.HealthCheck.csproj", "Poliedro.External.HealthCheck/"]
COPY ["Poliedro.Tolgee/Poliedro.Tolgee.csproj", "Poliedro.Tolgee/"]
COPY ["WhatsApp/Poliedor.External.WhatsApp.csproj", "WhatsApp/"]
COPY ["WorkerKeycloackService/WorkerKeycloackService.csproj", "WorkerKeycloackService/"]
COPY ["Poliedro.Eds.Api.Tests/Poliedro.Eds.Api.Tests.csproj", "Poliedro.Eds.Api.Tests/"]
COPY ["Poliedro.Eds.Domain.Test/Poliedro.Eds.Domain.Test.csproj", "Poliedro.Eds.Domain.Test/"]

# Restore packages
RUN dotnet restore "Poliedro.EDS.sln"

# Copy source code
COPY . .
WORKDIR "/src/Poliedro.Eds.Api"
RUN dotnet build "Poliedro.Eds.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Poliedro.Eds.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Poliedro.Eds.Api.dll"]