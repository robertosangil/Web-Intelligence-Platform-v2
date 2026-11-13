---
name: wip-pbi-delivery
description: Deliver or review an approved WIP v2 PBI from repository inspection through scoped implementation, relevant validation, and evidence-based handoff. Use for PBI work; do not treat it as authorization to commit, deploy, access production, or expand scope.
---

# WIP PBI Delivery

1. Confirm the WIP v2 repository, branch, HEAD, working tree, applicable `AGENTS.md`, and PBI source. Preserve pre-existing and unrelated work.
2. Separate approved decisions, requested scope, non-goals, acceptance criteria, assumptions, and decisions requiring Product Owner approval. Stop affected work when a material choice or authorization is missing.
3. Identify affected surfaces, dependencies, file ownership, and the smallest relevant agent assignment. Parallelize only independent tasks; keep overlapping edits with one coordinated owner.
4. Before editing, state the intended files and verification proportional to risk. Do not broaden the PBI through opportunistic refactoring or infrastructure.
5. Implement only authorized scope. Treat application changes, Git actions, Azure/production actions, credentials, migrations, and destructive operations as separate authorization boundaries.
6. Validate the affected surfaces using current observable evidence. Use the backend, frontend, or verification skill when its trigger applies.
7. Inspect the final diff and repository status. Report files changed, commands and results, acceptance-criteria status, limitations, unresolved decisions, and one `PASS`, `CONDITIONAL`, `FAIL`, or `UNVERIFIED` verdict.

For sequential delivery, finish one PBI before selecting another. When the task explicitly authorizes Git integration, commit only the scoped PBI change, push it, and merge through the approved pull-request workflow. Track enhancements as separate PBIs rather than expanding the active change.

Never claim delegation, test execution, runtime behavior, browser behavior, deployment, or production verification without direct evidence.
