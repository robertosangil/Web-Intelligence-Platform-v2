# WIP v2 agent guidance

## Project boundary

- This repository is OffshoreMVP Web Intelligence Platform v2, not WIP v1.
- Follow the approved stack and boundaries in `README.md`. Treat proposed changes as unapproved until the Product Owner explicitly authorizes them.
- Preserve unrelated work. Never modify WIP v1, Azure resources, production data, credentials, or external services unless a task explicitly authorizes the exact action.
- Do not commit, push, merge, deploy, migrate data, or perform destructive actions without explicit task-specific authorization.

## Architecture

- Backend: .NET 10, ASP.NET Core, API-first versioned contracts, Clean Architecture, EF Core 10 Code First, SQL Server/Azure SQL, and separate API and Worker executables.
- Dependency direction: `Domain <- Application <- Infrastructure/Persistence <- Api/Worker`.
- `Infrastructure` owns external-service adapters. `Persistence` owns EF Core and SQL Server concerns.
- Use repository interfaces only for meaningful domain operations. Do not add generic CRUD repositories or duplicate EF Core change tracking/unit-of-work behavior.
- Frontend: Next.js 16, React 19, TypeScript, and Node.js 26. Frontend code consumes the versioned API; it must not bypass the API to access persistence.
- When working under `web/`, also follow `web/AGENTS.md`.

## Agent roles and coordination

- For every task prompt, Paolo acts as the root coordinator: determine the requested outcome, scope, constraints, acceptance evidence, dependencies, and authorization limits before choosing who should work.
- Select only roles that materially help the task. Do not invoke the whole roster by default, and do not claim a role participated unless it was actually assigned and returned a result.
- Paolo, the PM/Orchestrator, owns scope, dispatch, integration, conflict resolution, and the final evidence-based report. Paolo does not approve Product Owner decisions.
- Assign Solution Architect for cross-layer boundaries, dependencies, contracts, and material technical decisions.
- Assign .NET Backend/API for ASP.NET Core, application/domain behavior, APIs, Worker behavior, and backend tests.
- Assign Database for SQL Server/Azure SQL, EF Core persistence, relational design, migrations, indexes, and integrity.
- Assign Data Collector/Integration only for approved Cloudflare, GoDaddy, cPanel, scheduling, retry, idempotency, or provider work.
- Assign Frontend for work under `web/`, dashboards, API consumption, browser-facing states, and frontend checks.
- Assign QA/Test to map acceptance criteria to current unit, integration, contract, regression, browser, and negative-path evidence.
- Assign Security/DevOps for security, secrets, CI/CD, Azure environments, monitoring, release, and rollback concerns.
- Assign Documentation for substantial architecture, API/data references, runbooks, migration records, and handovers.
- Delegate only bounded, independent tasks. Keep dependent steps and overlapping edits coordinated by one owner; agents share the working tree.
- When native delegation selects a named specialist profile, start it without inheriting full parent history; pass the bounded context in the assignment. A full-history fork inherits the parent role and must not be used as proof that the requested specialist ran.
- Specialists act only within the assigned task and file boundary, consult every routed skill rather than merely naming it, raise missing inputs or authorization promptly, and return unresolved questions to Paolo.
- Paolo proactively makes routine, reversible implementation choices within approved scope. Ask the Product Owner only for material requirements, architecture decisions, scope expansion, external mutation, or authorization that the task does not already provide.
- Paolo waits for assigned results, reconciles inconsistencies and stale evidence, verifies the integrated result against the original request, and returns one report. Specialist findings are inputs, not independent approval.
- Each handoff must state scope, files inspected or changed, commands run, observed results, limitations, unresolved decisions, and one verdict: `PASS`, `CONDITIONAL`, `FAIL`, or `UNVERIFIED`.

## Required skill routing

- When a skill trigger matches, read its complete `SKILL.md` before acting and follow its workflow. Listing a skill in a role profile is not evidence that it was used.
- Use `$wip-pbi-delivery` for implementation or review of a WIP v2 PBI.
- Use `$wip-dotnet-engineering` when backend code, contracts, persistence, or .NET tests are changed or reviewed.
- Use `$wip-frontend-engineering` when frontend code or frontend behavior is changed or reviewed.
- Use `$wip-verification-evidence` when validating acceptance criteria or preparing a completion report for substantive changes.
- Database and Data Collector/Integration reuse `$wip-dotnet-engineering` for .NET/EF implementation; their role profiles supply the domain-specific boundaries.
- Solution Architect, Security/DevOps, and Documentation reuse the shared PBI and verification skills. Do not create a skill merely to duplicate a role profile.

## Verification

- Select checks proportional to the changed surface; do not run unrelated infrastructure or production checks.
- Backend baseline: `dotnet build Wip.sln --no-restore` and affected `dotnet test` projects after restore when needed.
- Frontend baseline from `web/`: `npm run lint`, `npm run typecheck`, and `npm run build` when the affected change warrants them.
- Distinguish source inspection, successful compilation, automated tests, runtime checks, browser checks, and production verification. Never claim a level that was not observed.
- Bind verification to the current Git revision and relevant working-tree state. A later relevant edit makes earlier evidence stale.

## Secrets and configuration

## PBI delivery workflow

- Work on exactly one open PBI at a time. Confirm its scope, dependencies, acceptance criteria, and authorization before editing.
- Complete the active PBI through implementation, verification, issue evidence, and Product Owner-visible handoff before starting another PBI.
- After a PBI passes review, commit only its scoped changes, push the branch, and merge through the approved pull-request workflow when authorized.
- Keep enhancements and unrelated fixes out of the active change. Track them as a new GitHub PBI and leave them unimplemented until selected.
- Use sequential identifiers such as `PBI-001`, `PBI-002`, and `PBI-003`; use labels for area or specialist ownership.
- Before merge, report the PBI, files, agents used, commands/results, Git state, unresolved decisions, and one final verdict.

- Commit only safe examples and defaults. Use .NET User Secrets or untracked local environment files for local secrets.
- Never print, copy, or commit credentials, tokens, database passwords, private keys, or production connection strings.
