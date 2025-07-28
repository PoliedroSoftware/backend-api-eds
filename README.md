# Introduction
Poliedro EDS Backend API is a modular .NET 8 solution for fuel station management, integrating business logic, domain models, external services, and persistence. It provides RESTful APIs, authentication, inventory, and reporting features.

# Getting Started

## Prerequisites
- .NET 8 SDK
- Node.js (for frontend and tooling)
- MySQL (for persistence)

## Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/PoliedroSoftware/backend-api-eds.git
   ```
2. Restore .NET dependencies:
   ```sh
   dotnet restore Poliedro.EDS.sln
   ```
3. Install Node.js dependencies:
   ```sh
   npm install
   ```

## API Reference
- Main API: `Poliedro.Eds.Api` (REST endpoints)
- Swagger UI available at `/swagger` when running locally

# Build and Test

## Build
```sh
dotnet build Poliedro.EDS.sln
```

## Run
```sh
dotnet run --project Poliedro.Eds.Api/Poliedro.Eds.Api.csproj
```

## Test
```sh
dotnet test Poliedro.Eds.Api.Tests/Poliedro.Eds.Api.Tests.csproj
```

## Pre-push Hook
Before pushing, Husky runs:
- .NET build
- npm test

# Contribute

We use Conventional Commits and Commitlint for commit message validation. To contribute:
1. Fork the repository
2. Create a feature branch
3. Commit using conventional messages (e.g., `feat: add new endpoint`)
4. Push and create a pull request

## Tooling
- Husky: Git hooks for pre-commit and pre-push
- Commitlint: Enforces commit message style
- Commitizen: Helps write conventional commits

# Project Structure
- `Poliedro.Eds.Api`: Main API project
- `Poliedro.Eds.Application`: Business logic and services
- `Poliedro.Eds.Domain`: Domain models
- `Poliedro.Eds.Infraestructure.Persistence.Mysql`: MySQL persistence
- `Amazon`: AWS integration (S3, SecretsManager, etc.)
- `Poliedro.Eds.Infraestructure.External.*`: External service connectors
- `Poliedro.Eds.Api.Tests`, `Poliedro.Eds.Domain.Test`: Unit tests

# Main Libraries
- ASP.NET Core
- Entity Framework Core (MySQL)
- AWS SDK
- FluentValidation
- MediatR
- AutoMapper
- Husky, Commitlint, Commitizen (tooling)

For more inspiration, see:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)