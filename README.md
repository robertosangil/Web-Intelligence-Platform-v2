# OffshoreMVP Web Intelligence Platform v2

WIP v2 is the repository-managed successor to the validated WIP v1 platform. This foundation contains only the application skeleton: no product modules, provider collectors, user features, dashboard features, product entities, or migrations are present.

## Architecture

The backend follows a pragmatic Clean Architecture dependency direction:

```text
Wip.Domain
    ^
Wip.Application
    ^
Wip.Infrastructure    Wip.Persistence
           \            /
             Wip.Api
             Wip.Worker

web (Next.js) ---> versioned HTTP API
```

- `Wip.Domain`: domain model and rules. Empty until a product PBI defines them.
- `Wip.Application`: use cases and meaningful repository abstractions. No generic CRUD repository is provided.
- `Wip.Infrastructure`: future external-service and platform adapters.
- `Wip.Persistence`: EF Core, SQL Server, Code First configuration, and future migrations.
- `Wip.Api`: versioned HTTP API and OpenAPI document.
- `Wip.Worker`: independent background-processing executable.
- `web`: Next.js frontend consuming the API.

EF Core supplies change tracking and unit-of-work behavior. Repository interfaces should only be introduced for meaningful domain operations.

## Prerequisites

- .NET SDK 10.0.403 or a compatible 10.0 patch
- Node.js 26 LTS and npm
- Docker Desktop for the local SQL Server 2025 Developer container

## Local SQL Server

1. Copy `.env.example` to `.env`.
2. Set `MSSQL_SA_PASSWORD` in `.env` to a strong local-only password.
3. Start SQL Server:

   ```powershell
   docker compose up -d sqlserver
   ```

4. Store the matching development connection string outside source control:

   ```powershell
   dotnet user-secrets set --project src/Wip.Api "ConnectionStrings:WipDatabase" "<local SQL Server connection string>"
   dotnet user-secrets set --project src/Wip.Worker "ConnectionStrings:WipDatabase" "<local SQL Server connection string>"
   ```

The API and worker can start without a database connection during foundation development. Database-dependent features and migrations must supply `ConnectionStrings:WipDatabase`. No migration exists in PBI-001.

## Run locally

Restore and build the backend:

```powershell
dotnet restore Wip.slnx
dotnet build Wip.slnx --no-restore
```

Run the API at `http://localhost:5080`:

```powershell
dotnet run --project src/Wip.Api --launch-profile http
```

The foundation endpoints are:

- Health: `GET http://localhost:5080/api/v1/health`
- OpenAPI (Development): `GET http://localhost:5080/openapi/v1.json`

Run the worker:

```powershell
dotnet run --project src/Wip.Worker
```

Run the frontend at `http://localhost:3000`:

```powershell
Copy-Item web/.env.example web/.env.local
Set-Location web
npm install
npm run dev
```

`NEXT_PUBLIC_API_BASE_URL` configures the frontend's API base URL. The committed example targets the local API and contains no secret.

## Tests and checks

```powershell
dotnet test Wip.slnx
Set-Location web
npm run lint
npm run typecheck
npm run build
```

## Configuration and secrets

- Commit safe defaults only.
- Use .NET User Secrets for local backend secrets.
- Use untracked `.env.local` for local frontend configuration.
- Use environment variables or approved Azure configuration in hosted environments.
- Never commit database passwords, provider credentials, tokens, or production connection strings.

Azure resources, production configuration, deployment workflows, authentication, provider integrations, product entities, and data migration are intentionally outside PBI-001.
