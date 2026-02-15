# Agents Guide for ImperaPlus Backend

## Project Overview

ImperaPlus is an online multiplayer strategy game backend (Risk/Conquest-style) built with ASP.NET Core 6.0 and C# 10. It provides a REST API, real-time communication via SignalR, background job processing via Hangfire, and uses SQL Server for persistence. The architecture follows a layered/clean architecture approach with Domain-Driven Design influences.

## Build, Test, and Lint

### Prerequisites

- .NET 6.0 SDK (see `global.json`; `rollForward: latestMajor` allows newer SDKs)
- SQL Server (for full integration; in-memory provider used for tests)
- Docker (optional, for containerized development)

### Build

```bash
dotnet build ImperaPlus.sln
```

### Test

```bash
# Run all tests
dotnet test ImperaPlus.sln

# Run specific test projects
dotnet test ImperaPlus.Domain.Tests/ImperaPlus.Domain.Tests.csproj
dotnet test ImperaPlus.Application.Tests/ImperaPlus.Application.Tests.csproj
dotnet test ImperaPlus.IntegrationTests/ImperaPlus.IntegrationTests.csproj
```

### Lint / Code Style

Code style is enforced via `.editorconfig` and Roslyn analyzers at build time. There is no separate lint command — style violations surface as build warnings and errors during `dotnet build`.

### Run Locally

```bash
dotnet run --project ImperaPlus.Web
```

Or with Docker:

```bash
docker-compose up
```

## Database Migrations

This project uses Entity Framework Core with SQL Server. The `dotnet-ef` CLI tool (version 6.0.1) is configured in `.config/dotnet-tools.json`. **Any change to domain entities that adds, removes, or modifies persisted properties requires a new migration.**

### Setup

```bash
# Restore the EF CLI tool (first time or after clean)
dotnet tool restore
```

### Creating a Migration

When you add or change a persisted property on a domain entity (in `ImperaPlus.Domain`), generate a migration:

```bash
dotnet ef migrations add <MigrationName> --project ImperaPlus.Web --context ImperaContext
```

- `--project ImperaPlus.Web` — migrations live in `ImperaPlus.Web/Migrations/`
- `--context ImperaContext` — the EF Core DbContext defined in `ImperaPlus.DataAccess`
- `<MigrationName>` — use a descriptive PascalCase name (e.g., `AddTournamentPassword`, `RemoveUserID1`)

This generates three file changes:
1. `ImperaPlus.Web/Migrations/<timestamp>_<MigrationName>.cs` — the `Up()` and `Down()` migration methods
2. `ImperaPlus.Web/Migrations/<timestamp>_<MigrationName>.Designer.cs` — model snapshot for this migration
3. `ImperaPlus.Web/Migrations/ImperaContextModelSnapshot.cs` — updated cumulative model snapshot

### Removing the Last Migration

```bash
dotnet ef migrations remove --project ImperaPlus.Web --context ImperaContext
```

### Important Notes

- Always review the generated migration to verify it only contains the intended schema changes.
- Properties marked with `[NotMapped]` or computed properties do **not** require migrations.
- The `ImperaContextModelSnapshot.cs` is auto-generated — do not edit it manually.
- Migrations are applied automatically at application startup.

## Updating API Contracts (NSwag Client Generation)

This project uses [NSwag](https://github.com/RicoSuter/NSwag) to generate API client code from the OpenAPI/Swagger specification. Two clients are generated:

- **C# client** → `ImperaPlus.GeneratedClient/ImperaClients.cs` (configured by `clientGenerationSettings dotnet.nswag`)
- **TypeScript client** → `ImperaPlus.GeneratedClient.TypeScript/imperaClients.ts` (configured by `clientGenerationSettings typescript.nswag`)

The `.nswag` configuration files contain the embedded OpenAPI spec and all generation settings. **Any change to controller actions, route parameters, or DTO shapes requires regenerating the clients.**

### Setup

```bash
# Restore the NSwag CLI tool (first time or after clean)
dotnet tool restore
```

### Regenerating Clients

The clients are generated using `nswag run` with the existing `.nswag` configuration files. Each `.nswag` file contains an embedded copy of the OpenAPI spec and all code generation settings.

#### 1. Start the API server

The `.nswag` files are configured to fetch the OpenAPI spec from the running server at `http://localhost:57676/swagger/v1/swagger.json`. Start the server first:

```bash
dotnet run --project ImperaPlus.Web --launch-profile "IIS Express"
```

Or update the `url` field in the `.nswag` files to match your local server address (e.g., `http://localhost:5000/swagger/v1/swagger.json`).

#### 2. Regenerate the C# client

```bash
dotnet nswag run "clientGenerationSettings dotnet.nswag"
```

This reads settings from the `.nswag` file, fetches the OpenAPI spec from the running server (or uses the embedded `json` field as fallback), and writes to `ImperaPlus.GeneratedClient/ImperaClients.cs`.

#### 3. Regenerate the TypeScript client

```bash
dotnet nswag run "clientGenerationSettings typescript.nswag"
```

This writes to `ImperaPlus.GeneratedClient.TypeScript/imperaClients.ts`.

### Configuration Details

The `.nswag` files contain:
- **`swaggerGenerator.fromSwagger.json`** — an embedded copy of the OpenAPI spec (used as fallback if the server URL is unreachable)
- **`swaggerGenerator.fromSwagger.url`** — the URL of the running server's Swagger endpoint
- **`codeGenerators.swaggerToCSharpClient`** / **`swaggerToTypeScriptClient`** — all code generation settings (client base class, namespaces, type mappings, etc.)

To update the embedded OpenAPI spec without running the server, you can use `aspnetcore2openapi`:

```bash
dotnet nswag aspnetcore2openapi /project:ImperaPlus.Web/ImperaPlus.Web.csproj /output:swagger.json
```

Then replace the `json` field value in both `.nswag` files with the contents of `swagger.json` and delete `swagger.json` (it is not checked into the repository).

### Important Notes

- The NSwag CLI version (`nswag.consolecore` in `.config/dotnet-tools.json`) must match the `NSwag.AspNetCore` package version in `ImperaPlus.Web.csproj` (currently 13.15.5). A version mismatch causes runtime failures.
- After regenerating clients, always build the solution (`dotnet build ImperaPlus.sln`) and run integration tests to verify there are no breaking changes.
- If generated client method signatures change (e.g., new required parameters), update callers in `ImperaPlus.IntegrationTests` and any other projects that reference `ImperaPlus.GeneratedClient`.
- The generated files (`ImperaClients.cs`, `imperaClients.ts`) are auto-generated — do not edit them manually.

## Project Structure

```
ImperaPlus.sln
├── ImperaPlus.Web/              # ASP.NET Core host, controllers, SignalR hubs, Hangfire config, EF migrations
├── ImperaPlus.Application/      # Application services, DTOs mapping, background jobs, notifications
├── ImperaPlus.Domain/           # Domain entities, domain services, repository interfaces, domain events
├── ImperaPlus.DataAccess/       # EF Core DbContext, repository implementations (SQL Server)
├── ImperaPlus.DataAccess.InMemory/  # In-memory data access for testing
├── ImperaPlus.DataAccess.ConvertedMaps/  # Embedded map data resources
├── ImperaPlus.DTO/              # Data transfer objects shared between layers
├── ImperaPlus.Utils/            # Shared utility code
├── ImperaPlus.GeneratedClient/  # Auto-generated .NET API client (NSwag)
├── ImperaPlus.Domain.Tests/     # Domain layer unit tests
├── ImperaPlus.Application.Tests/ # Application layer tests
├── ImperaPlus.IntegrationTests/ # End-to-end API tests with TestServer
├── ImperaPlus.TestSupport/      # Shared test infrastructure (helpers, fakes, test data factories)
├── MapConverter/                # Tool for converting map data
└── ImperaPlus.GeneratedClient.TypeScript/  # TypeScript client generation config
```

### Layer Dependencies

```
Web → Application → Domain ← DataAccess
                      ↑
                     DTO
```

- **Domain** is the core with no upstream dependencies. It defines entities, domain services, repository interfaces, and domain events.
- **Application** depends on Domain. It contains use-case orchestration, DTO mapping (AutoMapper), background jobs (Hangfire), and notification handling.
- **Web** depends on Application. It hosts REST controllers, SignalR hubs, middleware, authentication (OpenIddict), and Hangfire dashboard.
- **DataAccess** implements the repository interfaces from Domain using EF Core against SQL Server.

## Architecture and Patterns

### Dependency Injection

Autofac is the IoC container. Each layer has a `DependencyInjectionModule.cs` that registers its services. Modules are composed in `Startup.cs`.

### Repository and Unit of Work

`IUnitOfWork` (defined in Domain) exposes typed repository properties (`IGameRepository`, `IUserRepository`, `ILadderRepository`, `ITournamentRepository`, etc.) and a `Commit()` method. The EF Core implementation lives in `ImperaPlus.DataAccess`.

### Domain Events

An `EventAggregator` dispatches domain events (e.g., `AccountDeleted`). Event handlers implement `IEventHandler<T>` and are registered in the DI modules.

### Background Jobs

Hangfire processes recurring and enqueued jobs. Key recurring jobs include:
- `TimeoutJob` — every 2 minutes (game timeout detection)
- `LadderJob` — every 4 minutes (ladder synchronization)
- `TournamentStartJob` — every hour
- `TournamentJob` — every 5 minutes
- `UserCleanupJob` — daily
- `GameCleanupJob` — hourly

Job classes are in `ImperaPlus.Application/Jobs/` and inherit from a base `Job` class.

### Real-Time Communication

Two SignalR hubs:
- `GameHub` at `/signalr/game` — game notifications, in-game chat, join/leave game groups
- `MessagingHub` at `/signalr/chat` — global chat channels, online user tracking

### Authentication

OpenIddict 3.1.1 provides OAuth 2.0 / OpenID Connect. ASP.NET Core Identity manages users and roles. Tokens are issued via `AccountController`.

## Code Style and Conventions

All rules are defined in `.editorconfig`. Key conventions:

- **Indentation**: 4 spaces for C# files, 2 spaces for XML/JSON
- **`var` usage**: Required everywhere (enforced as error)
- **Braces**: Allman style (opening brace on new line). Required for multi-line blocks.
- **Naming**:
  - Public/protected members: `PascalCase`
  - Private instance fields: `_camelCase`
  - Private static fields: `s_camelCase`
  - Constants: `PascalCase`
  - Locals and parameters: `camelCase`
  - Local functions: `PascalCase`
- **`this.` qualifier**: Avoided (enforced as error)
- **Language keywords**: Required over framework type names (e.g., `int` not `Int32`)
- **Pattern matching**: Preferred over `is`/`as` with null checks (enforced as error)
- **Expression bodies**: Required for properties, indexers, accessors; block bodies for methods, constructors, operators
- **Using directives**: `System.*` first, no blank lines between groups
- **Modifiers**: Ordered per IDE0036; accessibility modifiers required
- **Diagnostics**: IDE and CA analyzer warnings enabled (unused members, parameters, assignments)

## Testing

### Framework

- **MSTest** (Microsoft.VisualStudio.TestTools.UnitTesting) with `[TestClass]` / `[TestMethod]`
- **Moq** for mocking
- **Microsoft.AspNetCore.Mvc.Testing** for integration tests

### Test Layers

1. **Domain Tests** (`ImperaPlus.Domain.Tests`): Pure unit tests on domain entities and services. Use `TestUtils` for creating mock domain objects. Custom `ExpectedDomainExceptionAttribute` verifies domain exceptions with specific error codes.

2. **Application Tests** (`ImperaPlus.Application.Tests`): Tests inherit from `TestBase` which sets up an Autofac container with in-memory EF Core database. Each test gets a fresh DI scope with access to `UnitOfWork`, `Context`, and `TestData`.

3. **Integration Tests** (`ImperaPlus.IntegrationTests`): Use `TestServer` (ASP.NET Core test host) with full middleware pipeline. Assembly-level setup initializes the server once. Tests use `ApiClient` for HTTP requests.

### Test Support Library

`ImperaPlus.TestSupport` provides shared infrastructure:
- `TestBase` — base class for application-level tests with DI setup
- `TestData` — factory for creating and persisting test entities (users, games, map templates)
- `TestUtils` — static helpers for creating mock domain objects
- `FakeEmailService` — no-op email implementation
- `SynchronousBackgroundJobClient` — executes Hangfire jobs synchronously
- `TestUserProvider` — supplies a mock authenticated user
- `PredefinedRandomGen` — deterministic RNG for reproducible tests

## Key Domain Concepts

- **Game**: Central entity with states, turns, teams, players, and territories
- **Map/MapTemplate**: Game board definition with countries and connections
- **Victory Conditions**: Strategy pattern — `IVictoryCondition` with implementations (Rush, Capitals, Control Continent, Survival)
- **Tournaments**: Multi-round competitive events with pairings and teams
- **Ladders**: Ranked queues using Glicko2 rating algorithm
- **Alliances**: Player groups
- **Bots**: AI players for games

## Deployment

- **CI/CD**: GitHub Actions (`.github/workflows/deploy.yaml`) on push to `master`
- **Build**: Multi-stage Docker build (`Dockerfile`)
- **Environments**: dev (auto-deploy) → production (manual approval)
- **Runtime**: .NET 6.0 ASP.NET Core container on port 5000
- **Database**: SQL Server (separate container)
- **Secrets**: Environment-specific `appsettings.{environment}.json` via Docker secrets

## Key Dependencies

| Category | Package | Version |
|----------|---------|---------|
| Web Framework | ASP.NET Core | 6.0 |
| ORM | Entity Framework Core (SQL Server) | 6.0.1 |
| Auth | OpenIddict | 3.1.1 |
| DI Container | Autofac | 6.3.0 |
| Object Mapping | AutoMapper | 9.0.0 |
| Background Jobs | Hangfire | 1.7.28 |
| Real-Time | SignalR | 6.0 |
| Logging | NLog | 4.7.13 |
| API Docs | NSwag | 13.15.5 |
| Profiling | MiniProfiler | 4.2.22 |
| Serialization | Newtonsoft.Json | — |
| Testing | MSTest + Moq | 2.2.8 / 4.16.1 |
