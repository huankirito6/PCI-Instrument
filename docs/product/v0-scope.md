# V0 Scope — Internal RFQ & Quotation Copilot

**Status:** Draft baseline for Phase 0
**Owner:** Founder/domain owner
**Scope type:** Internal-only prototype/application

## 1. Product goal

Giảm thời gian thao tác và lỗi nhập liệu trong quy trình xử lý RFQ và lập báo giá cho thiết bị đo áp suất/chênh áp Yokogawa, nhưng vẫn giữ human approval trước khi chọn model/order code và xuất báo giá.

## 2. Initial users and approvers

- Người dùng chính: sales engineer xử lý field instruments.
- Người dùng thử bổ sung: 1–2 người nội bộ thuộc Sales/Kỹ thuật.
- Ban giám đốc: người duyệt cuối cùng cho báo giá.
- Hãng: không có tài khoản và không truy cập hệ thống; người dùng vẫn gửi yêu cầu cho hãng thủ công.

## 3. V0 product boundary

### In scope

- Tạo hồ sơ RFQ nội bộ.
- Nhận PDF, Excel và file scan.
- Trích xuất requirement có source mapping.
- Chuẩn hóa các field pressure/differential-pressure.
- Phát hiện field bắt buộc còn thiếu.
- Tra candidate từ catalog subset đã được phê duyệt.
- Hiển thị matched/unmatched/unknown, lý do và nguồn.
- Cho người dùng chọn, sửa, reject hoặc approve model/candidate.
- Đọc báo giá hãng từ PDF/Excel/scan.
- So sánh model dự kiến với model hãng xác nhận.
- Nhập thủ công giá vốn, giá bán, hệ số và VAT.
- Tính thành tiền, GP, GP%, subtotal, VAT và grand total.
- Điền form báo giá công ty.
- Checklist human approval trước khi export.
- Xuất Excel và PDF.
- Audit log cho sửa, approve, reject và export.
- Mã hóa khi truyền/lưu theo khả năng môi trường nội bộ.

### Out of scope

- Tự gửi email hoặc tự gửi file cho khách hàng/hãng/ban giám đốc.
- Hãng truy cập hoặc thao tác trong hệ thống.
- Tự động phê duyệt báo giá.
- Tự quyết định giá bán, hệ số hoặc điều kiện thương mại.
- Tích hợp ERP/CRM.
- Multi-tenant SaaS.
- Mobile app.
- Toàn bộ Yokogawa hoặc nhiều product family ngoài pressure/DP.
- Thương mại hóa ngoài công ty.
- Dự đoán giá, tồn kho hoặc lead time nếu không có nguồn chính thức.

## 4. Initial product scope

### Manufacturer

- Yokogawa-first.

### Product series

- EJA110E.
- EJA430E.
- EJA530E.
- EJAC80E.

Các series trên là phạm vi khởi đầu theo khảo sát; khả năng hỗ trợ từng order-code option phải được xác nhận bằng catalog/GS được phê duyệt, không được suy đoán từ tên model.

### Input

- PDF RFQ/specification.
- Excel bảng thông số hoặc báo giá.
- Scan/ảnh scan.

### Output

- Excel theo template báo giá công ty.
- PDF báo giá hoàn chỉnh.
- Báo cáo field thiếu, candidate comparison và approval checklist có thể được lưu kèm hồ sơ RFQ.

## 5. V0 workflow

```text
Create RFQ
-> upload PDF/Excel/scan
-> extract and normalize
-> review source mapping
-> detect missing fields
-> draft candidate/model
-> human review
-> user sends model request to vendor manually
-> upload vendor quotation
-> extract and compare
-> enter cost/factor/selling price manually
-> calculate GP/VAT/totals
-> mandatory approval checklist
-> export Excel/PDF
-> user sends/submits manually
```

## 6. Critical human-approved fields

Không được export nếu chưa có xác nhận phù hợp đối với:

- Customer/end-user information.
- Product family and model/order code.
- Measurement type and range.
- Quantity.
- Vendor-confirmed model/quotation values.
- Cost price, factor, selling price and GP.
- VAT, totals and rounding.
- Delivery time.
- Payment terms.
- Warranty.
- CO/CQ/CW and other document requirements.

## 7. Source and status semantics

Mỗi field kỹ thuật quan trọng phải có source hoặc trạng thái rõ ràng:

```text
provided | extracted | missing | unknown | unverified | approved | rejected | conflicting
```

- `missing`: không có giá trị trong input.
- `unknown`: chưa thể xác định từ dữ liệu hiện có.
- `unverified`: có giá trị nhưng chưa có source/kiểm tra đủ.
- `conflicting`: nhiều nguồn đưa ra giá trị khác nhau.
- `approved`: người có thẩm quyền đã xác nhận.

## 8. Success definition for V0

V0 đạt mục tiêu ban đầu khi trên fixture đã được duyệt:

1. Candidate/model đề xuất không vi phạm hard constraint và có source.
2. Critical field thiếu không bị bịa.
3. Người dùng có thể sửa và approve kết quả.
4. Báo giá Excel/PDF đúng template và đủ field bắt buộc.
5. Thành tiền, VAT, subtotal, grand total, GP và GP% tính đúng.
6. Hệ thống chặn export khi thiếu human approval.
7. Người dùng không phải nhập lại toàn bộ báo giá từ đầu.

## 9. Open decisions before Phase 1 exit

- Catalog/GS nào được phê duyệt cho từng series.
- Danh sách option/order-code rule có nguồn.
- Template Excel canonical và cách render PDF.
- Nhà cung cấp AI/OCR, retention và không dùng dữ liệu API để train.
- Chính sách mã hóa và vị trí lưu dữ liệu nội bộ.
- Quy tắc làm tròn tiền/VAT chính thức của công ty.
