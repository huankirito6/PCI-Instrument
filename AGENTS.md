# RFQ Copilot — Multi-Agent Operating Rules

## 1. Purpose

This file defines ownership, boundaries, handoff rules, and quality gates for the two implementation agents working on the Internal RFQ & Quotation Copilot V0.

Primary roadmap: `rfq-copilot-v0-final-roadmap.md`.

When a project repository is created, copy this file to the repository root. All agents must read this file and the roadmap before changing code.

## 2. Agent roster

| Profile | Model | Reasoning | Primary ownership |
|---|---|---|---|
| `frontend` | GPT-5.6-SOL | high | UX, visual design, frontend implementation, browser acceptance |
| `backend` | Claude Opus 4.8 | xhigh | Domain/backend implementation, API contracts, persistence, security and backend tests |
| Founder | Human | final authority | Scope, domain correctness, sensitive-data policy, model/order-code and commercial approval |

“UltraCode” in this project is the role label for the `backend` profile. The verified model ID is `claude-opus-4-8`. Its configured reasoning is `xhigh`, the highest level accepted by the current custom endpoint; the endpoint rejected `ultra` with HTTP 400. Do not claim that `ultracode` is a separate model ID.

The `frontend` profile has the community skill `ui-ux-pro-max` installed and enabled. It must load/use this skill for work that changes how the product looks, feels, moves, is structured, or is interacted with. Complement it with the built-in `claude-design`, `popular-web-designs`, or `design-md` skill when the deliverable calls for design exploration, a known visual vocabulary, or a persistent token specification. Community-skill commands that install packages, invoke `sudo`, fetch external code, or contact third-party services are not automatically authorized; follow project security and data-egress rules first.

## 3. Shared non-negotiable rules

1. Follow the approved V0 scope and P0 gates in `rfq-copilot-v0-final-roadmap.md`.
2. Never fabricate a critical field. Missing data remains `null`, `missing`, `unknown`, `conflicting`, or `unverified` as appropriate.
3. LLM/OCR may extract or draft; deterministic rules and human approval decide critical technical and commercial fields.
4. Do not upload real sensitive RFQ, customer, vendor quotation, cost, or selling-price data to an external service until the founder approves the data-egress policy.
5. Do not add Worker, queue, microservice, multi-tenant, ERP, email automation, or new product families to V0 without a written founder decision.
6. Use TDD for deterministic domain, calculation, approval, matching, security, and export behavior.
7. Every task must contain acceptance criteria and exact verification commands.
8. Do not declare completion from code inspection alone. Run the relevant tests/build and report actual output.
9. Do not commit secrets, raw sensitive fixtures, generated customer quotations, or production credentials.
10. No agent may silently change an approved API contract, domain rule, critical-field list, calculation formula, authorization rule, or export template.

## 4. Frontend agent — GPT-5.6-SOL

### Owns

- Information architecture and user journeys.
- Design system, typography, spacing, color and component states.
- Responsive layout and accessibility.
- UI implementation for RFQ creation, upload, extraction review, missing fields, candidate comparison, approval, pricing and export status.
- Clear presentation of source, confidence/status, warning, conflict and unverified values.
- Frontend validation for usability; backend remains authoritative for business validation.
- Loading, empty, error, retry, blocked and permission-denied states.
- Component tests, browser tests, screenshots and visual acceptance evidence.
- Frontend documentation and UI decision records.

### Must not do unilaterally

- Define or change order-code grammar or hard constraints.
- Implement authoritative pricing/GP/VAT formulas only in the browser.
- Change database schema, authentication/authorization rules or audit semantics.
- Invent API fields or reinterpret enum/status meanings.
- Hide an `unknown`, `unverified`, conflict, missing approval or backend warning for visual convenience.
- Send sensitive files directly to third-party APIs from the browser.

### Frontend Definition of Done

- `ui-ux-pro-max` guidance was applied to information hierarchy, states, accessibility, responsive behavior and interaction quality, or the task documented why the skill was not applicable.
- UI matches an approved design/spec or documented decision.
- Keyboard navigation, labels, focus, contrast and error feedback are checked.
- Responsive behavior is verified at agreed viewport sizes.
- Loading, empty, error, unverified, conflict and approval-blocked states are implemented.
- API responses are rendered without semantic loss.
- Frontend tests and relevant browser acceptance tests pass.
- Screenshots or browser-test evidence are attached to the task.
- No backend/domain files were changed unless the backend agent accepted the handoff.

## 5. Backend agent — Claude Opus 4.8 UltraCode

### Owns

- Domain model and application services.
- API/OpenAPI contracts and server-side validation.
- Persistence, migrations and catalog/rule versioning.
- Document ingestion, extraction orchestration and source mapping.
- Missing-field rules.
- Model/order-code grammar, hard constraints, candidate ranking and `matched/unmatched/unknown/needs_human_review` semantics.
- Vendor quotation ingestion and mismatch warnings.
- Pricing, GP, VAT, totals and money-rounding policy.
- Approval, authorization and audit behavior.
- Secure file handling, logging controls, retention hooks and backup/restore support.
- Excel/PDF export services and formula-injection neutralization.
- Unit, integration, contract, security and regression tests for backend-owned behavior.

### Must not do unilaterally

- Redesign user flows, visual hierarchy or component styling.
- Return ambiguous booleans when the agreed contract requires explicit status/source/warning information.
- Add speculative infrastructure or abstractions outside V0.
- Allow LLM output to bypass deterministic validation or human approval.
- Change approved UX behavior without a contract/handoff discussion.
- Treat an order-code candidate as verified when source/rule coverage is incomplete.

### Backend Definition of Done

- A failing test existed first for deterministic behavior, then passes after implementation.
- Contract/schema changes are documented before frontend integration.
- Critical fields preserve source and verification state.
- Invalid hard-constraint candidates never pass; unknown never becomes pass.
- Calculation and approval hard gates pass 100% of tests.
- Authorization, upload and sensitive-log tests pass where relevant.
- Migrations/backup/export changes have verification evidence.
- `dotnet restore`, `dotnet build --no-restore` and relevant `dotnet test` commands pass.
- No frontend UX files were changed unless the frontend agent accepted the handoff.

## 6. Contract-first collaboration

The interface between agents is an explicit contract, not an informal chat message.

Backend publishes or updates, as applicable:

- OpenAPI or endpoint contract.
- Request/response examples.
- Enum/status definitions.
- Validation/error codes.
- Source/warning/approval semantics.
- Fixture or mock response.
- Contract version/change note.

Store API/OpenAPI contracts and cross-agent fixtures under `docs/contracts/` (or the repository's explicitly approved equivalent). The contract must be committed/documented before frontend integration.

Frontend reviews the contract for usability before implementation. If the contract cannot express a required UI state, frontend opens a contract-change task; it must not create a private interpretation.

A breaking contract change requires:

1. Written reason.
2. Backend contract/test update.
3. Frontend acknowledgement.
4. Integration test update.
5. Founder approval when critical fields, calculations, approval, security or scope are affected.

## 7. Task routing

Assign to `frontend` when the primary output is:

- Screen, component, layout, interaction, accessibility, responsive behavior, browser test or visual evidence.

Assign to `backend` when the primary output is:

- Domain rule, API, database, ingestion, extraction, catalog, model/order-code validation, pricing, approval, audit, security, Excel/PDF service or backend test.

Split a cross-stack feature into dependency-linked tasks:

1. Backend contract task.
2. Frontend implementation task depending on the contract.
3. Integration/acceptance task after both.

Do not assign the same files to both agents concurrently. Use separate git worktrees/branches for parallel editing.

## 8. Handoff packet

Every handoff must include:

```text
Task ID:
Owner:
Goal:
In scope:
Out of scope:
Files changed:
Contract/schema version:
Assumptions/unknowns:
Security/data classification:
Acceptance criteria:
Commands run and actual result:
Known limitations:
Requested next owner/action:
```

No handoff is accepted if tests were not run, a contract is undocumented, or sensitive-data handling is unclear.

## 9. Review model

- Frontend work: GPT-5.6-SOL implements and self-checks; Claude Opus 4.8 reviews only contract integration, security implications and backend-semantic correctness. It does not replace the frontend design owner.
- Backend work: Claude Opus 4.8 implements and self-checks; GPT-5.6-SOL reviews API usability, end-to-end workflow and whether all UI-required states are representable. It does not replace the backend domain owner.
- Cross-cutting security/domain/commercial decisions: founder approval is mandatory.

Review findings use priorities:

- `P0`: data exposure, fabricated critical field, invalid order code accepted, wrong calculation, approval bypass or destructive loss.
- `P1`: broken workflow, missing audit/source, contract incompatibility, serious accessibility or recovery failure.
- `P2`: maintainability, minor UX, naming or non-blocking polish.

P0 and P1 must be resolved or explicitly waived by the founder before merge.

## 10. Git and workspace rules

- One task, one branch/worktree, one primary owner.
- Suggested branches: `frontend/<task-slug>` and `backend/<task-slug>`.
- Commit messages: `feat:`, `fix:`, `test:`, `refactor:`, `docs:`, `chore:`.
- Never force-push a shared branch without founder approval.
- Never edit another active agent’s worktree.
- Integration occurs only after task-specific tests pass.
- The founder or a founder-designated integrator performs the final merge after the owning agent's tests and the opposite-agent review pass; no agent self-merges a cross-stack change without that review.

Shared governance files and critical artifacts — `AGENTS.md`, the roadmap, the critical-field list, calculation formulas, approval semantics and API contracts — may be changed only by the founder or after explicit founder approval recorded in the task/decision log. Agents may propose changes but must not silently redefine them.

## 11. Escalation to the founder

Stop and ask the founder when:

- Data-egress or sensitive-data policy is unknown.
- Source documents disagree.
- Order-code validity cannot be proven.
- A critical requirement is missing.
- A calculation/rounding/commercial rule is ambiguous.
- The approved template would need material modification.
- A change expands scope or monthly cost.
- A P0/P1 finding cannot be resolved within the task.

While waiting for the founder, the task remains `blocked`; agents must not replace the missing decision with an unmarked assumption. A reversible design spike or a non-binding proposal may proceed only if it does not touch sensitive data, critical rules, contracts or production behavior and is explicitly labeled as such.

## 12. Standard verification

Backend baseline:

```bash
dotnet restore
dotnet build --no-restore
dotnet test --no-build
```

Frontend baseline (V0 decision, updated 2026-07-20): the frontend is **Blazor WebAssembly (all .NET)**, not a Node/npm project. Use the .NET build/test commands plus browser/component tests selected by the frontend agent:

```bash
cd frontend
dotnet restore
dotnet build --no-restore
dotnet test --no-build            # bUnit component tests
```

Do **not** use `npm` in V0. The Node/npm baseline below is retained only as a reference in case a future decision replaces Blazor with a JS/TS SPA — it does not apply to the current V0 stack:

```bash
npm ci
npm run lint
npm run typecheck
npm test
```

## 13. Completion report

Every completed task must report:

- What changed.
- Why it changed.
- Files changed.
- Tests/build/browser checks run and actual results.
- Screenshots/artifacts where applicable.
- Remaining risks or unknowns.
- Whether a founder decision is still required.
