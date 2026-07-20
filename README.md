# Internal RFQ & Quotation Copilot V0

Công cụ nội bộ hỗ trợ xử lý RFQ và lập báo giá cho thiết bị đo áp suất/chênh áp Yokogawa.

## Phạm vi hiện tại

- Yokogawa-first.
- Ưu tiên EJA110E, EJA430E, EJA530E và EJAC80E.
- Input: PDF, Excel, scan.
- Output: Excel và PDF theo mẫu công ty.
- Người dùng thử: founder và 1–2 người dùng nội bộ.
- Chưa thương mại hóa và chưa tự động gửi email.

## Trạng thái

Dự án đang ở **Phase 0 — Scope, security and acceptance baseline**. Chưa có mã nguồn ứng dụng production.

## Tài liệu chính

- `docs/product/v0-scope.md`
- `docs/security/data-classification.md`
- `docs/domain/pressure-dp-required-fields-v0.yaml`
- `evals/acceptance-criteria.md`
- `docs/decision-log/0001-phase0-baseline.md`
- `.hermes/plans/2026-07-17_234716-rfq-copilot-v0-roadmap.md`
- `tong-hop-khao-sat-v1.md`

## Nguyên tắc an toàn

- AI/OCR chỉ trích xuất, chuẩn hóa và soạn thảo.
- Rule/catalog và con người quyết định các trường kỹ thuật/thương mại quan trọng.
- Dữ liệu thiếu phải giữ là `null`, `missing` hoặc `unverified`; không được suy đoán.
- Không ghi dữ liệu báo giá nhạy cảm vào log.
- Không xuất báo giá khi các trường critical chưa được người dùng xác nhận.

## Quy trình Git (theo dõi và backup)

- `main`: nhánh ổn định, chỉ nhận code đã kiểm tra.
- `feat/<mô-tả>`, `fix/<mô-tả>`, `docs/<mô-tả>`: nhánh làm việc.
- Mỗi thay đổi được commit nhỏ với message rõ ràng để dễ theo dõi.
- Đẩy thường xuyên lên GitHub để có bản backup; nếu code sai có thể quay lại commit trước.
