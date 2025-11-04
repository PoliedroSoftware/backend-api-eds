# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WorkerS3UploaderService/WorkerS3UploaderService.csproj", "WorkerS3UploaderService/"]
COPY ["Poliedro.Eds.Application/Poliedro.Eds.Application.csproj", "Poliedro.Eds.Application/"]
COPY ["Poliedro.Eds.Domain/Poliedro.Eds.Domain.csproj", "Poliedro.Eds.Domain/"]
RUN dotnet restore "./WorkerS3UploaderService/WorkerS3UploaderService.csproj"
COPY . .
WORKDIR "/src/WorkerS3UploaderService"
RUN dotnet build "./WorkerS3UploaderService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WorkerS3UploaderService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copiar el appsettings.json específico del WorkerService
COPY ["WorkerS3UploaderService/appsettingsS3.json", "./appsettings.json"]
ENTRYPOINT ["dotnet", "WorkerS3UploaderService.dll"]