---
name: wip-dotnet-engineering
description: Implement or review WIP v2 backend changes involving .NET, ASP.NET Core, API contracts, workers, EF Core, SQL Server, or backend tests while preserving the approved Clean Architecture and data-access boundaries.
---

# WIP .NET Engineering

Read root `AGENTS.md`, `README.md`, the affected projects, and the PBI before deciding an approach.

- Preserve .NET 10, ASP.NET Core, explicit versioned API contracts, separate API and Worker executables, EF Core 10 Code First, and SQL Server/Azure SQL compatibility.
- Enforce the dependency direction `Domain <- Application <- Infrastructure/Persistence <- Api/Worker`.
- Keep domain rules in Domain, use cases and meaningful abstractions in Application, external adapters in Infrastructure, and EF Core/SQL Server implementation in Persistence.
- Add repository interfaces only for meaningful domain operations. Do not add generic CRUD wrappers or duplicate EF Core change tracking and unit-of-work behavior.
- Keep endpoints and background processing thin; do not move business rules into startup, controllers/endpoints, EF configurations, or Worker plumbing.
- Do not create entities, migrations, provider integrations, authentication, queues, or Azure resources unless the PBI explicitly requires them.

Choose focused checks for the affected projects. Restore when dependencies changed or assets are absent, build with warnings treated as errors, run affected tests, and perform API/Worker runtime checks when behavior requires them. Report exact commands, results, architecture impact, limitations, and handoff needs; compilation alone does not prove runtime or database behavior.
