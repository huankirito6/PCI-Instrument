# Phase 0 Acceptance Criteria — Internal RFQ & Quotation Copilot V0

**Status:** Draft baseline
**Purpose:** Chốt tiêu chí để không đánh giá prototype bằng cảm nhận.

## 1. Scope acceptance

- [ ] V0 chỉ bao gồm pressure/differential-pressure Yokogawa.
- [ ] Initial model-series scope được ghi rõ: EJA110E, EJA430E, EJA530E, EJAC80E.
- [ ] Input scope là PDF, Excel và scan.
- [ ] Output scope là Excel và PDF.
- [ ] Hãng không có account/quyền truy cập.
- [ ] V0 không tự gửi email và không tự duyệt báo giá.
- [ ] V0 chưa thương mại hóa.

## 2. Data and security acceptance

- [ ] Dữ liệu RFQ/quotation được phân loại trước khi xử lý.
- [ ] Không có raw sensitive fixture trong Git.
- [ ] Không có customer identifier, vendor quotation hoặc cost/selling price trong application logs.
- [ ] Dữ liệu truyền qua TLS/HTTPS trong môi trường có network.
- [ ] File/database có cơ chế encryption at rest phù hợp môi trường.
- [ ] Provider AI/OCR có policy được kiểm tra về training use, retention, region và deletion trước khi dùng sensitive pilot.
- [ ] Có test hoặc checklist cho hard delete.
- [ ] Có audit event cho view/edit/approve/reject/export quan trọng.

## 3. Requirement extraction acceptance

- [ ] PDF text extraction giữ được page source.
- [ ] Excel extraction giữ được sheet và cell source.
- [ ] Scan không đọc được phải báo lỗi/action rõ ràng.
- [ ] Field có source phải lưu quote/location khi có thể.
- [ ] Field không có source không được đánh dấu verified.
- [ ] Field thiếu phải là `missing`/`null`, không suy đoán.
- [ ] Field conflicting phải được gắn `conflicting` và đưa vào review.

## 4. Missing-information acceptance

- [ ] Required fields được version hóa theo product family.
- [ ] Missing unconditional field được phát hiện.
- [ ] Conditional requirement chỉ kích hoạt khi điều kiện đúng.
- [ ] Clarification draft chỉ dùng dữ liệu đã biết và field thiếu.
- [ ] Clarification draft không tự bịa model, specification, price hoặc certification.

## 5. Model/order-code acceptance

- [ ] Candidate phải thuộc catalog subset đã phê duyệt.
- [ ] Hard-constraint violation luôn bị loại.
- [ ] `unknown` không được tính như `pass`.
- [ ] Candidate hiển thị matched/unmatched/unknown.
- [ ] Candidate hiển thị rule pass/fail và source.
- [ ] Model/order code chỉ là draft/candidate cho đến khi user approve.
- [ ] Vendor-confirmed model được lưu riêng với proposed model.
- [ ] Mismatch giữa proposed/vendor-confirmed model, range hoặc quantity tạo warning.

## 6. Commercial calculation acceptance

- [ ] Giá vốn có thể nhập thủ công.
- [ ] Hệ số có thể nhập thủ công.
- [ ] Giá bán có thể nhập/xác nhận thủ công.
- [ ] Có thể chọn dùng nguyên giá hãng.
- [ ] `Line total = Quantity * Selling price`.
- [ ] `GP = Selling price - Cost price`.
- [ ] `GP% = (Selling price - Cost price) / Selling price * 100%`.
- [ ] Chia cho 0 hoặc dữ liệu thiếu phải trả validation error, không trả giá trị giả.
- [ ] Subtotal, VAT và grand total được kiểm thử boundary.
- [ ] Rounding policy được ghi rõ trước khi productionize.

## 7. Human approval and export acceptance

Không được export nếu thiếu xác nhận đối với các trường phù hợp:

- [ ] Customer/end-user.
- [ ] Model/order code.
- [ ] Range/measurement type.
- [ ] Quantity.
- [ ] Vendor cost/quotation values.
- [ ] Factor/selling price/GP.
- [ ] VAT/totals.
- [ ] Delivery/payment/warranty.
- [ ] CO/CQ/CW/documents.

- [ ] Excel mở được bằng Excel/LibreOffice.
- [ ] PDF không vỡ layout trên fixture chuẩn.
- [ ] Formula injection từ dữ liệu ngoài được neutralize.
- [ ] Export version được lưu cùng audit metadata.
- [ ] Export không tự gửi tài liệu.

## 8. Pilot success baseline

Với tập fixture đã được founder duyệt:

- [ ] Không có critical field bị bịa.
- [ ] Model family/candidate hợp lệ đạt 100% trên các case đã có ground truth.
- [ ] Tính toán thương mại đúng 100% trên test cases.
- [ ] Báo giá đủ trường và đúng mẫu.
- [ ] Người dùng không phải nhập lại toàn bộ file từ đầu.
- [ ] Có đo trước/sau về active time, số lỗi nhập liệu và số lần sửa.
- [ ] Có ít nhất một internal reviewer ngoài founder kiểm tra output trước khi tuyên bố pilot pass.

## 9. Required evidence before Phase 0 exit

- `docs/product/v0-scope.md` được founder review.
- `docs/security/data-classification.md` được review.
- Catalog/GS nguồn cho phạm vi model được xác định.
- Template Excel canonical được xác định.
- Cách render PDF được xác định ở mức spike hoặc có acceptance sample.
- Tối thiểu 3 fixture ground truth hoặc kế hoạch tạo synthetic fixture được chấp nhận.
- Các open decisions được ghi trong decision log.
