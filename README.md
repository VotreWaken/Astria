# Astria
.NET 8 API project including Clean Architecture principles, DDD, CQRS, Event Sourcing, MediatR

## Features

- ASP.NET Core 8.0 Web API application
- Clean Architecture implementation with applying SOLID principles
- Domain-driven Design (DDD)
- CQRS implementation on Commands, Queries and Projections
- MediatR implementation (Request- and Notification-Handling, Pipeline-Behaviour for Logging, Metrics and Authentication)
- Swagger Open API endpoint
- Dockerfile and Docker Compose (YAML) file for environmental setup
- MongoDB Repository for performance-optimized querying
- Authentication Service based on ASP.NET Core Idenity
- Email SMTP Service for account verification
- Shared Kernel class library implementation for DDD & Event Sourcing
- EventStoreDB Repository and custom Client for storing of events
- nUnit-based Test-driven Design (TDD) with Integration Testing
- Cross-Cutting Concern

## Technologies

- .NET 8
- Docker
- MediatR
- EventStoreDB
- MongoDB
- ASP.NET Core Indentity
- EF Core
- xUnit
- Moq
- FluentAssertions
- Swashbuckle
- Serilog

## Running the Project

To run the project, follow these steps:

1. Clone the repository to your local machine.
2. Open the solution in your IDE of choice.
3. Build the solution to restore the dependencies.
4. Update the connection string in the appsettings.json file to point to your database.
5. Start the API project
6. The database migrations will be automatically applied on start-up. If the database does not exist, it will be created.
7. The API should be accessible at https://localhost:<port>/api/<controller> where <port> is the port number specified in the project properties and <controller> is the name of the API controller.

## Continuous Integration
This project uses GitHub Actions to build and test the project on every commit to the main branch. The workflow consists of several steps, including restoring packages, building the project and running tests.
| CI | win-x64 | linux-x64 | osx-x64 |
|-|-|-|-|
| <sup>Authentication Service</sup> | ![Win Workflow](https://github.com/VotreWaken/Astria/actions/workflows/dotnet.yml/badge.svg) | | |


