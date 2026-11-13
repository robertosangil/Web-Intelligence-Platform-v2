# OffshoreMVP Web Intelligence Platform v2

WIP v2 is the repository-managed successor to the validated WIP v1 platform. This foundation contains the application skeleton and the initial Identity/access persistence model; product modules, provider collectors, dashboard features, and product entities remain outside the current scope.

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
- `Wip.Persistence`: EF Core, SQL Server, Code First configuration, Identity/access entities, and migrations.
- `Wip.Api`: versioned HTTP API and OpenAPI document.
- `Wip.Worker`: independent background-processing executable.
- `web`: Next.js frontend consuming the API.

EF Core supplies change tracking and unit-of-work behavior. Repository interfaces should only be introduced for meaningful domain operations.

## PBI delivery workflow

WIP v2 is delivered one PBI at a time. Paolo confirms scope, dependencies, acceptance criteria, and authorization; assigns only the necessary agents; implements the bounded change; verifies it; records evidence on GitHub; and reports one final verdict. After review, the scoped change is committed, pushed, and merged when authorized. Enhancements are tracked as separate PBIs and are not mixed into active work.

PBI identifiers use the sequential format `PBI-001`, `PBI-002`, `PBI-003`. Area labels identify authentication, database, frontend, security, or documentation concerns.

## Identity foundation

The original identity-foundation PBI established the Identity persistence foundation in `Wip.Persistence` using ASP.NET Core Identity with GUID user and role keys. It configured unique email addresses, framework password hashing, configurable password policy, lockout defaults, and default token providers. Later PBIs now provide the versioned authentication, account, role/module access, and frontend flows; email delivery, broad administration, and employee association remain separate concerns.

The current local defaults require an 8-character password with digit, lowercase, uppercase, and non-alphanumeric characters; five failed attempts lock an account for 15 minutes. The shorter local minimum is a Product Owner-requested development setting and is not a production security recommendation.

The API configures JWT bearer validation from the `Jwt` configuration section (`Issuer`, `Audience`, `SigningKey`, and `ClockSkewSeconds`). The signing key is intentionally blank in committed configuration and must be supplied through local User Secrets or an approved hosted-environment configuration. Authentication, profile, permissions, and logout flows are versioned under `/api/v1`; the frontend consumes them through its API client.

The persistence model maps Identity and access tables to `Users`, `Roles`, `UserRoles`, `UserClaims`, `UserLogins`, `UserTokens`, `RoleClaims`, `Modules`, and `RoleModules`.

The module catalogue is fixed reference data. Each page is a separate module with a stable key, group, display name, and order; module records are not user-deletable or user-creatable. The current local migration seeds 32 page modules across Dashboard, Cloudflare, GoDaddy, cPanel, WP Umbrella, and Settings/User Management. Future provider data contracts attach to these page-level keys rather than to the navigation group headings.

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
   dotnet user-secrets set --project src/Wip.Api "Jwt:SigningKey" "<local development signing key>"
   dotnet user-secrets set --project src/Wip.Worker "ConnectionStrings:WipDatabase" "<local SQL Server connection string>"
   ```

The API and worker can start without a database connection during foundation development. Database-dependent features and migrations must supply `ConnectionStrings:WipDatabase`. The API project has a User Secrets ID, so values set with `dotnet user-secrets` remain outside the repository. Do not put connection strings, JWT signing keys, or other credentials in `appsettings.json`, `.env`, or committed files.

Apply the checked-in EF Core migrations locally after setting the design-time environment variable:

```powershell
$env:ConnectionStrings__WipDatabase = "<local SQL Server connection string>"
dotnet ef database update --project src/Wip.Persistence --startup-project src/Wip.Api
```

The migration history includes the initial Identity schema and the subsequent rename/addition migration for the table names listed above. This documents local database setup only; it does not represent a production migration or deployment.

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

Azure resources, production configuration, production migrations, provider integrations, product entities, and live deployment remain outside this repository-only setup. See [SECURITY.md](SECURITY.md) for the readiness outline and security boundaries.
