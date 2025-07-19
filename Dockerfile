FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Poliedro.Eds.Api/Poliedro.Eds.Api.csproj", "Poliedro.Eds.Api/"]
RUN dotnet restore "./Poliedro.Eds.Api/Poliedro.Eds.Api.csproj"
COPY . .
WORKDIR "/src/Poliedro.Eds.Api"
RUN dotnet build "./Poliedro.Eds.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Poliedro.Eds.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Poliedro.Eds.Api.dll"]