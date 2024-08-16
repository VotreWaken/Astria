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
