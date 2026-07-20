# Internal RFQ & Quotation Copilot V0 Implementation Plan

> **For Hermes:** Use subagent-driven-development skill to implement this plan task-by-task.

**Goal:** Trong 16 tuần, xây và kiểm chứng một công cụ nội bộ hỗ trợ đọc RFQ, phát hiện thông tin thiếu, đề xuất model/order code cho pressure/DP Yokogawa, đọc báo giá hãng, tính giá/GP dưới sự kiểm soát của người dùng và xuất đúng form Excel/PDF của công ty.

**Architecture:** Bắt đầu bằng concierge prototype để kiểm chứng schema và workflow, sau đó chuyển thành modular monolith bằng ASP.NET Core. LLM/OCR chỉ dùng cho parsing, extraction và drafting; rule/catalog cùng human approval quyết định các trường kỹ thuật và thương mại quan trọng.

**Tech Stack:** C#/.NET, ASP.NET Core, xUnit, PostgreSQL hoặc database local phù hợp, PDF/Excel parsers, OCR/LLM structured output, Excel template export và PDF rendering.

---

## Constraints

- 12 giờ làm việc mỗi tuần.
- Ngân sách dưới 500.000 VNĐ/tháng.
- AI coding model/agent viết phần lớn mã; founder review và acceptance test.
- Chỉ dùng nội bộ công ty; chưa thương mại hóa.
- Người dùng thử: founder và 1–2 người nội bộ.
- Input: PDF, Excel và scan.
- Output: Excel và PDF.
- Không tự gửi email; không cho hãng truy cập hệ thống.
- Giá, hệ số, model và các trường critical phải được human approve.

---

## Phase 0 — Scope, security and acceptance baseline

**Timeline:** Week 1

**Objective:** Chốt V0, data boundary, catalog nguồn và định nghĩa kết quả đúng.

**Deliverables:**

- `docs/product/v0-scope.md`
- `docs/security/data-classification.md`
- `docs/domain/pressure-dp-required-fields-v0.yaml`
- `evals/acceptance-criteria.md`

**Exit gate:** Có template báo giá, catalog/GS nguồn và tiêu chí model/quotation correctness rõ ràng.

---

## Phase 1 — Domain schema and ground truth

**Timeline:** Weeks 2–3

**Objective:** Tạo schema pressure/DP, catalog subset và đáp án chuẩn trước khi xây app.

**Deliverables:**

- `docs/domain/pressure-dp-schema-v0.md`
- `catalog/sample-data/pressure-dp-v0.csv`
- `evals/fixtures/`
- `evals/expected/`
- `evals/manifest.json`

**Exit gate:** Tối thiểu 3 bộ ground truth được founder duyệt, tổng 5–10 fixture thật/synthetic.

---

## Phase 2 — Concierge prototype

**Timeline:** Weeks 4–5

**Objective:** Chứng minh AI/parser có thể hỗ trợ workflow end-to-end trước khi đầu tư vào nền tảng.

**Deliverables:**

- Prototype đọc PDF/Excel/scan.
- JSON structured extraction.
- Missing-information output.
- Candidate/model draft có source.
- Vendor quotation extraction.
- GP calculation.
- `templates/quotation-template-v0.xlsx`
- `research/findings/correction-log.csv`

**Exit gate:** Không bịa critical field, model có source, workbook đúng trường chính và giúp giảm nhập liệu.

---

## Phase 3 — Internal application foundation

**Timeline:** Weeks 6–7

**Objective:** Đưa prototype vào ứng dụng .NET nội bộ nhỏ, có state và audit.

**Likely structure:**

```text
src/
├── RfqCopilot.Api/
├── RfqCopilot.Application/
├── RfqCopilot.Domain/
├── RfqCopilot.Infrastructure/
└── RfqCopilot.Worker/
tests/
├── RfqCopilot.Domain.Tests/
├── RfqCopilot.Application.Tests/
└── RfqCopilot.IntegrationTests/
```

**Core modules:** RFQ, Document, Requirement, Catalog, VendorQuote, CustomerQuote, Approval and Audit.

**Exit gate:** Tạo RFQ, upload file, xem trạng thái/requirements, lưu correction và chạy test domain ổn định.

---

## Phase 4 — Document ingestion and missing information

**Timeline:** Weeks 8–9

**Objective:** Xử lý PDF/Excel/scan có source mapping và phát hiện thông tin thiếu bằng rule.

**TDD targets:**

- PDF text giữ page number.
- Excel field giữ sheet/cell.
- Scan rỗng kích hoạt OCR.
- Corrupted file trả error code rõ.
- Missing field trả `null`, không suy đoán.
- Field không có source là `unverified`.

**Exit gate:** Mỗi critical field có source hoặc warning; regression chạy trên ít nhất 5 fixture.

---

## Phase 5 — Model matching and vendor quotation ingestion

**Timeline:** Weeks 10–11

**Objective:** Đề xuất candidate bằng hard constraints/soft ranking và đọc lại dữ liệu báo giá hãng.

**TDD targets:**

- Candidate vi phạm hard constraint luôn bị loại.
- Unknown không được tính là pass.
- Kết quả có matched/unmatched/unknown và source.
- Model hãng xác nhận được lưu riêng với model dự kiến.
- Model/range/quantity mismatch sinh warning.

**Exit gate:** Founder hiểu và kiểm tra được lý do đề xuất trước khi approve.

---

## Phase 6 — Pricing, GP and quotation export

**Timeline:** Weeks 12–13

**Objective:** Tạo đúng báo giá công ty, kiểm soát tính toán và chặn export nếu chưa review.

**Required formulas:**

```text
Line total = Quantity × Selling price
GP = Selling price - Cost price
GP% = (Selling price - Cost price) / Selling price × 100%
Subtotal = Sum of line totals
VAT = Subtotal × VAT rate
Grand total = Subtotal + VAT
```

**TDD targets:**

- Dùng nguyên giá hãng hoặc hệ số do user nhập.
- Chặn chia cho 0 khi tính GP%.
- Total/VAT/GP đúng ở boundary cases.
- Chặn export khi critical approval còn thiếu.
- Excel mở được.
- PDF không vỡ layout ở fixture chuẩn.
- Neutralize Excel formula injection.

**Exit gate:** Excel/PDF đúng mẫu, tính đúng 100% trên test cases và không tự gửi đi.

---

## Phase 7 — Internal pilot and hardening

**Timeline:** Weeks 14–15

**Objective:** Chạy founder + 1–2 internal users trên 5–10 fixture/RFQ, đo thời gian, độ đúng và mức sửa.

**Metrics:**

- Model family correctness.
- Order-code acceptance/edit rate.
- Missing-field recall.
- Unsupported critical assertions.
- Input-to-export time.
- Number of manual data-entry corrections.
- Quotation-template pass rate.
- Calculation pass rate.

**Exit gate:** Không bịa critical field, báo giá đúng mẫu, tính toán đúng và output được chấp nhận sau review.

---

## Phase 8 — Go/Adjust/Harden/Stop gate

**Timeline:** Week 16

**Objective:** Ra quyết định tiếp theo bằng dữ liệu pilot.

**Deliverables:**

- `evals/reports/internal-pilot-v0.md`
- `docs/decision-log/gate-v0.md`
- KPI before/after.
- Evidence-based backlog.

**Decision rules:**

- **Go:** dùng lặp lại, model đủ tin cậy và báo giá giảm thao tác.
- **Adjust:** pain đúng nhưng schema/model family/template chưa đúng.
- **Harden:** hữu ích nhưng cần cải thiện bảo mật, stability hoặc PDF layout.
- **Stop/Pause:** không dùng lặp lại hoặc không đảm bảo data/model correctness.

---

## Weekly operating cadence

| Activity | Hours/week |
|---|---:|
| Workflow/data and ground-truth review | 2 |
| AI-assisted coding | 5 |
| Automated tests and evaluation | 2 |
| Domain/output review | 2 |
| Documentation and decision log | 1 |

## Global verification commands

Commands will be finalized after repository creation, but expected quality gates are:

```bash
dotnet restore
dotnet build --no-restore
dotnet test --no-build
```

When a frontend is introduced:

```bash
npm ci
npm run lint
npm run typecheck
npm test
```

## Risks and mitigations

- **Too few real samples:** add synthetic fixtures and increase corpus before claiming reliability.
- **LLM hallucination:** JSON schema, null for missing, citations, deterministic rules and human approval.
- **Sensitive quotations:** TLS, encrypted storage, redaction/pseudonymization, no sensitive application logs.
- **PDF layout instability:** retain the approved Excel template and build repeatable rendering tests.
- **Catalog/order-code complexity:** restrict V0 to four model series and version rules/catalog.
- **Scope growth:** no email automation, ERP, multi-tenant SaaS, additional product family or commercialization in V0.

## Definition of Done for V0

- PDF/Excel/scan can be ingested.
- Critical requirements have source or are marked unverified.
- Missing fields are detected by rules.
- Invalid candidates are rejected by hard constraints.
- User approves the selected model/order code.
- Vendor quotation is parsed and compared.
- User manually approves cost, factor, selling price, GP and commercial terms.
- Excel and PDF quotation match the approved company template.
- Calculation, approval and audit tests pass.
- Pilot output is accepted by founder and at least one internal reviewer without recreating the quotation from scratch.
