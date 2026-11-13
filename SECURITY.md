# Security and deployment readiness

## Configuration

Tracked configuration contains placeholders only. Local secrets belong in .NET User Secrets or an untracked frontend `.env.local`. Hosted values must be supplied through Azure App Service configuration or another approved secret store; credentials must never be committed or written to logs.

The API fails startup outside Development when `Jwt:SigningKey` is missing. Production signing keys must be long, random, rotated through the hosting secret-management process, and never shared in issue comments or deployment files.

## Operational checks

- `/api/v1/health` is a liveness endpoint and does not prove database readiness.
- Run backend build/tests and frontend lint/typecheck/build in CI.
- Review authentication failures, refresh-token revocation, and authorization denials without logging passwords, tokens, or connection strings.
- Treat Azure deployment, production migrations, rollback, monitoring exporters, and live-provider checks as separately authorized activities.

## Azure App Service readiness outline

For a separately approved deployment, configure the API and frontend as independent App Services, set environment-specific values through App Service configuration, restrict CORS to the deployed frontend origin, enable HTTPS-only, and run the health check path `/api/v1/health`. Validate startup, authentication, database connectivity, logs, and rollback in a non-production slot before production promotion. No Azure resources are created by this repository workflow.
