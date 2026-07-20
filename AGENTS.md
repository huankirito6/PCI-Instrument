# RFQ Copilot — Operating Rules

This repository implements the Internal RFQ & Quotation Copilot V0. The approved product scope and roadmap are in the project documents under `docs/` and `.hermes/plans/`.

## Non-negotiable rules

1. Follow the approved V0 scope. Do not add email automation, ERP/CRM, multi-tenant SaaS, new product families or commercialization without an explicit founder decision.
2. Never fabricate critical technical or commercial fields. Missing data remains `null`, `missing`, `unknown`, `conflicting` or `unverified` as appropriate.
3. LLM/OCR may extract, normalize, explain and draft. Deterministic rules, approved catalog data and human approval decide critical fields.
4. Do not send real sensitive RFQ, customer, vendor quotation, cost or selling-price data to an external service until the data-egress/retention conditions are recorded and verified.
5. Use TDD for deterministic domain, calculation, approval, matching, security and export behavior: RED then GREEN then REFACTOR.
6. Every task must state acceptance criteria and exact verification commands.
7. Do not declare completion from code inspection alone. Run relevant tests/builds and report actual output.
8. Do not commit secrets, raw sensitive fixtures, generated customer quotations or production credentials.
9. Do not silently change domain rules, calculation formulas, approval semantics, critical-field lists or API contracts.
10. Keep the initial system internal, Yokogawa-first and limited to pressure/differential-pressure workflows.

## Ownership

- Coordinator/backend/domain/API/security: Claude Opus 4.8 (default profile).
- Frontend/UI/UX: GPT-5.6-SOL (frontend profile), used sparingly to conserve credits.
- Founder: final authority for scope, domain correctness, data policy, model/order-code rules and commercial decisions.

## Git workflow

- `main` is the stable branch; do not force-push it.
- Work on `feat/*`, `fix/*` or `docs/*` branches and open small, reviewable changes.
- Push frequently so GitHub holds a backup; a bad change can be reverted to the previous commit.
- Commit messages use `feat:`, `fix:`, `docs:`, `test:`, `refactor:`, `chore:`.

## Completion report

Every completed task reports: what changed, why, files changed, tests/build checks with actual results, artifacts, remaining risks and whether a founder decision is required.

## Build / verification baseline

```bash
dotnet restore
dotnet build --no-restore
dotnet test --no-build
```

(These apply once the .NET solution exists in Phase 3.)
