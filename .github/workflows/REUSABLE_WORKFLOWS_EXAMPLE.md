# Example: How to Use Reusable Workflows

This document demonstrates how to use the optimized reusable workflows in other repositories.

## Basic .NET Build and Test

```yaml
name: CI

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    uses: PoliedroSoftware/backend-api-eds/.github/workflows/reusable-dotnet-build.yml@main
    with:
      dotnet-version: '8.0.x'
      configuration: 'Release'
      solution-path: 'MySolution.sln'
      run-tests: true
```

## Complete CI/CD Pipeline

```yaml
name: Complete CI/CD

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  # Build and test
  build:
    uses: PoliedroSoftware/backend-api-eds/.github/workflows/reusable-dotnet-build.yml@main
    with:
      dotnet-version: '8.0.x'
      run-tests: true

  # Code quality analysis
  sonar:
    needs: build
    uses: PoliedroSoftware/backend-api-eds/.github/workflows/reusable-sonar.yml@main
    with:
      project-key: 'my-project-key'
      organization: 'my-organization'
    secrets:
      SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}

  # Load testing
  load-test:
    needs: build
    uses: PoliedroSoftware/backend-api-eds/.github/workflows/reusable-jmeter.yml@main
    with:
      test-plan-path: 'tests/load-test.jmx'
      generate-html-report: true

  # Build and push Docker image
  docker:
    if: github.ref == 'refs/heads/main'
    needs: [build, sonar]
    uses: PoliedroSoftware/backend-api-eds/.github/workflows/reusable-docker-build.yml@main
    with:
      image-name: 'my-api'
      registry-type: 'dockerhub'
    secrets:
      DOCKERHUB_USERNAME: ${{ secrets.DOCKERHUB_USERNAME }}
      DOCKERHUB_TOKEN: ${{ secrets.DOCKERHUB_TOKEN }}

  # Deploy to AWS
  deploy:
    if: github.ref == 'refs/heads/main'
    needs: docker
    uses: PoliedroSoftware/backend-api-eds/.github/workflows/reusable-aws-deploy.yml@main
    with:
      environment: 'production'
      aws-region: 'us-east-1'
    secrets:
      AWS_ACCESS_KEY_ID: ${{ secrets.AWS_ACCESS_KEY_ID }}
      AWS_SECRET_ACCESS_KEY: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
      AWS_ECS_CLUSTER: ${{ secrets.AWS_ECS_CLUSTER }}
      AWS_ECS_SERVICE: ${{ secrets.AWS_ECS_SERVICE }}
      # TASK_DEFINITION_ARN is optional - if not provided, will use latest from service
      TASK_DEFINITION_ARN: ${{ secrets.TASK_DEFINITION_ARN }}
```

## Matrix Strategy for Multiple Environments

```yaml
name: Multi-Environment Deploy

on:
  push:
    branches: [ main ]

jobs:
  build:
    uses: PoliedroSoftware/backend-api-eds/.github/workflows/reusable-dotnet-build.yml@main
    with:
      dotnet-version: '8.0.x'

  deploy:
    needs: build
    strategy:
      matrix:
        environment: [dev, staging, prod]
    uses: PoliedroSoftware/backend-api-eds/.github/workflows/reusable-aws-deploy.yml@main
    with:
      environment: ${{ matrix.environment }}
    secrets:
      AWS_ACCESS_KEY_ID: ${{ secrets.AWS_ACCESS_KEY_ID }}
      AWS_SECRET_ACCESS_KEY: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
      AWS_ECS_CLUSTER: ${{ secrets[format('AWS_ECS_CLUSTER_{0}', upper(matrix.environment))] }}
      AWS_ECS_SERVICE: ${{ secrets[format('AWS_ECS_SERVICE_{0}', upper(matrix.environment))] }}
      # TASK_DEFINITION_ARN is optional - if not provided, will use latest from service
      TASK_DEFINITION_ARN: ${{ secrets[format('TASK_DEFINITION_ARN_{0}', upper(matrix.environment))] }}
```