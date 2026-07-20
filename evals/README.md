# Evaluation Harness

**Status:** Phase 1 scaffold

Thư mục này chứa fixture (đầu vào RFQ) và expected ground truth (đáp án chuẩn do chuyên gia duyệt) để đánh giá công cụ một cách khách quan thay vì cảm nhận.

## Cấu trúc

```text
evals/
├── manifest.json                 # Danh sách fixture
├── acceptance-criteria.md        # Tiêu chí nghiệm thu Phase 0
├── fixtures/<ID>/rfq.md          # Đầu vào RFQ (synthetic hoặc đã ẩn danh)
├── expected/<ID>.json            # Đáp án chuẩn theo schema
└── scripts/validate_fixtures.py  # Kiểm tra cấu trúc fixture
```

## Nguyên tắc dữ liệu

- Chỉ chứa fixture synthetic hoặc dữ liệu đã được founder cho phép và ẩn danh.
- Không commit tên khách hàng thật, giá, discount hoặc báo giá hãng.
- Mỗi fixture có đầu vào + expected ground truth được người kiểm tra duyệt.

## Chạy kiểm tra cấu trúc

```bash
python evals/scripts/validate_fixtures.py
```

Phase 1 chỉ kiểm tra cấu trúc (fixture có đủ input + expected, field key thuộc schema, status hợp lệ, field `missing` có value null). Khi có engine trích xuất (Phase 4+), sẽ bổ sung chế độ chấm điểm so sánh output với expected.

## Metric dự kiến (khi có engine)

| Metric | Định nghĩa |
|---|---|
| Field precision | Field trích xuất đúng / tổng field trích xuất |
| Field recall | Field đúng tìm được / tổng field đúng trong ground truth |
| Source accuracy | Field có citation đúng vị trí / field có citation |
| Missing recall | Field thiếu phát hiện được / tổng field thiếu theo expert |
| Unsupported assertion rate | Khẳng định không nguồn / tổng khẳng định (mục tiêu: 0% cho critical) |
| Top-3 candidate recall | Model đúng nằm trong top 3 |
