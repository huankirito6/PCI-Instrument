# Decision 0001 — Phase 0 Baseline

**Status:** Proposed for founder review
**Date:** 2026-07-20

## Decisions recorded from survey

1. V0 is an internal tool for pressure/differential-pressure Yokogawa RFQ and quotation work.
2. Initial series: EJA110E, EJA430E, EJA530E and EJAC80E.
3. Initial users: founder plus 1-2 internal users.
4. Input: PDF, Excel and scan.
5. Output: Excel and PDF.
6. Vendor communication remains manual.
7. Final quotation approval remains with management.
8. Pricing factor, selling price and GP are manually entered/approved.
9. External AI/cloud analysis is allowed by the user for internal development, with encryption/protection requirements.
10. Commercialization outside the company is not approved for V0.
11. Coordination model: Claude Opus 4.8 is the default coordinator/backend model; GPT-5.6-SOL is reserved for frontend/UI/UX to conserve credits.
12. Source control: GitHub repo https://github.com/huankirito6/PCI-Instrument.git for change tracking and backup.

## Decisions still requiring evidence or confirmation

- AI/OCR provider retention, training use, region and deletion terms.
- Approved catalog/GS sources per model series.
- Exact critical order-code rules.
- Canonical quotation template and PDF rendering method.
- Official money rounding and VAT policy.
- Retention and hard-delete period.

## Consequence

Implementation should begin with documentation, schema and synthetic/approved fixtures. No real sensitive quotation should be uploaded to a provider until the data-egress evidence above is recorded.
