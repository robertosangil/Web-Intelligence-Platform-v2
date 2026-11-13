---
name: wip-frontend-engineering
description: Implement or review WIP v2 frontend changes under `web/` using Next.js 16, React 19, TypeScript, Node.js 26, and the versioned API boundary. Use for frontend code, configuration, API consumption, or browser-facing behavior.
---

# WIP Frontend Engineering

Read root `AGENTS.md`, `web/AGENTS.md`, the affected code, and the PBI. Because this Next.js version may differ from prior knowledge, consult the relevant installed documentation under `web/node_modules/next/dist/docs/` before changing framework behavior.

- Preserve Next.js 16, React 19, TypeScript, Node.js 26, and the existing App Router structure.
- Consume the backend through explicit versioned API contracts. Do not access SQL Server or persistence directly from the frontend.
- Keep public environment values non-secret. Never put credentials or server-only secrets in `NEXT_PUBLIC_*` variables.
- Implement only approved user behavior and cover relevant loading, empty, unavailable, validation, and error states accessibly.
- Avoid unrelated UI frameworks, state libraries, abstractions, generated-file edits, or speculative dashboard features.
- Surface contract mismatches to the Coordinator and .NET owner instead of inventing response fields or fallback semantics.

Run affected checks from `web/`: `npm run lint`, `npm run typecheck`, and `npm run build` when appropriate. Add focused tests or browser verification only when the PBI or behavior warrants them. Report API assumptions, exact commands and results, routes or browser evidence observed, limitations, and unresolved backend needs. A successful build is not browser proof.
