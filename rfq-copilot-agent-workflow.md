# RFQ Copilot — Agent Workflow

## Agent configuration

| Agent | Hermes profile | Model | Reasoning | Role |
|---|---|---|---|---|
| Frontend | `frontend` | `gpt-5.6-sol` | `high` | UX/UI design and frontend implementation |
| Backend | `backend` | `claude-opus-4-8` | `xhigh` | Backend/domain/API/security implementation (UltraCode role) |

The detailed operating rules are in `AGENTS.md`. The approved product roadmap is `rfq-copilot-v0-final-roadmap.md`.

Frontend UI/UX tasks must force-load `ui-ux-pro-max`. Use `claude-design` for design process/prototypes, `popular-web-designs` for a reference design vocabulary, and `design-md` when the output is a repository design-token specification.

## Recommended operating mode

Use the Hermes Kanban board for durable multi-agent work. Each feature becomes linked tasks instead of letting both agents edit the same files concurrently.

### Feature graph

```text
Founder-approved feature brief
          ↓
Backend contract/domain task (`backend`)
          ↓
Frontend UI task (`frontend`)
          ↓
Integration acceptance task
          ↓
Founder acceptance for critical behavior
```

A design-only task can start with `frontend`. A pure backend task can stay with `backend`. A cross-stack task must be split.

## Task template — frontend

```text
Role: Frontend owner — GPT-5.6-SOL
Read AGENTS.md and rfq-copilot-v0-final-roadmap.md first.
Load and apply the ui-ux-pro-max skill before designing or changing UI.

Goal:
[Describe one screen/component/interaction]

Backend contract:
[Endpoint/schema/fixture/version, or state that this is design-only]

Contract/schema version:
[Version or N/A for design-only work]

Security/data classification:
[Public/internal/confidential/restricted; state whether real data is allowed]

Required states:
loading, empty, success, error, missing, unverified, conflict, approval-blocked

Acceptance criteria:
- [Visual/interaction behavior]
- [Accessibility]
- [Responsive behavior]
- [Browser/component tests]
- [Screenshot evidence]

Out of scope:
- Backend domain rules
- Database changes
- Authoritative pricing/order-code logic

Completion report:
List files, tests, browser checks, screenshots and remaining unknowns.
```

## Task template — backend

```text
Role: Backend owner — Claude Opus 4.8 UltraCode
Read AGENTS.md and rfq-copilot-v0-final-roadmap.md first.
Use TDD for deterministic behavior.

Goal:
[Describe one domain/API/persistence/security behavior]

Contract:
[Request/response, status enums, validation errors and source semantics]

Contract/schema version:
[Version and location under `docs/contracts/`]

Security/data classification:
[Public/internal/confidential/restricted; state whether real data is allowed]

Acceptance criteria:
- Write failing tests first.
- Critical fields retain source/verification state.
- Unknown never becomes pass.
- Approval/security/calculation gates are enforced.
- Build and relevant tests pass.

Out of scope:
- Frontend visual redesign
- Speculative infrastructure
- V0 scope expansion

Completion report:
List contract changes, files, migrations, tests and actual command output.
```

## Cross-stack handoff checklist

- Contract committed/documented before frontend integration.
- Request/response example or mock fixture supplied.
- Explicit handling of every UI state.
- Error codes are stable and human-readable mapping is possible.
- Source, warning and approval semantics are preserved.
- No sensitive data is included in mock fixtures.
- Contracts and fixtures are stored under `docs/contracts/` or the explicitly approved repository equivalent.
- Both agent test suites pass.
- P0/P1 review findings are closed.

The founder or a founder-designated integrator performs the final cross-stack merge. When agents disagree or a founder decision is pending, the task is blocked; neither agent may silently substitute an assumption for the decision.

## Founder approval gates

The founder must decide or approve:

- Data-egress policy.
- Critical-field list.
- Supported model series.
- Source of truth for order-code validity.
- Model/order-code selection.
- Cost, factor, selling price and commercial terms.
- Any V0 scope expansion.
- Any unresolved P0/P1 issue.

## Commands

Inspect profiles:

```bash
hermes profile list
hermes profile show frontend
hermes profile show backend
```

Run a direct frontend task:

```bash
hermes -p frontend chat -q "Read AGENTS.md and implement the assigned frontend task"
```

Force-load UI/UX Pro Max for a direct frontend task:

```bash
hermes -p frontend -s ui-ux-pro-max chat -q "Read AGENTS.md and implement the assigned frontend task"
```

Run a direct backend task:

```bash
hermes -p backend chat -q "Read AGENTS.md and implement the assigned backend task"
```

Kanban commands are preferable for implementation work because they preserve state, handoffs and audit history:

```bash
hermes kanban list
hermes kanban show <task-id>
hermes kanban comment <task-id> "<handoff or review note>"
```

Use worktree-backed tasks when agents edit a real git repository. Do not use two agents on the same checkout concurrently.

For Kanban frontend tasks, attach the skill explicitly:

```bash
hermes kanban create "<frontend task>" --assignee frontend --skill ui-ux-pro-max --workspace worktree
```
