# Data Classification & Handling — V0

**Status:** Phase 0 baseline
**Scope:** Internal RFQ, customer/end-user, vendor quotation and company quotation workflows

## 1. Declared operating context

- Báo giá và thông tin RFQ là dữ liệu mật nội bộ công ty.
- Người dùng đã xác nhận công cụ được phép dùng AI/cloud bên ngoài để phân tích, với yêu cầu bảo vệ và mã hóa dữ liệu.
- V0 chỉ dùng nội bộ và chưa được thương mại hóa.

Việc cho phép AI/cloud trong prototype không thay thế việc kiểm tra điều khoản nhà cung cấp, retention, training use, access logging và hợp đồng/DPA trước khi đưa dữ liệu thật vào production.

## 2. Classification levels

| Level | Ví dụ | Quy tắc V0 |
|---|---|---|
| `public` | Catalog/datasheet công khai, tài liệu kỹ thuật công khai | Có thể dùng làm catalog/source sau khi kiểm tra nguồn và phiên bản |
| `internal` | Template báo giá, workflow, rule nội bộ không chứa giá/khách hàng | Chỉ người dùng nội bộ được truy cập |
| `confidential` | RFQ, thông tin end user, vendor quotation, company quotation | Mã hóa khi truyền/lưu; hạn chế quyền; không ghi vào log |
| `restricted` | Giá vốn, discount, selling price, account/payment data, customer identifiers | Chỉ xử lý khi cần; redaction/pseudonymization trước AI nếu không ảnh hưởng kết quả; audit bắt buộc |

## 3. Handling rules

### Storage

- File và database phải được lưu trong vùng private/internal.
- Không commit file RFQ, báo giá, giá vốn, giá bán hoặc thông tin khách hàng vào Git.
- Tên file người dùng không được dùng trực tiếp làm storage path.
- Có retention và hard-delete policy trước pilot nội bộ.

### Transport

- Dùng TLS/HTTPS cho API và object storage.
- Không truyền dữ liệu confidential/restricted qua endpoint không được phê duyệt.

### AI/OCR processing

- Chỉ gửi phần dữ liệu tối thiểu cần để trích xuất/chuẩn hóa.
- Redact hoặc pseudonymize tên, email, số điện thoại, mã dự án và identifier không cần cho tác vụ.
- Không gửi bảng giá/discount nếu tác vụ không cần đến chúng.
- Provider phải được kiểm tra về: không dùng API data để train, retention, region, subprocessor, deletion và breach notification.
- Không coi payload đã mã hóa ở storage là đã được mã hóa end-to-end trong lúc model inference; API provider vẫn có thể thấy nội dung plaintext trong thời gian xử lý.

### Logs and telemetry

Không log:

- Nội dung RFQ hoặc báo giá.
- Customer/end-user identifiers.
- Giá vốn, discount, giá bán hoặc số tài khoản.
- Raw OCR text.

Telemetry chỉ nên chứa ID nội bộ, hash, trạng thái, error code, latency và cost aggregate.

### Access

- Chỉ user nội bộ được cấp quyền mới xem RFQ/quotation.
- Ban giám đốc có quyền approval theo workflow nội bộ.
- Hãng không có account và không có quyền truy cập hệ thống.
- Mọi view/edit/approve/reject/export quan trọng phải có audit event.

## 4. Current data-egress decision

**Declared decision:** Có thể dùng AI/cloud bên ngoài để phân tích trong phạm vi phát triển nội bộ, nhưng trước khi dùng dữ liệu confidential/restricted thật trên production cần ghi nhận và kiểm tra provider policy/contract.

**Required evidence before real sensitive pilot:**

- Provider/API terms và data-training policy.
- Retention/zero-retention setting.
- Region/subprocessor review.
- Encryption in transit/at rest.
- Redaction strategy.
- Deletion test.
- Internal approval record.

## 5. Incident response minimum

Khi phát hiện gửi nhầm dữ liệu hoặc truy cập trái phép:

1. Dừng provider/job liên quan.
2. Ghi nhận thời gian, loại dữ liệu và phạm vi ảnh hưởng; không copy dữ liệu nhạy cảm vào ticket/log.
3. Xác định provider retention/deletion action.
4. Báo người phụ trách nội bộ.
5. Rotate credential nếu có nguy cơ lộ.
6. Tạo regression/security test trước khi bật lại workflow.

## 6. Open decisions

- Ai là người phê duyệt provider AI/OCR.
- Thời gian retention mặc định.
- Vị trí lưu trữ/region.
- Cách mã hóa key management.
- Có được dùng dữ liệu giá thật trong AI prompt hay chỉ dùng local/private processing.
- Quy trình hard delete và backup deletion.
