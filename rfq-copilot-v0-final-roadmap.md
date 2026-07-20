# Internal RFQ & Quotation Copilot V0

## Roadmap và kế hoạch triển khai cuối cùng

> Bản tổng hợp phối hợp giữa phân tích của GPT-5.6-SOL và review độc lập của Claude Opus 4.8.
>
> **Trạng thái:** Conditional Go — có thể tiếp tục, nhưng phải vượt các P0 gate trước khi xử lý dữ liệu báo giá thật.
>
> **Ngày tổng hợp:** 19/07/2026

---

## 1. Tóm tắt điều hành

### 1.1. Mục tiêu

Trong 16 tuần, với tối đa 12 giờ/tuần và ngân sách dưới 500.000 VNĐ/tháng, xây và kiểm chứng một công cụ nội bộ hỗ trợ một workflow báo giá cụ thể cho pressure/DP Yokogawa:

1. Đọc RFQ từ PDF, Excel và — nếu khả thi — scan.
2. Trích xuất yêu cầu kỹ thuật có source mapping.
3. Phát hiện thông tin thiếu bằng rule, không tự suy đoán.
4. Đề xuất model family, option và order code trong phạm vi V0.
5. Đọc báo giá hãng và so sánh với yêu cầu.
6. Tính cost, factor, selling price, GP, VAT và total.
7. Cho người dùng review/approve các trường critical.
8. Xuất Excel/PDF theo một template công ty đã được xác nhận.
9. Lưu correction, approval và audit trail để có thể giải thích kết quả.

### 1.2. Phán quyết

**Nên tiếp tục, nhưng không triển khai nguyên bản kế hoạch ban đầu.**

Kế hoạch ban đầu có triết lý đúng: workflow-first, ground truth trước code, human-in-the-loop và có gate Stop/Adjust. Tuy nhiên, cần sửa ba điểm trước:

1. **Chốt data-egress và security boundary** trước khi gửi bất kỳ dữ liệu thật nào tới OCR/LLM.
2. **Chứng minh order-code validation bằng spike nhỏ** trước khi xây matching engine lớn.
3. **Giảm kiến trúc V0** xuống một ứng dụng nội bộ mỏng, xử lý đồng bộ; bỏ Worker, queue, microservice và các abstraction chưa cần thiết.

### 1.3. Mục tiêu thực tế của V0

V0 không phải một quotation platform hoàn chỉnh. V0 phải chứng minh được:

> Một người dùng nội bộ có thể đưa vào một RFQ thuộc phạm vi đã chọn, xem extraction có nguồn, biết phần còn thiếu, kiểm tra candidate/model, tự approve các trường critical, tính giá chính xác và xuất một quotation đúng template mà không phải dựng lại toàn bộ từ đầu.

Nếu không chứng minh được workflow này, không mở rộng sang ERP, email automation, SaaS, nhiều product family hoặc multi-tenant.

---

## 2. Nguồn thông tin và quy ước đọc tài liệu

### 2.1. Fact đã có trong yêu cầu ban đầu

- Thời gian: 16 tuần.
- Thời lượng: 12 giờ/tuần.
- Ngân sách: dưới 500.000 VNĐ/tháng.
- Người dùng thử: founder và 1–2 người nội bộ.
- Phạm vi: nội bộ, chưa thương mại hóa.
- Input: PDF, Excel, scan.
- Output: Excel và PDF.
- Không tự gửi email.
- Không cho hãng truy cập hệ thống.
- Giá, hệ số, model và trường critical phải được human approve.
- V0 giới hạn tối đa bốn model series.

### 2.2. Kết quả review của Claude Opus 4.8

Claude Opus 4.8 đánh giá kế hoạch ở mức **có điều kiện**: triết lý sản phẩm tốt nhưng có rủi ro over-building, thiếu data-egress policy, thiếu order-code validation, thiếu catalog versioning, auth, backup/retention và acceptance thresholds.

### 2.3. Assumption/unknown phải được chốt

Các điểm sau chưa được xác nhận trong yêu cầu ban đầu và không được coi là fact:

- Giá hãng, giá bán và tên khách có được phép gửi lên cloud LLM/OCR hay không.
- Nguồn machine-readable để xác minh grammar và tính hợp lệ của order code.
- Số lượng RFQ mỗi tháng và số người dùng đồng thời.
- Ứng dụng chạy trên máy cá nhân, localhost hay server nội bộ.
- Template Excel/PDF công ty đã sẵn sàng đến mức nào.
- Chính sách lưu trữ, retention, backup và xóa dữ liệu của công ty.
- Founder muốn V0 tạo full order code hay chỉ đề xuất model family/option.
- Các threshold KPI cuối cùng được người có trách nhiệm nghiệp vụ phê duyệt.

---

## 3. Phạm vi V0

### 3.1. In scope

- Một workflow pressure/DP cụ thể.
- Tối đa bốn model series.
- PDF có text layer.
- Excel RFQ và quotation.
- Scan ở mức feasibility spike; chỉ đưa vào pilot nếu đạt ngưỡng chất lượng.
- Required fields và source mapping.
- Missing-information detection bằng rule.
- Catalog subset có version.
- Model matching bằng hard constraint và soft ranking.
- Order-code validator trong phạm vi đã được xác minh.
- Vendor quotation extraction.
- Pricing/GP calculation deterministic.
- Human approval cho model/order code và trường thương mại.
- Audit trail.
- Excel/PDF export từ template đã được duyệt.
- Correction log và evaluation fixtures.

### 3.2. Out of scope

- Tự gửi email.
- Tự đặt hàng hoặc giao tiếp với hãng.
- ERP/CRM integration.
- Multi-tenant SaaS.
- Public deployment.
- Nhiều product family ngoài phạm vi pressure/DP.
- Tự phê duyệt hoặc tự quyết giá bán.
- Tự khẳng định order code không có source hoặc rule xác minh.
- Microservices, message queue, background Worker trong V0.
- Tối ưu hóa cho volume lớn trước khi có số liệu pilot.

---

## 4. Các P0 gate bắt buộc

Không được chuyển sang xử lý dữ liệu thật nếu một trong các gate dưới đây chưa đạt.

### P0-1 — Data-egress và data classification

Phải có tài liệu `docs/security/data-classification.md` quy định:

- Loại dữ liệu public, internal, confidential và restricted.
- Trường nào được gửi tới cloud LLM/OCR.
- Trường nào phải redaction/pseudonymization.
- Provider có lưu request/response hay dùng cho training không.
- Log nào được phép lưu.
- Cách xử lý raw file, extracted text, prompt và model response.
- Cách xóa dữ liệu tạm.
- Người có quyền thay đổi policy.

Khuyến nghị mặc định cho V0:

- OCR tài liệu nhạy cảm chạy local trước.
- Không gửi giá hãng, giá bán, tên khách hàng hoặc điều khoản thương mại lên cloud nếu chưa được phê duyệt.
- Nếu dùng cloud LLM cho extraction, chỉ gửi dữ liệu đã redaction hoặc phần không nhạy cảm.
- Pin provider, model và prompt/schema version.
- Không ghi raw quotation vào application log.

Nếu policy công ty không cho phép cloud processing, V0 phải chạy local hoặc giảm scope về PDF/Excel extraction có kiểm soát.

### P0-2 — Order-code validation spike

Trước khi xây matching engine đầy đủ, phải chứng minh:

- Grammar hoặc cấu trúc segment của order code.
- Option hợp lệ theo từng model series.
- Combination bị cấm.
- Nguồn dùng để xác minh.
- Cách xử lý unknown.
- Cách gắn source và catalog/rule version.
- Người chịu trách nhiệm xác nhận ground truth.

Kết quả phải phân biệt:

```text
matched
unmatched
unknown
needs_human_review
```

**Unknown không được tính là pass.**

Nếu không thể xác minh full order code, V0 phải chuyển scope thành:

- Đề xuất model family.
- Đề xuất option chính.
- Hiển thị candidate và source.
- Người dùng tự xác minh full order code ngoài hệ thống.

### P0-3 — Không over-build

V0 dùng một deployable app tối giản và xử lý đồng bộ. Không tạo Worker/queue/API service riêng nếu chưa có nhu cầu đo được.

### P0-4 — Acceptance thresholds

Các metric phải có định nghĩa, mẫu đo, baseline và pass/fail trước pilot. Không chờ tới tuần 16 mới quyết định thế nào là thành công.

---

## 5. Workflow người dùng mục tiêu

```text
Create RFQ
   ↓
Upload PDF / Excel / scan
   ↓
Ingest + classify + extract
   ↓
Show extracted fields + source + confidence/status
   ↓
Detect missing critical fields
   ↓
Apply hard constraints
   ↓
Rank valid candidates
   ↓
Human reviews model/order code and corrections
   ↓
Upload/read vendor quotation
   ↓
Compare RFQ ↔ vendor quote ↔ selected model
   ↓
Enter/approve cost, factor, selling price, commercial terms
   ↓
Calculate GP/VAT/total deterministically
   ↓
Approval gate
   ↓
Export approved Excel/PDF
   ↓
Audit + correction log + evaluation record
```

Không được để hệ thống “âm thầm sửa” dữ liệu. Mọi sửa critical phải hiển thị cho người dùng và tạo audit event.

---

## 6. Nguyên tắc correctness và human control

### 6.1. Critical field

Danh sách critical field phải được chốt trong:

```text
docs/domain/pressure-dp-required-fields-v0.yaml
```

Ví dụ nhóm field cần xem xét:

- Model family.
- Model code/order code.
- Measurement range.
- Process connection.
- Wetted material.
- Output/signal.
- Accuracy/performance requirement.
- Quantity.
- Cost price.
- Factor.
- Selling price.
- VAT/commercial terms.

Danh sách trên chỉ là nhóm gợi ý; founder/domain reviewer phải phê duyệt danh sách cuối cùng.

### 6.2. Trạng thái dữ liệu

Mỗi field quan trọng nên có trạng thái:

```text
verified
user_confirmed
extracted_unverified
inferred_not_allowed
missing
conflicting
unknown
```

V0 không cho phép trạng thái `inferred` được dùng như `verified`.

### 6.3. Source mapping

Mỗi extracted field phải có source nếu có thể:

```text
SourceRef
- document_id
- page_number       # PDF
- sheet_name        # Excel
- cell_or_range     # Excel
- bounding_box      # OCR, nếu có
- extraction_method
- extractor_version
```

Field không có source phải được đánh dấu `unverified` và không được tự động dùng để approve.

---

## 7. Kiến trúc V0 cuối cùng

### 7.1. Hình dạng ứng dụng

**Quyết định (cập nhật 20/07/2026):** V0 là một web hoàn chỉnh, **tách backend và
frontend thật sự**, toàn bộ trong môi trường .NET. Đây là thay đổi có chủ đích so
với bản trước (vốn đề xuất một app server-render tối giản để tiết kiệm giờ). Founder
đã chọn web đầy đủ; đổi lại timeline và ngân sách giờ công phải được tính lại (xem
Mục 11 và 12). Việc tách tầng cũng khớp trực tiếp với mô hình 2 agent: backend agent
sở hữu API/domain, frontend agent sở hữu SPA.

- **Backend** = ASP.NET Core **Web API** (REST/JSON, OpenAPI/Swagger). Không render
  HTML; chỉ trả JSON theo contract trong `docs/contracts/`.
- **Frontend** = **Blazor WebAssembly** (SPA thật chạy trong trình duyệt, vẫn là
  C#/.NET). Gọi backend qua HTTP/JSON. Không chứa business rule authoritative.

```text
RfqCopilot/
├── backend/
│   ├── src/
│   │   ├── RfqCopilot.Domain/            # entity, rule, value objects (thuần)
│   │   ├── RfqCopilot.Application/       # use case, service, validation
│   │   ├── RfqCopilot.Infrastructure/    # EF Core, storage, OCR/LLM adapter, export
│   │   └── RfqCopilot.Api/               # ASP.NET Core Web API + OpenAPI (chủ backend)
│   └── tests/
│       ├── RfqCopilot.Domain.Tests/
│       ├── RfqCopilot.Application.Tests/
│       └── RfqCopilot.Api.IntegrationTests/
├── frontend/
│   └── RfqCopilot.Web/                   # Blazor WebAssembly SPA (chủ frontend)
│       ├── (Pages, Components, Services gọi API)
│       └── ...
├── frontend-tests/
│   └── RfqCopilot.Web.Tests/             # bUnit component tests
├── docs/
│   └── contracts/                        # OpenAPI + fixture chia sẻ giữa 2 agent
├── catalog/
├── evals/
├── templates/
└── README.md
```

Ranh giới sở hữu:

- `backend/` (Domain, Application, Infrastructure, Api) + `docs/contracts/` → backend agent.
- `frontend/RfqCopilot.Web` + `frontend-tests/` → frontend agent.
- Giao diện giữa hai bên **chỉ** là OpenAPI contract trong `docs/contracts/`, không
  chia sẻ code business trực tiếp.

Vẫn giữ nguyên các ràng buộc chống over-build: **không** Worker, queue, microservice,
multi-tenant trong V0. "Tách backend/frontend" ở đây nghĩa là hai deployable (một API,
một SPA) — không phải nhiều service.

Ghi chú lựa chọn frontend: dùng Blazor WebAssembly để giữ đúng cam kết "tất cả trong
.NET". Nếu sau này cần hệ sinh thái component/JS phong phú hơn, có thể đổi sang SPA
JS/TS (React/Vue) mà **không** phải sửa backend, vì backend chỉ là Web API thuần —
đây là lợi ích của việc tách tầng ngay từ đầu.

### 7.2. Thành phần đề xuất

| Thành phần | Lựa chọn V0 | Ghi chú |
|---|---|---|
| Backend runtime | ASP.NET Core Web API | Trả JSON theo contract; có OpenAPI/Swagger |
| Frontend | Blazor WebAssembly (SPA) | Chạy trong trình duyệt, C#/.NET; gọi backend qua HTTP |
| Contract | OpenAPI/Swagger sinh từ backend | Nguồn sự thật của giao diện 2 tầng; lưu trong `docs/contracts/` |
| API auth | Token/cookie phù hợp deployment | SPA và API tách domain → cần CORS + auth rõ ràng |
| Data access | EF Core | Chỉ ở backend; domain/application tách khỏi storage |
| Database | SQLite cho single-machine; PostgreSQL nếu shared server | Chốt theo deployment thực tế |
| Excel | ClosedXML hoặc thư viện đã được công ty phê duyệt | Ở backend; kiểm tra license và khả năng mở file |
| PDF | QuestPDF hoặc pipeline đã được kiểm chứng | Ở backend; kiểm tra license theo version và doanh thu tổ chức; không mặc định miễn phí |
| OCR | Local OCR trước; cloud chỉ khi policy cho phép | Ở backend; scan phải qua feasibility test |
| LLM | Chỉ extraction/drafting | Ở backend; không để LLM quyết critical field |
| Queue/Worker | Không dùng | Chỉ thêm nếu pilot chứng minh cần |
| Backend tests | xUnit + integration tests | Có fixture regression |
| Frontend tests | bUnit (component) + browser acceptance | Frontend agent chọn; có screenshot evidence |
| Catalog | Database/table có version/effective date | Không dùng CSV phẳng làm source duy nhất |

Tên thư viện chỉ là đề xuất kỹ thuật, không phải quyết định license cuối cùng. Phải kiểm tra license/version trước khi đưa vào repository hoặc môi trường công ty.

### 7.3. Database tối thiểu

Các aggregate/table chính:

```text
Rfq
Document
ExtractedField
SourceReference
Requirement
CatalogVersion
CatalogItem
RuleVersion
VendorQuote
VendorQuoteLine
CustomerQuote
QuoteLine
Approval
AuditEvent
Correction
EvaluationRun
```

Các record liên quan đến kết quả phải lưu version tương ứng:

- `catalog_version`.
- `rule_version`.
- `price_list_version` nếu có.
- `extractor_version`.
- `llm_model_version` nếu dùng LLM.
- `prompt_or_schema_version` nếu phù hợp.

### 7.4. Quyền tối thiểu

V0 có thể dùng bốn role:

```text
Viewer
Operator
Approver
Admin
```

Tối thiểu phải kiểm soát:

- Ai được xem raw document.
- Ai được sửa extraction.
- Ai được approve model/order code.
- Ai được approve cost/factor/selling price.
- Ai được export.
- Ai được quản lý catalog/rule.
- Audit record không bị sửa tùy ý bởi user thông thường.

---

## 8. Data security, backup và audit

### 8.1. Security tối thiểu

- Authentication phù hợp với deployment.
- Authorization theo role.
- HTTPS nếu truy cập qua network.
- Không commit secrets vào repository.
- Không ghi raw quotation hoặc full prompt vào log mặc định.
- Temporary files phải có lifecycle rõ ràng.
- File upload giới hạn kích thước, extension và content type.
- Chống path traversal khi lưu file.
- Chống Excel formula injection khi export.
- Validate input trước khi parse.
- Error message không làm lộ dữ liệu nhạy cảm.

### 8.2. Encryption và storage

- Nếu chạy trên một máy: dùng encrypted volume/storage được công ty chấp nhận.
- Nếu cần database-level encryption: đánh giá SQLite encryption/SQLCipher về license, compatibility và backup trước khi chọn.
- Nếu chạy shared server: cân nhắc PostgreSQL và cơ chế encryption/backup của môi trường đó.

### 8.3. Backup/restore

Phải có tài liệu:

```text
docs/operations/backup-restore-v0.md
```

Tối thiểu quy định:

- Backup khi nào.
- Backup ở đâu.
- Mã hóa backup ra sao.
- Giữ bao lâu.
- Cách restore.
- Ai có quyền restore.
- Kiểm tra restore bằng fixture không nhạy cảm.

Có thể bắt đầu với backup local được mã hóa và kiểm tra restore thủ công, nhưng không được coi “có file backup” là đã có disaster recovery.

### 8.4. Audit event

Mỗi thay đổi critical nên lưu:

```text
who
when
rfq_id
entity
field
old_value_or_hash
new_value_or_hash
reason
source
catalog_version
rule_version
```

Audit log nên append-only ở tầng nghiệp vụ; quyền xóa/sửa trực tiếp chỉ dành cho administrator theo policy rõ ràng.

---

## 9. Acceptance criteria và KPI

Các ngưỡng dưới đây là **đề xuất ban đầu**. Founder/domain reviewer phải phê duyệt hoặc điều chỉnh trong tuần 1.

### 9.1. Hard gates

| Metric | Ngưỡng V0 | Cách đo |
|---|---:|---|
| Fabricated critical field | 0 | Audit toàn bộ output trên fixture/pilot |
| Unsupported critical assertion được coi là verified | 0 | Kiểm tra trạng thái và source |
| Invalid candidate vượt hard constraint | 0 | Negative test suite |
| Calculation pass rate | 100% | Expected-value tests + boundary cases |
| Export khi critical approval còn thiếu | 0 | Integration tests |
| Excel formula injection không được neutralize | 0 | Security tests |
| Audit event bị mất khi sửa critical field | 0 | Integration tests |

### 9.2. Target quality

| Metric | Target đề xuất | Ghi chú |
|---|---:|---|
| Model family correctness | ≥95% trên eligible cases | Không tính case thiếu dữ liệu cần thiết |
| Full order-code top-1 exact match | ≥80% trên eligible cases | Chỉ áp dụng nếu validator đã có ground truth |
| Correct order code nằm trong top-3 | ≥95% trên eligible cases | Nếu V0 có ranking nhiều candidate |
| Missing-field recall | ≥90% | Critical missing fields phải được cảnh báo |
| Quotation-template pass rate | 100% trên template fixture | Kiểm tra bằng mở file/render review |
| Input-to-export time | Giảm tối thiểu 30% so với baseline | Đo cùng nhóm workflow |
| Manual data-entry corrections | Có giảm so với baseline | Ghi số lần sửa, không chỉ cảm nhận |
| Pilot users muốn dùng lại | ≥1 internal reviewer ngoài founder | Chất lượng phải tạo usage lặp lại |

Các tỷ lệ phần trăm trong bảng chỉ là chỉ báo ban đầu khi corpus còn nhỏ. Trước khi có tối thiểu 20 eligible cases cho một metric, báo cáo phải ghi cả số tuyệt đối theo dạng `x/N đúng`, không dùng phần trăm đơn lẻ để tuyên bố độ tin cậy. V0 với 5–10 fixture chỉ dùng để quyết định feasibility và hướng điều chỉnh.

### 9.3. Quy tắc diễn giải

- 0 fabricated critical field, calculation 100% và không bypass approval là hard gate.
- Order-code exact match không đồng nghĩa với auto-approval.
- Unknown phải chuyển sang human review.
- 5–10 fixture chỉ đủ cho feasibility/pilot ban đầu, không đủ để tuyên bố độ tin cậy tổng quát.
- Mọi metric phải ghi rõ denominator, eligible case và loại lỗi.
- Mục tiêu giảm thời gian phải so trên cùng bộ case hoặc các case có độ phức tạp tương đương; báo cáo phải ghi số case baseline, median/average phù hợp và các bước nào được tính vào thời gian `input-to-export`.

---

## 10. Chiến lược test và evaluation

### 10.1. Ground truth

Tạo:

```text
docs/domain/pressure-dp-schema-v0.md
catalog/sample-data/pressure-dp-v0.csv
catalog/versions/
evals/fixtures/
evals/expected/
evals/manifest.json
```

Mỗi fixture cần có:

- Input file.
- Loại document.
- Expected extracted fields.
- Expected missing fields.
- Expected source.
- Expected candidate status.
- Expected calculation.
- Expected warnings.
- Người duyệt.
- Version catalog/rule.

### 10.2. Test targets

#### Ingestion

- PDF text giữ page number.
- Excel giữ sheet/cell.
- Scan rỗng kích hoạt OCR/fallback.
- Corrupted file trả error code rõ.
- File vượt giới hạn bị từ chối an toàn.

#### Extraction

- Missing field trả `null`/missing state.
- Không tự suy đoán field critical.
- Source mapping không bị mất.
- Conflict giữa hai nguồn sinh warning.

#### Matching

- Candidate vi phạm hard constraint bị loại.
- Unknown không được coi là pass.
- Ranking có lý do và source.
- Model hãng xác nhận được lưu riêng với model dự kiến.
- Range/model/quantity/option mismatch sinh warning.

#### Pricing

- Quantity × selling price.
- GP = selling price − cost price.
- GP% xử lý selling price bằng 0.
- VAT và grand total đúng boundary case.
- Làm tròn tiền theo rule đã chốt, không để mỗi layer tự làm tròn khác nhau.

#### Approval/export

- Không export khi thiếu approval critical.
- Approval lưu who/when/what.
- Excel mở được.
- PDF không vỡ layout trên fixture chuẩn.
- Công thức/chuỗi bắt đầu bằng `=`, `+`, `-`, `@` được neutralize khi cần.

### 10.3. Verification commands dự kiến

Vì cả hai tầng đều là .NET (backend Web API + frontend Blazor WebAssembly), toàn bộ
verification dùng `dotnet`. **Không dùng `npm`** trong V0 (đây là điểm khác biệt so
với bản roadmap trước, vốn giả định frontend Node riêng).

Backend:

```bash
cd backend
dotnet restore
dotnet build --no-restore
dotnet test --no-build            # Domain + Application + Api integration tests
```

Frontend (Blazor WebAssembly):

```bash
cd frontend
dotnet restore
dotnet build --no-restore
dotnet test --no-build            # bUnit component tests trong frontend-tests/
```

Hoặc build cả solution một lần nếu dùng một file `.sln` chung:

```bash
dotnet restore RfqCopilot.sln
dotnet build --no-restore RfqCopilot.sln
dotnet test --no-build RfqCopilot.sln
```

Browser acceptance test (do frontend agent chọn công cụ) chạy trên SPA đã publish,
kiểm tra các state: loading, empty, error, missing, unverified, conflict,
approval-blocked. Kèm screenshot evidence theo Frontend Definition of Done.

Contract check: mỗi khi backend đổi OpenAPI, phải cập nhật `docs/contracts/` và
frontend agent xác nhận trước khi tích hợp (xem AGENTS.md Mục 6).

---

## 11. Kế hoạch triển khai 16 tuần

Các tuần dưới đây là timebox định hướng, không phải cam kết lịch cứng. Với khoảng 5 giờ AI-assisted coding mỗi tuần, deliverable có thể trượt sang tuần kế tiếp; **Exit gate, không phải số tuần, là điều kiện chuyển bước**. Khi thiếu thời gian, phải giảm scope theo thứ tự ưu tiên thay vì bỏ test, source mapping, approval hoặc security gate.

### Tuần 1 — Scope, policy và baseline

**Deliverables**

- `docs/product/v0-scope.md`
- `docs/security/data-classification.md`
- `docs/security/data-egress-policy-v0.md`
- `docs/domain/pressure-dp-required-fields-v0.yaml`
- `evals/acceptance-criteria.md`
- Template quotation và nguồn catalog được lập danh mục.

**Công việc**

- Chốt data classification và cloud/local policy.
- Chốt 4 model series hoặc thu hẹp hơn.
- Chốt critical fields.
- Chốt người dùng, role và deployment target.
- Đo baseline làm quotation thủ công trên một vài case.
- Chốt acceptance thresholds.

**Exit gate**

- Không còn unknown P0 về data-egress.
- Có template và nguồn ground truth.
- Có owner phê duyệt correctness.

### Tuần 2 — Schema và catalog subset

**Deliverables**

- Schema pressure/DP.
- Catalog subset version đầu tiên.
- Required field definitions.
- Source reference model.

**Exit gate**

- Tối thiểu một workflow pressure/DP được mô tả rõ.
- Catalog có version/effective date/source.

### Tuần 3 — Order-code spike

**Deliverables**

- Grammar/rule subset.
- Validator thử nghiệm.
- Positive/negative fixtures.
- Decision: full order code hay model family + option.

**Exit gate**

- Hard constraint không cho invalid candidate qua.
- Unknown được phân loại đúng.
- Có người xác nhận ground truth.

Nếu gate thất bại, giảm scope ngay; không tiếp tục xây ranking engine đầy đủ.

### Tuần 4 — Concierge ingestion

**Deliverables**

- PDF text extraction có page source.
- Excel extraction có sheet/cell source.
- JSON structured extraction.
- Null/missing/unverified states.

**Exit gate**

- Chạy được trên fixture đầu tiên.
- Không bịa critical field.

### Tuần 5 — Missing information và correction loop

**Deliverables**

- Rule missing-field detection.
- Correction UI hoặc correction file workflow.
- Correction log.
- Error handling cho corrupted/unsupported file.

**Exit gate**

- Mỗi critical field có source hoặc warning.
- Người dùng sửa được extraction mà không mất source gốc.

### Tuần 6 — Candidate và pricing prototype

**Deliverables**

- Candidate recommendation có source/lý do.
- Hard constraints.
- Pricing/GP calculator deterministic.
- Vendor quote extraction thử nghiệm.

**Exit gate**

- Calculation tests đạt 100%.
- Candidate invalid bị loại.

### Tuần 7 — OCR feasibility và prototype gate

**Deliverables**

- OCR spike local.
- Confidence/error classification.
- Fallback manual input.
- Prototype correction log.

**Exit gate**

- Quyết định scan có vào pilot hay không.
- Nếu OCR không đạt, V0 chính thức ưu tiên PDF text + Excel.
- Có bằng chứng workflow giảm thao tác.

### Tuần 8 — App skeleton hai tầng (API + SPA)

**Deliverables**

- Backend: ASP.NET Core Web API skeleton (`backend/src/RfqCopilot.Api`) với
  OpenAPI/Swagger bật, một endpoint tạo RFQ + upload file trả JSON theo contract.
- Frontend: Blazor WebAssembly skeleton (`frontend/RfqCopilot.Web`) gọi API, có
  màn hình tạo RFQ/upload file với loading/empty/error state.
- Contract đầu tiên trong `docs/contracts/` (OpenAPI + fixture request/response).
- SQLite hoặc PostgreSQL theo deployment decision.
- Basic domain/application structure ở backend.
- CORS + auth tối thiểu cho phép SPA gọi API.

**Exit gate**

- Tạo RFQ và upload file được end-to-end qua SPA → API → DB.
- Contract được commit vào `docs/contracts/` trước khi frontend tích hợp.
- Không có Worker/queue/microservice thừa (tách tầng = 2 deployable, không phải nhiều service).

### Tuần 9 — State, source và correction persistence

**Deliverables**

- Document/Requirement/ExtractedField persistence.
- Source mapping persistence.
- Correction persistence.
- Catalog/rule version reference.

**Exit gate**

- Đóng/mở lại RFQ không mất trạng thái.
- Correction và source hiển thị được.

### Tuần 10 — Matching và approval state

**Deliverables**

- Candidate matching.
- matched/unmatched/unknown.
- Approval workflow cho model/order code.
- Warning mismatch.

**Exit gate**

- Không thể đánh dấu verified nếu thiếu source/approval.
- Có audit event khi sửa/approve.

### Tuần 11 — Vendor quotation ingestion

**Deliverables**

- VendorQuote/VendorQuoteLine.
- So sánh RFQ, selected model và vendor quote.
- Warning quantity/range/model/option mismatch.

**Exit gate**

- Model hãng xác nhận được lưu tách biệt.
- Mismatch không bị che giấu.

### Tuần 12 — Pricing và commercial approval

**Deliverables**

- Cost price.
- Factor.
- Selling price.
- GP/GP%.
- VAT/total.
- Commercial approval.

**Exit gate**

- Boundary tests đạt.
- Selling price bằng 0 không làm crash hoặc chia cho 0.
- User phải approve critical commercial fields.

### Tuần 13 — Excel/PDF export

**Deliverables**

- Excel template export.
- PDF render/export.
- Formula-injection protection.
- Approval gate trước export.
- Layout regression fixture.

**Exit gate**

- Excel mở được.
- PDF đạt template review.
- Export chưa được approve bị chặn.

### Tuần 14 — Security, backup và hardening

**Deliverables**

- Role checks.
- File upload limits.
- Sensitive log review.
- Backup/restore procedure.
- Retention decision.
- Audit review.

**Exit gate**

- Có test restore.
- Không có critical approval bypass trong integration test.

### Tuần 15 — Internal pilot

**Deliverables**

- 5–10 RFQ/fixture phù hợp policy.
- Founder + 1–2 internal users.
- Evaluation report.
- Correction log.
- Time baseline vs tool-assisted.

**Exit gate**

- Đo đủ metric.
- Ghi rõ lỗi critical và lỗi không critical.
- Người dùng có thể hoàn thành workflow mà không dựng lại từ đầu.

### Tuần 16 — Go / Adjust / Harden / Stop

**Deliverables**

- `evals/reports/internal-pilot-v0.md`
- `docs/decision-log/gate-v0.md`
- KPI before/after.
- Evidence-based backlog.

**Decision rules**

#### Go

Chỉ khi:

- Không có fabricated critical field.
- Calculation pass rate 100%.
- Approval gate không bị bypass.
- Template output đạt.
- Có cải thiện thời gian hoặc giảm nhập lại.
- Ít nhất một internal reviewer muốn dùng lại.

#### Adjust

Khi workflow có giá trị nhưng schema, catalog, model scope hoặc UX chưa phù hợp.

#### Harden

Khi workflow có giá trị nhưng cần tăng security, stability, backup, audit hoặc PDF/OCR quality.

#### Stop/Pause

Khi không có usage lặp lại, không xác minh được order code, data policy không cho phép hoặc rủi ro lớn hơn giá trị.

---

## 12. Phân bổ thời gian và ngân sách

### 12.1. Nguồn lực lý thuyết

- 16 tuần × 12 giờ/tuần = **192 giờ**.
- Theo cadence ban đầu:
  - Ground truth/workflow review: 32 giờ.
  - AI-assisted coding: 80 giờ.
  - Test/evaluation: 32 giờ.
  - Domain/output review: 32 giờ.
  - Documentation/decision log: 16 giờ.

Vì vậy, coding thực tế chỉ khoảng 80 giờ. Đây là lý do phải cắt kiến trúc và ưu tiên prove-first.

**Cảnh báo đánh đổi khi tách 2 tầng (cập nhật 20/07/2026):** Quyết định tách backend
Web API và frontend Blazor WebAssembly (Mục 7) làm tăng bề mặt công việc so với một
app server-render đơn: phải duy trì OpenAPI contract, xử lý CORS/auth giữa hai domain,
serialize/deserialize DTO hai phía, và viết thêm bộ test frontend (bUnit + browser
acceptance) bên cạnh test backend. Với chỉ ~80 giờ coding, phần "chống lãng phí" này
là rủi ro tiến độ thật, **không phải chi phí bằng 0**. Hai biện pháp giảm thiểu:

1. **Tận dụng 2 agent song song.** Backend agent (Claude Opus 4.8) và frontend agent
   (GPT-5.6-SOL) chạy trên contract chung nên phần lớn giờ coding của hai tầng diễn ra
   song song, không cộng tuyến tính vào 80 giờ của founder. Founder chủ yếu tốn giờ cho
   review, contract approval và acceptance — không phải tự viết cả hai tầng.
2. **Giữ nguyên kỷ luật prove-first.** Concierge prototype (tuần 4–7) vẫn phải chứng
   minh workflow trước khi dựng skeleton 2 tầng (tuần 8). Nếu đến tuần 7 giờ công đã
   cạn hoặc workflow chưa chứng minh được giá trị, **fallback là gộp về một ASP.NET Core
   app server-render** thay vì cố hoàn thành cả hai tầng — đây là quyết định giảm scope
   hợp lệ, cần ghi vào decision log.

Nói thẳng: nếu mục tiêu tối thượng là chứng minh workflow trong giới hạn 192 giờ, một
app đơn tầng sẽ nhanh hơn. Việc tách 2 tầng chỉ đáng làm vì (a) founder muốn một web
sản phẩm thật có thể phát triển tiếp, và (b) mô hình 2 agent hấp thụ được phần lớn chi
phí tách tầng. Nếu một trong hai điều kiện này không còn đúng, nên xét lại quyết định.

### 12.2. Ngân sách

- Trần tháng: 500.000 VNĐ.
- 16 tuần tương đương khoảng 4 tháng: trần lý thuyết 2.000.000 VNĐ.

Ngân sách cần bao gồm hoặc xác nhận rõ:

- LLM/API usage.
- OCR nếu dùng dịch vụ trả phí.
- Storage/backup.
- Hosting nếu không chạy local.
- Domain/TLS nếu có.
- Chi phí thư viện có license.

Nếu chọn cloud extraction, phải ước tính bằng fixture đại diện và xác nhận tổng chi phí thực tế vẫn dưới 500.000 VNĐ/tháng. Nếu OCR/extraction chạy local, ghi riêng chi phí API xấp xỉ 0 và chi phí vận hành/phần cứng liên quan để tránh hiểu sai cơ cấu ngân sách.

Không giả định rằng mọi công cụ miễn phí đều phù hợp về license, chất lượng hoặc data policy.

---

## 13. Risk register

| Risk | Priority | Tác động | Mitigation |
|---|---|---|---|
| Dữ liệu nhạy cảm đi ra cloud | P0 | Rò rỉ thương mại/vi phạm policy | Data-egress gate, local OCR, redaction |
| Order code sai | P0 | Sai sản phẩm/báo giá | Validator, source, hard constraints, human approval |
| Over-building | P0 | Hết giờ trước khi chứng minh value | Một app, sync, không Worker/queue |
| Không có threshold | P0 | Gate cảm tính | Acceptance criteria trong tuần 1 |
| Catalog/price drift | P1 | Không tái lập được kết quả | Version/effective date/source |
| Thiếu auth/role | P1 | User không phù hợp sửa/approve | Viewer/Operator/Approver/Admin |
| Mất dữ liệu | P1 | Không khôi phục RFQ/quotation | Backup, restore test, retention |
| OCR scan kém | P1 | Extraction sai silently | Feasibility spike, confidence, manual fallback |
| PDF layout lỗi | P1 | Output không dùng được | Template fixture, render regression |
| LLM/model drift | P1 | Kết quả thay đổi | Pin model/schema/prompt version |
| Audit không đủ | P2 | Không giải thích được thay đổi | Append-only events, who/when/old/new |
| Corpus quá nhỏ | P2 | Đánh giá quá tự tin | Ghi rõ giới hạn; tăng corpus trước khi mở rộng |
| Upload độc hại/không hợp lệ | P1 | Crash hoặc rò dữ liệu | Size/type validation, sandbox/lifecycle |
| Sai quy tắc làm tròn | P1 | Sai GP/total | Chốt money policy và test boundary |

---

## 14. Các quyết định founder phải chốt trong tuần 1

1. Giá hãng, giá bán, tên khách hàng có được phép gửi tới cloud LLM/OCR không?
2. Nếu không, có chấp nhận local processing và chất lượng thấp hơn không?
3. V0 có xác minh full order code hay chỉ model family + option?
4. Bốn model series cụ thể là gì?
5. Nguồn nào là source of truth cho catalog/order code?
6. Ai là domain approver cho model/order code?
7. Ai approve cost, factor, selling price và commercial terms?
8. Ứng dụng chạy trên máy cá nhân, localhost hay server nội bộ?
9. Có bao nhiêu RFQ/tháng và số user đồng thời?
10. Template Excel/PDF công ty đã sẵn sàng chưa?
11. Scan/OCR có bắt buộc trong V0 hay chỉ là thử nghiệm?
12. Dữ liệu giữ trong bao lâu và quy trình xóa thế nào?
13. Có policy công ty về cloud AI, backup, encryption và access logging không?
14. Threshold đề xuất có được chấp nhận không, đặc biệt 0 fabricated critical field, calculation 100%, model family ≥95% và order-code top-1 ≥80% trên eligible cases?

---

## 15. Definition of Done cho V0

V0 chỉ được coi là hoàn thành khi:

- PDF text và Excel trong phạm vi hỗ trợ được ingest.
- Scan đã có quyết định rõ: hỗ trợ trong pilot hoặc loại khỏi V0.
- Critical requirements có source hoặc được đánh dấu unverified/missing.
- Missing fields được phát hiện bằng rule.
- Invalid candidates bị loại bởi hard constraints.
- Unknown không được coi là pass.
- Model/order code được user approve trước khi dùng.
- Vendor quotation được parse và so sánh, với mismatch warning.
- Cost, factor, selling price, GP và commercial terms được user approve.
- Calculation tests đạt 100%.
- Không export khi thiếu critical approval.
- Excel/PDF khớp template đã duyệt.
- Excel formula injection được neutralize.
- Approval, correction và audit tests pass.
- Backup/restore procedure đã được kiểm tra.
- Pilot có founder và ít nhất một internal reviewer.
- Có số liệu so sánh với baseline thủ công.
- Ít nhất một user hoàn thành workflow mà không phải dựng lại quotation từ đầu.
- Có decision record Go/Adjust/Harden/Stop dựa trên dữ liệu.

---

## 16. Việc cần làm trong 7 ngày đầu

### Ngày 1–2

- Thu thập một template quotation Excel/PDF thật đã được phép sử dụng.
- Liệt kê bốn model series có khả năng đưa vào V0.
- Thu thập catalog/GS/source document.
- Lập danh sách data elements nhạy cảm.

### Ngày 3

- Chốt local/cloud policy.
- Chốt deployment target.
- Chốt người approve model và giá.

### Ngày 4

- Viết required fields YAML.
- Chọn 3 ground-truth cases đầu tiên.
- Đo thời gian làm báo giá thủ công.

### Ngày 5

- Viết order-code spike nhỏ cho một model series.
- Tạo positive và negative cases.
- Quyết định full order code hay model family + option.

### Ngày 6–7

- Chốt acceptance criteria.
- Kiểm tra OCR trên một vài scan đại diện.
- Chọn PDF/Excel trước hay đưa scan vào pilot.
- Chỉ sau các bước trên mới tạo app skeleton.

---

## 17. Kết luận cuối cùng

Bản kế hoạch cuối cùng giữ lại các điểm mạnh của kế hoạch ban đầu:

- Workflow-first.
- Ground truth trước code.
- Human-in-the-loop.
- Source traceability.
- Deterministic pricing.
- Approval gate.
- Audit và evaluation.

Đồng thời đã bổ sung các điểm Claude Opus 4.8 xác định là thiếu:

- Data-egress policy.
- Order-code validation spike.
- Catalog/price-list versioning.
- Authentication/authorization.
- Backup/restore/retention.
- OCR feasibility gate.
- Acceptance thresholds.
- LLM/model version pinning.
- Audit detail.
- Over-building control.

**Khuyến nghị triển khai:** Conditional Go.

**Điều kiện bắt buộc:** Không code lớn trước khi P0-1, P0-2, P0-3 và P0-4 được chốt. Nếu order-code hoặc data policy không đạt, phải thu hẹp scope thay vì cố xây một hệ thống lớn hơn.

**Mục tiêu đúng của V0:** Không phải “AI tự làm báo giá”, mà là “trợ lý nội bộ có bằng chứng nguồn, rule kiểm soát, tính toán không sai và bắt buộc con người chịu trách nhiệm phê duyệt”.

---

## Phụ lục A — Dấu vết phối hợp model

- **GPT-5.6-SOL:** tổng hợp yêu cầu gốc, đánh giá tính khả thi theo nguồn lực, thiết kế roadmap hợp nhất và kiểm tra tính nhất quán giữa scope, architecture, acceptance criteria và Definition of Done.
- **Claude Opus 4.8:** review độc lập về product/engineering/security; xác định các P0/P1/P2 gap, đề xuất threshold, kiến trúc V0 mỏng hơn, order-code spike, data-egress gate và backup/retention.
- **Kết quả cuối:** các đề xuất được hợp nhất, các điểm chưa có dữ liệu được đánh dấu là assumption/unknown, không coi recommendation là fact.

## Phụ lục B — Các file dự kiến cần tạo trong repository

```text
docs/
├── product/v0-scope.md
├── security/data-classification.md
├── security/data-egress-policy-v0.md
├── operations/backup-restore-v0.md
├── domain/pressure-dp-required-fields-v0.yaml
├── domain/pressure-dp-schema-v0.md
└── decision-log/gate-v0.md

catalog/
├── sample-data/pressure-dp-v0.csv
└── versions/

evals/
├── acceptance-criteria.md
├── fixtures/
├── expected/
├── manifest.json
└── reports/internal-pilot-v0.md

templates/
└── quotation-template-v0.xlsx

backend/
├── src/
│   ├── RfqCopilot.Domain/
│   ├── RfqCopilot.Application/
│   ├── RfqCopilot.Infrastructure/
│   └── RfqCopilot.Api/
└── tests/
    ├── RfqCopilot.Domain.Tests/
    ├── RfqCopilot.Application.Tests/
    └── RfqCopilot.Api.IntegrationTests/

frontend/
└── RfqCopilot.Web/          # Blazor WebAssembly SPA

frontend-tests/
└── RfqCopilot.Web.Tests/    # bUnit component tests

docs/contracts/              # OpenAPI + fixture chia sẻ giữa 2 agent
```

Lưu ý: cây `backend/` + `frontend/` + `frontend-tests/` ở trên thay thế cấu trúc
`src/` + `tests/` phẳng của bản roadmap trước, để khớp với quyết định tách hai tầng
.NET ở Mục 7.1 (cập nhật 20/07/2026).

---

*Hết tài liệu.*
