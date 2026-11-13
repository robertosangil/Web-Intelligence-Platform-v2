---
name: wip-verification-evidence
description: Verify WIP v2 acceptance criteria against current observable evidence and prepare an honest completion report. Use when validating substantive changes, reviewing readiness, or resolving disputed success claims.
---

# WIP Verification and Evidence

1. Record the PBI scope, acceptance criteria, current HEAD, relevant working-tree state, and the exact claim being verified.
2. Map each criterion to the smallest observable check that can prove it. Distinguish source review, static analysis, build, automated test, runtime, browser, integration, and production evidence.
3. Inspect the implementation and test quality; test presence or test counts do not prove the intended behavior.
4. Run authorized, relevant checks and preserve exact commands, exit results, and important observations. Record skipped, unavailable, or environment-dependent checks explicitly.
5. Treat evidence as revision-bound. If a relevant file changes after verification, mark affected results stale and require targeted revalidation.
6. Classify each criterion as `PASS`, `CONDITIONAL`, `FAIL`, or `UNVERIFIED`. Missing evidence is `UNVERIFIED`, not automatically `FAIL`; a reproducible contrary result is `FAIL`.
7. Return an acceptance-criteria matrix, defects and reproduction details, limitations, unresolved questions, revalidation needs, and one overall verdict using the same vocabulary.

Do not infer deployment or production behavior from repository state, a push, or local tests. Do not expose secrets in logs or reports. QA findings are technical evidence, not Product Owner approval.
