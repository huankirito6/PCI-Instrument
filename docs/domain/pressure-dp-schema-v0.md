# Pressure / Differential-Pressure Schema — V0

**Status:** Draft (Phase 1)
**Source of truth for fields:** `docs/domain/pressure-dp-required-fields-v0.yaml`
**Owner:** Founder/domain owner

Tài liệu này mô tả canonical schema mà công cụ dùng để chuẩn hóa yêu cầu RFQ cho thiết bị đo áp suất/chênh áp. File YAML là bản máy đọc; file này là bản người đọc, giải thích ý nghĩa, đơn vị và quy tắc chuẩn hóa.

> Nguyên tắc bắt buộc: dữ liệu thiếu giữ nguyên `missing`/`null`. Không suy đoán giá trị critical. Mọi order-code/option phải dựa trên catalog/GS được phê duyệt, không suy ra từ tên model.

## 1. Nhóm field

### 1.1 Nhận dạng thiết bị

| Field | Kiểu | Bắt buộc | Critical | Ghi chú |
|---|---|---|---|---|
| `product_family` | enum | có | có | `pressure_transmitter` hoặc `differential_pressure_transmitter` |
| `model_series` | string | có | có | Chỉ là candidate cho tới khi có catalog + human approve |
| `measurement_type` | enum | có | có | gauge / absolute / differential / unknown |

### 1.2 Dải đo và áp suất

| Field | Kiểu | Đơn vị chuẩn hóa | Ghi chú |
|---|---|---|---|
| `measurement_range_min` | decimal | theo `measurement_range_unit` | Giữ cả raw text |
| `measurement_range_max` | decimal | theo `measurement_range_unit` | Giữ cả raw text |
| `measurement_range_unit` | engineering_unit | kPa, MPa, bar, mbar, mmH2O... | Chuẩn hóa deterministic |
| `range_reference` | enum | — | gauge / absolute / differential / unknown — không tự đổi gauge↔absolute |
| `process_pressure_min/max` | decimal | theo `process_pressure_unit` | Điều kiện vận hành |
| `static_pressure_or_overpressure` | decimal_or_text | — | Quan trọng với DP |

### 1.3 Nhiệt độ

| Field | Đơn vị chuẩn hóa | Ghi chú |
|---|---|---|
| `process_temperature_min/max` | °C | Chuyển °F↔°C khi cần, test boundary |
| `process_temperature_unit` | °C / °F | Giữ đơn vị gốc |
| `ambient_temperature_min/max` | °C | Không critical |

### 1.4 Tín hiệu và nguồn

| Field | Ghi chú |
|---|---|
| `output_signal` | Ví dụ `4-20mA`, `4-20mA_HART`. Không suy diễn HART chỉ từ 4-20mA |
| `communication_protocol` | HART / BRAIN / FOUNDATION Fieldbus / PROFIBUS... theo nguồn |
| `power_supply` | Ví dụ dải VDC |

### 1.5 Kết nối và vật liệu

| Field | Ghi chú |
|---|---|
| `process_connection` | Ren/flange, ví dụ `1/2 NPT`, `G1/2`. Chỉ merge synonym khi taxonomy xác nhận |
| `electrical_connection` | Ví dụ `1/2 NPT female` |
| `wetted_material` | Ví dụ `316L SST`, `Hastelloy C-276` |
| `diaphragm_or_seal_type` | Loại màng/seal |
| `remote_seal` | boolean_or_unknown — kích hoạt yêu cầu bổ sung |

### 1.6 Chứng nhận và môi trường

| Field | Ghi chú |
|---|---|
| `hazardous_area_requirement` | Không coi "Ex" chung chung là "Ex ia" |
| `certification` | ATEX / IECEx / CSA... theo nguồn |
| `ingress_protection` | IP66/IP67... |
| `display_requirement` | Có/không LCD |

### 1.7 Lắp đặt và phụ kiện

| Field | Ghi chú |
|---|---|
| `mounting_requirement` | Bracket 2-inch pipe... |
| `manifold_and_accessories` | list — manifold, bracket... quyết định bằng rule |
| `calibration_certificate_requirement` | CO/CQ/CW... |
| `quantity` | integer |

## 2. Quy tắc chuẩn hóa deterministic

1. **Đơn vị áp suất:** bar ↔ kPa ↔ MPa ↔ mbar; test boundary (0, giá trị âm nếu có, giá trị lớn).
2. **Nhiệt độ:** °C ↔ °F với `F = C*9/5 + 32`; test boundary.
3. **Chuỗi tín hiệu:** chuẩn hóa `4 to 20mA`, `4-20 mA`, `4...20mA` về `4-20mA`.
4. **Kết nối:** `1/2 NPT`, `1/2"NPT`, `NPT 1/2` gộp; `G 1/2`, `G1/2` gộp; chỉ khi taxonomy xác nhận tương đương.
5. **Đơn vị lạ:** giữ raw value và tạo warning, không tự loại bỏ.
6. **Gauge/absolute:** không tự đổi nếu nguồn không nói rõ.

## 3. Trạng thái field

```text
provided | extracted | missing | unknown | unverified | approved | rejected | conflicting
```

Mỗi field lưu song song: `raw_text`, `normalized_value`, `unit`, `status`, `source` (file/page/sheet/cell/quote).

## 4. Conditional requirements (Phase 1 khởi đầu)

- Nếu `hazardous_area_requirement` có → yêu cầu `certification`.
- Nếu `remote_seal = true` → yêu cầu `diaphragm_or_seal_type`, `process_temperature_max`, `manifold_and_accessories`.
- Nếu `measurement_type = differential_pressure` → yêu cầu `static_pressure_or_overpressure`.

## 5. Việc cần founder xác nhận trước Phase 2

- [ ] Danh sách đơn vị và synonym chính thức cho từng field.
- [ ] Bộ order-code option hợp lệ cho mỗi series (EJA110E/430E/530E/EJAC80E) từ catalog/GS được phê duyệt.
- [ ] Field nào bắt buộc tuyệt đối vs field nào có thể clarify sau.
- [ ] Quy tắc merge synonym kết nối/vật liệu.
