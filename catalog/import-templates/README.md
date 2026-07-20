# Catalog Import Template

**Status:** Draft (Phase 1)

Đây là hợp đồng import catalog. Mỗi dòng mô tả MỘT thuộc tính của MỘT part number. Một part number có nhiều dòng (mỗi thuộc tính một dòng).

## Cột bắt buộc

| Cột | Ý nghĩa | Bắt buộc |
|---|---|---|
| `Manufacturer` | Hãng, ví dụ Yokogawa | có |
| `ProductFamily` | `pressure_transmitter` / `differential_pressure_transmitter` | có |
| `ModelSeries` | EJA110E, EJA430E, EJA530E, EJAC80E | có |
| `PartNumber` | Order code / part number cụ thể | có |
| `Description` | Mô tả ngắn | có |
| `AttributeKey` | Khóa thuộc tính, khớp field trong schema | có |
| `AttributeValue` | Giá trị | có |
| `Unit` | Đơn vị nếu có | không |
| `SourceDocument` | Tên tài liệu/GS nguồn | có |
| `SourcePage` | Trang/mục nguồn | không |

## Quy tắc validate khi import (Phase 5)

1. Thiếu cột bắt buộc → reject dòng, báo lỗi rõ.
2. `AttributeKey` không thuộc schema → cảnh báo, không âm thầm bỏ qua.
3. Duplicate part number → theo chính sách rõ ràng (định nghĩa sau).
4. Một dòng lỗi không được publish catalog dở dang.
5. Cross-tenant isolation (khi có nhiều workspace).

## Lưu ý bảo mật/IP

- Chỉ import catalog/GS công khai hoặc tài liệu founder được phép dùng.
- Không import bảng giá/discount nội bộ vào bản dùng chung.
- Mỗi thuộc tính phải có nguồn (`SourceDocument`) để mọi candidate về sau có citation.
