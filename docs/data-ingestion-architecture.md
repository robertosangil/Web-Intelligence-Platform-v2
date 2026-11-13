# WIP v2 Provider Data Ingestion Architecture

Status: proposed architecture for PBI-017  
Scope: read-only design for migrating the WIP data-table model to SQL Server and introducing WIP v2 collection workflows.

## Evidence baseline

The source snapshot is `C:\Web Intelligence Platform\data table 11112026`. It contains 58 CSV exports: 20 Cloudflare datasets, 13 cPanel datasets, 9 GoDaddy datasets, 14 WordPress/WP Umbrella datasets, and 2 shared operational datasets (`website_monitoring_transactions` and `website_monitoring_collector_health`).

The prior n8n exports show scheduled/manual provider collectors that call provider APIs, normalize responses in Code nodes, and upsert rows into n8n Data Tables. The n8n webhook workflows are authenticated GET reporting/query endpoints; no dedicated inbound provider POST ingestion workflow was found in the inspected exports.

WIP v2 currently contains only identity/access/session persistence. The Worker is a foundation executable and does not yet collect or persist provider data.

## Approved boundaries

- `Wip.Worker` owns scheduling and provider collection execution.
- `Wip.Application` owns collection use cases, contracts, idempotency decisions, and retry classification.
- `Wip.Infrastructure` owns provider HTTP adapters and external-service concerns.
- `Wip.Persistence` owns EF Core models, SQL Server mappings, indexes, and migrations.
- `Wip.Api` owns versioned read contracts and any explicitly authorized ingestion boundary.
- The frontend consumes the API and never accesses SQL Server directly.
- EF Core remains the unit-of-work/change-tracking mechanism; no generic repository layer is introduced.

## Shared lifecycle

```text
Schedule/manual trigger
  -> create CollectionRun
  -> create SourceAttempt records
  -> provider adapter request
  -> normalize and validate response
  -> persist raw evidence reference and normalized rows
  -> complete SourceAttempt and CollectionRun
  -> expose data through versioned API
```

Each run has a provider, collector name, requested period, workflow/execution reference, start/end timestamps, status, completeness, and retry count. Each source attempt records the logical source/endpoint, attempt number, HTTP/result status, retryability, safe error category, and timestamps.

## Persistence strategy

The first shared SQL foundation should support:

- `CollectionRuns`
- `CollectionSourceAttempts`
- `RawProviderPayloads` or a reference to retained raw evidence
- `IngestionErrors`
- `IngestionIdempotency`

Provider data should remain in deliberate normalized tables rather than one generic JSON table. JSON is retained only where the source is genuinely variable or is evidence that is not part of a query contract.

Every provider table should include a surrogate key, a provider/business key, collection-run reference, observation/report period, created/updated timestamps, and a unique index for its logical deduplication key.

## Provider mapping

| Provider | Initial SQL domains |
|---|---|
| Cloudflare | Daily reports, traffic intervals, cache intervals, origin intervals, HTTP errors, security, DNS, SSL/TLS, zones/accounts, rules, configuration changes, data quality, attention items |
| GoDaddy | Domains, status snapshots/components, incidents, maintenance, changes, API health/evidence, transactions |
| cPanel | Resource samples/summaries, daily statistics, server errors, SSL certificates, AutoSSL, backups, databases, server info, capabilities, API evidence, transactions |
| WP Umbrella | Project snapshots, plugins, themes, vulnerabilities, uptime, incidents, PHP issues/info, backups, processes, daily summaries, changes, attention items, API evidence, transactions |

The legacy cPanel resource sample dataset must be classified explicitly as historical/legacy before schema implementation; it must not silently become the default current sample model.

## Idempotency and duplicate handling

Idempotency is based on a stable provider/business key plus the collection context where required. Examples from the source dump include `record_key`, `transaction_id`, `snapshot_key`, provider event IDs, and `event_hash`.

- Enforce keys with SQL Server unique indexes, not only application lookups.
- Store an idempotency key and payload hash for externally submitted batches.
- Treat a replay with the same key and equivalent payload as a successful no-op.
- Treat the same key with a conflicting payload hash as a conflict requiring review.
- Use a transaction around each normalized source section and its related run state.
- Prefer constrained insert/update logic with concurrency handling; do not rely on an unconstrained check-then-insert pattern.

## Error and retry policy

Errors are classified as:

- `Transient`: timeout, connection failure, provider 5xx, or retryable rate limiting.
- `Permanent`: invalid credentials, unsupported capability, invalid request, or contract/validation failure.
- `Partial`: one source section failed while other sections produced valid data.

Transient failures receive bounded retries with backoff and a recorded next-attempt time. Permanent failures complete the source attempt without retry. Partial runs remain visible as incomplete and do not erase successful sections. A durable dead-letter/review state is required before production use.

The prior n8n workflows record errors and have reconciliation/recovery workflows, but do not provide one consistent portable retry/backoff/dead-letter contract. WIP v2 must make those states explicit.

## API direction

Provider collection should normally be Worker-to-Application-to-Persistence. If an inbound trusted integration is later required, use an authenticated, versioned boundary such as:

```text
POST /api/v1/ingestion/{provider}/{dataset}
```

It must require an idempotency key and collection metadata, and return `202 Accepted` for accepted work or an idempotent replay. It must not expose raw credentials or permit arbitrary table writes.

Read APIs should eventually replace direct n8n Data Table reads:

```text
GET /api/v1/cloudflare/...
GET /api/v1/godaddy/...
GET /api/v1/cpanel/...
GET /api/v1/wordpress/...
GET /api/v1/attention
GET /api/v1/reports
```

Exact endpoint contracts are deferred to the provider-schema and read-API PBIs.

## Migration sequence

1. Define this architecture and resolve Product Owner decisions.
2. Add shared collection-run, source-attempt, raw-evidence, error, and idempotency persistence.
3. Add one provider schema at a time: Cloudflare, GoDaddy, cPanel, then WP Umbrella.
4. Import and reconcile the `data table 11112026` snapshot only after target schemas are verified.
5. Implement Worker scheduling and provider collectors.
6. Add SQL-backed read APIs and validate their results against the n8n responses.
7. Transition reporting consumers without deleting or disabling n8n workflows until a separately approved cutover.

## Decisions requiring Product Owner approval

1. Whether the first delivery is Cloudflare-only or includes all providers in the first release.
2. Retention periods for raw payloads, time-series data, and operational run history.
3. Whether raw provider payloads are stored in SQL Server, external blob storage, or both.
4. Whether an inbound POST ingestion API is needed, or Worker collection is the only write path.
5. Production retry limits, rate-limit handling, and dead-letter operational ownership.
6. Whether historical dump data is imported as authoritative history or treated as a reference-only baseline.

## Non-goals for PBI-017

This document does not create entities, migrations, provider integrations, scheduled jobs, API endpoints, database objects, Azure resources, or data imports.
