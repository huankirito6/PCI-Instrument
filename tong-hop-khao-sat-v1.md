# Tổng hợp khảo sát Field Instrument RFQ & Quotation Copilot — V1

**Phạm vi tài liệu:** Tổng hợp 30 câu trả lời của người dùng cùng các yêu cầu được rút ra từ file kế hoạch, bảng thông số thiết bị và báo giá mẫu.

**Nguyên tắc bảo mật:** Tài liệu này không lặp lại tên khách hàng, địa chỉ, email, số điện thoại, số tài khoản, mã báo giá hoặc mức giá cụ thể trong các file mẫu đã cung cấp.

---

## I. Tổng hợp 30 câu trả lời

### 1. Nhóm thiết bị đang xử lý

Thiết bị đo áp suất và chênh áp Yokogawa.

### 2. Model/model series thường gặp

- EJA110E
- EJA430E
- EJA530E
- EJAC80E

### 3. Số lượng RFQ mỗi tháng

Khoảng **4–6 RFQ/tháng**.

### 4. Số line item trong một RFQ

- RFQ nhỏ: khoảng **1–5 line item**.
- RFQ lớn: có thể khoảng **10 line item**.

### 5. Thời gian xử lý

- Active engineer time: khoảng **1 giờ/RFQ**.
- Thời gian chờ ban giám đốc duyệt: khoảng **nửa ngày đến một ngày**.

### 6. Workflow hiện tại

1. Nhận yêu cầu/RFQ và thông tin kỹ thuật.
2. Đọc catalog.
3. Chọn model/order code phù hợp.
4. Gửi model cùng thông tin end user cho hãng kiểm tra và báo giá.
5. Nhận báo giá của hãng.
6. Nhập dữ liệu vào form báo giá công ty, gồm:
   - Tên công ty thương mại hoặc end user mua hàng.
   - Địa chỉ.
   - Người nhận.
   - Email hoặc số điện thoại liên hệ.
   - Mã báo giá.
   - Ngày báo giá.
   - Thời hạn hiệu lực.
   - Thiết bị.
   - Model.
   - Dải đo.
   - Hãng và xuất xứ.
   - Số lượng.
   - Đơn giá và thành tiền.
   - Giấy tờ kèm theo như CO, CQ, CW, CO thương mại và các hồ sơ liên quan.
   - Thời gian giao hàng.
   - Điều kiện thanh toán.
   - Thời gian bảo hành.
   - Người lập báo giá.
7. Gửi ban giám đốc duyệt.
8. Sau khi duyệt, người dùng tự gửi tài liệu cho bên liên quan.

### 7. Ba công đoạn mất thời gian nhất

1. Chờ duyệt.
2. Điền thông tin vào báo giá.
3. Chọn model.

### 8. Thông tin khách hàng thường thiếu

Không có một nhóm thiếu cố định. Tùy từng RFQ, người dùng phải điều tra hoặc yêu cầu bổ sung một số thông tin theo bảng thông số thiết bị.

### 9. Thông tin kỹ thuật thường phải kiểm tra

- Dải đo.
- Loại phép đo/loại áp suất cần đo.
- Vị trí lắp đặt.
- Điều kiện process.
- Các trường kỹ thuật liên quan trong bảng thông số thiết bị.

Cần xây một schema riêng cho pressure/differential-pressure thay vì dùng nguyên trạng bảng thông số flowmeter.

### 10. Lỗi/rework thường gặp

Model được hãng kiểm tra trước nên phần chọn model có thêm một lớp xác nhận. Khi lập báo giá, lỗi thường gặp chủ yếu là **nhập sai hoặc thiếu thông tin**.

### 11. Đầu ra mong muốn trong phiên bản đầu

Công cụ cần hỗ trợ toàn bộ các nhóm chức năng sau:

1. Đọc yêu cầu, phát hiện dữ liệu thiếu và đề xuất model/order code.
2. Tạo yêu cầu gửi hãng kiểm tra/báo giá, kèm thông tin end user.
3. Đọc báo giá của hãng và điền form báo giá công ty.
4. Kiểm tra lỗi và xuất báo giá để trình ban giám đốc duyệt.

Công cụ chỉ tạo tài liệu hoặc bản nháp; người dùng vẫn tự gửi.

### 12. Quyền sử dụng dữ liệu

- Các loại dữ liệu liên quan được phép sử dụng để phát triển nội bộ.
- Báo giá là dữ liệu mật, chỉ được dùng nội bộ công ty.

### 13. AI/cloud

Dữ liệu được phép cho AI/cloud bên ngoài phân tích, với yêu cầu phải bảo vệ và mã hóa dữ liệu.

### 14. Quyền phát triển và thương mại hóa

- Được phát triển để sử dụng nội bộ.
- Chưa được phép thương mại hóa hoặc cung cấp ra ngoài công ty.

### 15. Dữ liệu mẫu có thể chuẩn bị ngay

Khoảng **1–3 bộ dữ liệu mẫu**.

Một bộ nên gồm, nếu có:

- RFQ hoặc bảng thông số đầu vào.
- Catalog/GS liên quan.
- Model/order code đã chọn đúng.
- Yêu cầu hoặc email gửi hãng.
- Báo giá của hãng.
- Báo giá công ty đã hoàn thiện.
- Các lần sửa và kết quả duyệt.

### 16. Người kiểm tra kết quả

Chỉ người nội bộ công ty:

- Sales.
- Kỹ thuật.
- Ban giám đốc.

Hãng không có tài khoản và không can thiệp trực tiếp vào hệ thống. Khi cần, model vẫn được người dùng gửi hãng kiểm tra theo cách thủ công bên ngoài hệ thống.

### 17. Người dùng thử ban đầu

Người dùng chính và **1–2 người khác** trong nội bộ công ty.

### 18. Thời gian có thể dành cho dự án

Khoảng **12 giờ/tuần**.

### 19. Năng lực kỹ thuật

- Có nền tảng C#/.NET.
- Sử dụng AI để hỗ trợ phát triển.

### 20. Ngân sách thử nghiệm

Dưới **500.000 VNĐ/tháng** cho AI API/OCR, hosting, database và lưu trữ.

### 21. Phương pháp phát triển

AI coding model/agent hỗ trợ viết phần lớn mã nguồn. Người dùng chịu trách nhiệm:

- Kiểm tra yêu cầu.
- Kiểm tra hành vi thực tế.
- Kiểm tra tính đúng của model/order code.
- Kiểm tra file báo giá.
- Chấp nhận hoặc từ chối kết quả.

### 22. File đầu vào

- PDF.
- Excel.
- File scan/ảnh scan.

### 23. File đầu ra

- Excel.
- PDF.

### 24. Hai cách hình thành giá bán

1. Người dùng nhập hệ số để nhân với giá hãng/giá vốn và cần tính GP.
2. Người dùng có thể lấy nguyên giá hãng làm giá bán.

### 25. Công thức GP và quyền nhập giá

- Hệ số và giá cuối cùng được người dùng nhập/xác nhận thủ công.
- Công thức GP:

```text
GP% = (Giá bán - Giá vốn) / Giá bán × 100%
```

Công cụ chỉ tính toán và cảnh báo; không tự quyết định giá bán.

### 26. Người duyệt cuối cùng

Ban giám đốc duyệt toàn bộ báo giá.

### 27. Vai trò Sales/Kỹ thuật trước khi trình duyệt

Tùy từng báo giá. Có trường hợp Sales/Kỹ thuật cần kiểm tra hoặc góp ý trước; không phải mọi báo giá đều có cùng một luồng duyệt cố định.

### 28. Gửi email/tài liệu

Công cụ chỉ tạo file. Người dùng tự kiểm tra và gửi; V1 không tự động gửi email cho khách hàng, hãng hoặc ban giám đốc.

### 29. Định nghĩa thành công

Phiên bản đầu được xem là thành công khi:

1. Chọn đúng model.
2. Tạo file báo giá đúng mẫu công ty.

### 30. Human approval

Trước khi xuất Excel/PDF, bắt buộc người dùng kiểm tra và xác nhận thủ công các trường quan trọng, gồm:

- Model/order code.
- Dải đo.
- Số lượng.
- Giá hãng/giá vốn.
- Hệ số.
- Giá bán.
- GP.
- VAT.
- Thời gian giao hàng.
- Điều kiện thanh toán.
- Bảo hành.
- Giấy tờ CO/CQ/CW và tài liệu liên quan.
- Thông tin khách hàng/end user.

---

## II. Định nghĩa sản phẩm sau khảo sát

### Tên làm việc

**Internal RFQ & Quotation Copilot for Field Instruments**

### Người dùng ban đầu

- Sales engineer đang xử lý field instruments.
- 1–2 người dùng nội bộ thuộc Sales/Kỹ thuật.
- Ban giám đốc là người duyệt cuối.

### Phạm vi V0

- Chỉ dùng nội bộ công ty.
- Yokogawa-first.
- Chỉ tập trung thiết bị đo áp suất/chênh áp.
- Bắt đầu từ EJA110E, EJA430E, EJA530E và EJAC80E.
- Không hỗ trợ toàn bộ field instruments hoặc mọi hãng trong V0.
- Không thương mại hóa khi chưa có phê duyệt riêng.

### Bài toán chính

Giảm thao tác thủ công và lỗi nhập liệu trong luồng:

```text
RFQ/bảng thông số
→ phát hiện thông tin thiếu
→ đề xuất model/order code
→ tạo nội dung gửi hãng
→ nhận và đọc báo giá hãng
→ nhập giá/hệ số
→ tính GP
→ điền form báo giá công ty
→ kiểm tra
→ xuất Excel/PDF
→ trình ban giám đốc duyệt
```

---

## III. Workflow V0 đề xuất

### Bước 1 — Tạo hồ sơ RFQ

Người dùng tạo RFQ và tải lên:

- PDF.
- Excel.
- File scan.

### Bước 2 — Trích xuất và chuẩn hóa

AI/OCR trích xuất:

- Thông tin khách hàng/end user.
- Line item.
- Loại thiết bị.
- Thông số process.
- Dải đo.
- Loại phép đo.
- Vị trí lắp đặt.
- Số lượng.
- Các yêu cầu chứng từ và thương mại nếu có.

Mỗi trường cần giữ nguồn gốc: file, trang, sheet, cell hoặc vùng scan.

### Bước 3 — Phát hiện thông tin thiếu

Rule engine so sánh dữ liệu với schema pressure/DP và tạo:

- Danh sách trường thiếu.
- Lý do cần trường đó.
- Nội dung câu hỏi bổ sung.

AI không được tự suy đoán giá trị kỹ thuật còn thiếu.

### Bước 4 — Đề xuất model/order code

- Tìm candidate trong catalog/GS đã được phê duyệt.
- Loại candidate vi phạm điều kiện cứng.
- Hiển thị lý do chọn/loại.
- Dẫn nguồn catalog.
- Người dùng bắt buộc kiểm tra và xác nhận.

### Bước 5 — Tạo nội dung gửi hãng

Công cụ chuẩn bị file hoặc bản nháp gồm:

- Thông tin end user cần thiết.
- Danh sách thiết bị.
- Model/order code dự kiến.
- Dải đo.
- Số lượng.
- Các câu hỏi cần hãng xác nhận.

Người dùng tự gửi hãng; hệ thống không tự gửi.

### Bước 6 — Đọc báo giá hãng

Khi người dùng tải báo giá hãng lên, công cụ trích xuất:

- Model/order code hãng xác nhận.
- Mô tả.
- Dải đo.
- Số lượng.
- Đơn giá/giá vốn.
- Thời gian giao hàng.
- Hiệu lực.
- Điều kiện thanh toán.
- Bảo hành.
- Tài liệu/chứng chỉ kèm theo.

### Bước 7 — Tính giá bán và GP

Người dùng chọn một trong hai cách:

1. Nhập hệ số nhân trên giá hãng/giá vốn.
2. Dùng nguyên giá hãng.

Hệ thống tính:

```text
Giá bán = Giá vốn × Hệ số
GP = Giá bán - Giá vốn
GP% = (Giá bán - Giá vốn) / Giá bán × 100%
```

Giá và hệ số phải được người dùng xác nhận thủ công.

### Bước 8 — Tạo báo giá công ty

Công cụ điền đúng template công ty, gồm bốn nhóm chính:

1. Header và thông tin khách hàng.
2. Danh mục thiết bị.
3. Chứng từ/tài liệu.
4. Điều kiện thương mại, thanh toán, giao hàng và bảo hành.

### Bước 9 — Kiểm tra bắt buộc

Hiển thị checklist xác nhận các trường critical trước khi cho phép xuất file.

### Bước 10 — Xuất tài liệu

- Xuất Excel theo đúng form công ty.
- Xuất PDF hoàn chỉnh.
- Người dùng tự gửi và trình duyệt.

---

## IV. Chức năng bắt buộc của V0

1. Upload PDF, Excel và scan.
2. OCR và document parsing.
3. Trích xuất dữ liệu có nguồn dẫn.
4. Schema riêng cho pressure/DP.
5. Phát hiện thông tin thiếu.
6. Đề xuất model/order code có giải thích và nguồn catalog.
7. Tạo nội dung để người dùng gửi hãng thủ công.
8. Đọc báo giá hãng.
9. Cho phép nhập hệ số/giá thủ công.
10. Tính GP và GP%.
11. Điền template báo giá công ty.
12. Kiểm tra lỗi nhập liệu.
13. Checklist xác nhận bắt buộc.
14. Xuất Excel và PDF.
15. Phân quyền nội bộ.
16. Mã hóa dữ liệu khi lưu và truyền.
17. Audit log cho các lần sửa, xác nhận và xuất file.

---

## V. Ngoài phạm vi V0

- Không tự gửi email.
- Không cho hãng truy cập hệ thống.
- Không tự động phê duyệt báo giá.
- Không tự quyết định giá bán hoặc hệ số.
- Không tự cam kết model/order code khi chưa có người kiểm tra.
- Không hỗ trợ toàn bộ Yokogawa hoặc nhiều hãng ngay từ đầu.
- Không thương mại hóa ra ngoài công ty.
- Không tự động hóa hoàn toàn thời gian chờ ban giám đốc duyệt; chỉ có thể hỗ trợ tạo file đúng và theo dõi trạng thái.

---

## VI. Schema pressure/DP cần xây

Danh sách khởi đầu cần được xác nhận bằng các catalog và case thực tế:

- Measurement type: gauge, absolute hoặc differential pressure.
- Application/medium.
- Range minimum/maximum và đơn vị.
- Working/process pressure.
- Static pressure/overpressure.
- Process temperature.
- Ambient temperature.
- Vị trí và điều kiện lắp đặt.
- Output signal.
- Communication protocol.
- Power supply.
- Process connection.
- Electrical connection.
- Wetted material.
- Diaphragm/seal type.
- Remote seal/capillary nếu có.
- Display.
- Ingress protection.
- Hazardous-area requirement.
- Certification.
- Mounting.
- Manifold và phụ kiện.
- Calibration/certificate requirement.
- Quantity.

Mỗi trường phải có trạng thái:

```text
provided | extracted | missing | unverified | approved | rejected
```

---

## VII. Bảo mật và ranh giới dữ liệu

### Yêu cầu đã xác nhận

- Báo giá là dữ liệu mật nội bộ.
- Được dùng AI/cloud bên ngoài để phân tích.
- Dữ liệu phải được mã hóa.
- Sản phẩm hiện chỉ được phát triển nội bộ.

### Lưu ý kỹ thuật quan trọng

Mã hóa khi lưu và truyền không đồng nghĩa nhà cung cấp AI không nhìn thấy nội dung trong lúc inference. Khi gọi API thông thường, payload phải được giải mã để model xử lý.

Do đó cần tối thiểu:

1. TLS khi truyền dữ liệu.
2. Mã hóa database/object storage.
3. Redact hoặc pseudonymize tên khách hàng, email, số điện thoại, mã dự án và thông tin không cần thiết trước khi gửi AI.
4. Chọn nhà cung cấp có chính sách không dùng dữ liệu API để huấn luyện.
5. Ưu tiên zero-retention hoặc thời gian lưu ngắn.
6. Không ghi nội dung báo giá vào application log.
7. Phân quyền theo vai trò nội bộ.
8. Audit đầy đủ các lần xem, sửa và xuất file.
9. Không sử dụng dữ liệu cho thương mại hóa khi chưa có phê duyệt bằng văn bản.

---

## VIII. Nguồn lực và cách triển khai phù hợp

- Thời gian: 12 giờ/tuần.
- Ngân sách: dưới 500.000 VNĐ/tháng.
- Nền tảng kỹ thuật: C#/.NET.
- AI agent viết phần lớn mã nguồn; người dùng là product owner, domain reviewer và acceptance tester.

Cách triển khai phù hợp:

- ASP.NET Core modular monolith.
- Database local hoặc PostgreSQL chi phí thấp.
- Lưu file local/private trong giai đoạn đầu hoặc object storage được mã hóa.
- Chỉ gọi OCR/LLM khi cần để kiểm soát chi phí.
- Không xây multi-tenant SaaS hoặc hạ tầng phức tạp ở V0.
- Bắt đầu bằng một workflow end-to-end nhỏ thay vì xây toàn bộ kế hoạch 24 tuần cùng lúc.

---

## IX. Tiêu chí nghiệm thu đề xuất

Hai tiêu chí gốc của người dùng:

1. Chọn đúng model.
2. File báo giá đúng mẫu.

Để có thể kiểm thử, nên chuyển thành các tiêu chí định lượng sau:

- Model family đúng: 100% trên tập fixture đã duyệt.
- Order code/candidate đúng hoặc hợp lệ: người dùng chấp nhận sau review.
- Không có thông số critical bị bịa.
- Mỗi thông số kỹ thuật dùng để chọn model có nguồn catalog/RFQ.
- Báo giá Excel/PDF có đầy đủ trường bắt buộc và đúng layout.
- Công thức thành tiền, VAT, tổng và GP tính đúng.
- Không cho xuất nếu thiếu xác nhận các trường critical.
- Không lặp lại thao tác nhập các thông tin đã trích xuất đúng.
- Lỗi nhập liệu giảm so với quy trình thủ công.

Vì hiện chỉ có 1–3 bộ mẫu, kết quả mới có giá trị chứng minh kỹ thuật ban đầu, chưa đủ để kết luận độ chính xác ổn định. Sau prototype nên tăng lên ít nhất 10–20 fixture đã ẩn danh hoặc synthetic.

---

## X. Các điểm cần chốt trước khi bắt đầu code

1. Xác nhận chính xác model series và tài liệu catalog/GS được dùng cho từng series.
2. Tạo bảng thông số riêng cho pressure/DP.
3. Xác định bộ trường bắt buộc và các rule chọn model/order code.
4. Chuẩn bị 1–3 bộ dữ liệu mẫu end-to-end đầu tiên.
5. Tạo đáp án chuẩn cho từng bộ mẫu.
6. Xác định chính xác template Excel nguồn để giữ đúng format khi xuất.
7. Xác định cách chuyển Excel sang PDF mà không vỡ layout.
8. Chốt nhà cung cấp AI/OCR và chính sách bảo mật dữ liệu API.
9. Chốt trạng thái workflow linh hoạt trước ban giám đốc duyệt.
10. Chốt tiêu chí thế nào được xem là “chọn model đúng”: đúng family, đúng candidate hay đúng toàn bộ order code.

---

## XI. Kết luận

Sản phẩm phù hợp nhất ở thời điểm hiện tại không phải là một SaaS lớn, mà là một **copilot nội bộ hẹp cho RFQ và lập báo giá thiết bị đo áp suất/chênh áp Yokogawa**.

Giá trị đầu tiên cần chứng minh là:

1. Đọc PDF/Excel/scan và chuẩn hóa dữ liệu.
2. Phát hiện thông tin còn thiếu.
3. Đề xuất đúng model/order code có nguồn dẫn.
4. Đọc báo giá hãng.
5. Giảm nhập liệu thủ công.
6. Tính giá và GP dưới sự kiểm soát của người dùng.
7. Xuất đúng form Excel/PDF của công ty.
8. Bắt buộc con người kiểm tra trước khi trình ban giám đốc.

V0 chỉ dùng nội bộ, với người dùng chính cùng 1–2 đồng nghiệp. Hãng không truy cập hệ thống; mọi trao đổi/gửi file vẫn do người dùng thực hiện thủ công.

---

## XII. Kế hoạch triển khai V0 và mục tiêu từng giai đoạn

### 1. Giả định triển khai

Kế hoạch này được điều chỉnh theo nguồn lực hiện tại:

- Thời gian: **12 giờ/tuần**.
- Ngân sách: **dưới 500.000 VNĐ/tháng**.
- Người viết phần lớn mã nguồn: AI coding model/agent.
- Người chịu trách nhiệm kiểm tra: founder/domain expert.
- Người dùng thử: founder và 1–2 người nội bộ.
- Dữ liệu mẫu ban đầu: 1–3 bộ; cần bổ sung fixture synthetic nếu thiếu dữ liệu thật.
- Phạm vi: pressure/differential-pressure Yokogawa, ưu tiên EJA110E, EJA430E, EJA530E và EJAC80E.
- Hình thức: internal tool trước, chưa thương mại hóa.

Không nên cố xây toàn bộ hệ thống SaaS hoặc toàn bộ kế hoạch 24 tuần ngay từ đầu. Mục tiêu trước tiên là chứng minh một workflow end-to-end có giá trị.

### 2. Tổng quan roadmap

| Giai đoạn | Thời gian | Mục tiêu chính |
|---|---:|---|
| Giai đoạn 0 | Tuần 1 | Chốt phạm vi, dữ liệu, bảo mật và tiêu chí đúng |
| Giai đoạn 1 | Tuần 2–3 | Tạo schema, catalog subset và ground truth |
| Giai đoạn 2 | Tuần 4–5 | Dựng concierge prototype để chứng minh AI có thể hỗ trợ |
| Giai đoạn 3 | Tuần 6–7 | Xây nền tảng nội bộ tối thiểu bằng C#/.NET |
| Giai đoạn 4 | Tuần 8–9 | Hoàn thiện upload, đọc tài liệu, OCR và phát hiện thiếu dữ liệu |
| Giai đoạn 5 | Tuần 10–11 | Đề xuất model và đọc báo giá hãng |
| Giai đoạn 6 | Tuần 12–13 | Tính giá/GP và xuất Excel/PDF đúng mẫu |
| Giai đoạn 7 | Tuần 14–15 | Pilot nội bộ, đo KPI và sửa lỗi |
| Giai đoạn 8 | Tuần 16 | Gate quyết định tiếp tục, điều chỉnh hoặc dừng |

---

### Giai đoạn 0 — Chốt phạm vi và điều kiện an toàn

**Thời gian:** Tuần 1

**Mục tiêu:** Xác định chính xác V0 sẽ làm gì, không làm gì, dùng dữ liệu nào và thế nào được xem là kết quả đúng.

**Công việc:**

1. Chốt một workflow duy nhất:

   ```text
   RFQ/thông số → chọn model → đọc báo giá hãng → nhập giá/GP → xuất báo giá
   ```

2. Chốt bốn model series ưu tiên và tài liệu catalog/GS được phép sử dụng.
3. Xác định template Excel và mẫu PDF báo giá công ty.
4. Phân loại dữ liệu: công khai, nội bộ, mật, không được gửi ra ngoài.
5. Chốt chính sách xử lý AI/cloud, mã hóa, log và xóa dữ liệu.
6. Định nghĩa “model đúng”:
   - Đúng loại thiết bị/family.
   - Đúng model series.
   - Đúng order code hoặc candidate hợp lệ để người dùng duyệt.
7. Định nghĩa “báo giá đúng mẫu”:
   - Đủ trường bắt buộc.
   - Đúng bố cục.
   - Tính đúng thành tiền, VAT, tổng tiền và GP.

**Đầu ra:**

- `docs/product/v0-scope.md`
- `docs/security/data-classification.md`
- `docs/domain/pressure-dp-required-fields-v0.yaml`
- `evals/acceptance-criteria.md`

**Cổng quyết định:** Chỉ sang Giai đoạn 1 khi có template báo giá, catalog nguồn và tiêu chí nghiệm thu rõ ràng.

---

### Giai đoạn 1 — Tạo schema, catalog subset và ground truth

**Thời gian:** Tuần 2–3

**Mục tiêu:** Tạo dữ liệu chuẩn để AI và rule engine có thể được kiểm tra; không xây giao diện trước khi có đáp án chuẩn.

**Công việc:**

1. Tạo schema pressure/DP gồm các nhóm:
   - Loại phép đo: gauge/absolute/differential.
   - Dải đo và đơn vị.
   - Process pressure và process temperature.
   - Medium/application.
   - Vị trí lắp đặt.
   - Output/protocol/power supply.
   - Process connection và electrical connection.
   - Wetted material/seal/remote seal.
   - Hazardous area/certification.
   - Display, mounting, accessory và quantity.
2. Chuẩn hóa tên field, đơn vị và synonym thường gặp.
3. Tạo catalog subset chỉ chứa sản phẩm được phép dùng cho V0.
4. Tạo 3 bộ ground truth từ dữ liệu được phép hoặc synthetic.
5. Mỗi bộ phải có:
   - Input RFQ/thông số.
   - Field chuẩn.
   - Field còn thiếu.
   - Model/order code đúng.
   - Candidate bị loại và lý do.
   - Nguồn catalog/datasheet.
   - Giá vốn/giá bán giả lập nếu dùng trong test pricing.
   - Báo giá đầu ra đúng mẫu.
6. Tạo thêm fixture synthetic để đạt tối thiểu 5–10 trường hợp kiểm thử.

**Đầu ra:**

- `docs/domain/pressure-dp-schema-v0.md`
- `catalog/sample-data/pressure-dp-v0.csv`
- `evals/fixtures/`
- `evals/expected/`
- `evals/manifest.json`

**Cổng quyết định:** Không tiếp tục nếu chưa có ít nhất 3 đáp án chuẩn được founder kiểm tra thủ công.

---

### Giai đoạn 2 — Concierge prototype

**Thời gian:** Tuần 4–5

**Mục tiêu:** Kiểm chứng giá trị trước khi đầu tư vào ứng dụng hoàn chỉnh. Prototype có thể chạy bán thủ công nhưng phải tạo được đầu ra giống workflow thật.

**Công việc:**

1. Tạo form hoặc thư mục nhận PDF/Excel/scan.
2. Dùng parser/OCR/LLM để trích xuất dữ liệu vào JSON có schema.
3. Hiển thị hoặc xuất danh sách trường thiếu.
4. Tạo bản nháp model/candidate kèm source.
5. Đọc báo giá hãng mẫu.
6. Cho phép nhập giá vốn, hệ số hoặc chọn dùng nguyên giá hãng.
7. Tính GP/GP%.
8. Điền một workbook báo giá mẫu.
9. Ghi lại mọi chỉnh sửa của founder.

**Đầu ra:**

- Một script/workflow xử lý được 3–5 bộ dữ liệu.
- `research/findings/correction-log.csv`
- `templates/quotation-template-v0.xlsx`
- Báo cáo so sánh thời gian thủ công và thời gian prototype.

**Mục tiêu nghiệm thu:**

- Không bịa field critical.
- Field thiếu được đánh dấu rõ.
- Model đề xuất có source.
- File báo giá có thể mở được và đúng các trường chính.
- Founder xác nhận prototype giúp giảm thao tác nhập liệu.

**Cổng quyết định:** Nếu prototype không giúp chọn model hoặc giảm nhập liệu, phải sửa schema/workflow trước khi xây nền tảng.

---

### Giai đoạn 3 — Xây nền tảng nội bộ tối thiểu

**Thời gian:** Tuần 6–7

**Mục tiêu:** Đưa concierge workflow vào ứng dụng nội bộ nhỏ, có trạng thái xử lý và lịch sử thay đổi.

**Công việc:**

1. Tạo repository và solution C#/.NET.
2. Dựng ASP.NET Core API dạng modular monolith.
3. Tạo module RFQ, Document, Requirement, Catalog, Quote và Audit.
4. Tạo màn hình hoặc API để:
   - Tạo RFQ.
   - Upload tài liệu.
   - Xem trạng thái xử lý.
   - Xem requirement.
   - Xem cảnh báo thiếu dữ liệu.
5. Thêm user nội bộ tối thiểu; chưa cần multi-tenant phức tạp.
6. Thêm mã hóa file/database theo khả năng môi trường.
7. Thêm audit log cho upload, sửa, approve và export.
8. Viết unit test cho state transition, validation và GP calculation.

**Đầu ra:**

- Repository V0 chạy được local.
- `src/` và `tests/` theo cấu trúc .NET.
- `docs/operations/local-development.md`
- `docker-compose.yml` nếu cần database/object storage.

**Mục tiêu nghiệm thu:**

- User nội bộ tạo RFQ và upload file được.
- Không mất file hoặc requirement khi job lỗi.
- Có thể xem lịch sử sửa.
- Test domain chạy ổn định.

---

### Giai đoạn 4 — Upload, đọc tài liệu và phát hiện thiếu dữ liệu

**Thời gian:** Tuần 8–9

**Mục tiêu:** Xử lý được ba loại input chính: PDF, Excel và scan; trích xuất có source để người dùng kiểm tra.

**Công việc:**

1. Tạo adapter parser cho PDF và Excel.
2. Tạo adapter OCR cho file scan.
3. Chuẩn hóa output thành text block, table, page/sheet/cell và quote nguồn.
4. Dùng structured output/JSON Schema cho extraction.
5. Lưu raw value và normalized value song song.
6. Dùng rule engine để phát hiện field bắt buộc còn thiếu.
7. Đánh dấu rõ:
   - `extracted`
   - `missing`
   - `unverified`
   - `approved`
   - `rejected`
8. Tạo bản nháp câu hỏi clarification; không tự bịa giá trị còn thiếu.

**Đầu ra:**

- PDF text extraction.
- Excel sheet/cell extraction.
- Scan OCR cơ bản.
- Requirement review screen hoặc JSON review endpoint.
- Missing-information report.

**Mục tiêu nghiệm thu:**

- Mỗi field quan trọng có source hoặc bị đánh dấu unverified.
- File scan không đọc được phải trả lỗi rõ ràng.
- Field thiếu được phát hiện theo rule.
- Chạy regression trên ít nhất 5 fixture.

**Cổng quyết định:** Chỉ sang matching khi extraction và missing-field đã đủ ổn định; không dùng matching để che lỗi extraction.

---

### Giai đoạn 5 — Đề xuất model và đọc báo giá hãng

**Thời gian:** Tuần 10–11

**Mục tiêu:** Tự động hóa phần tra catalog và nhập lại dữ liệu báo giá hãng, nhưng vẫn giữ người dùng và hãng ở vị trí kiểm soát.

**Công việc:**

1. Import catalog subset từ CSV/XLSX.
2. Xây hard filter cho các trường quan trọng:
   - Product family.
   - Measurement type.
   - Range.
   - Output/protocol.
   - Connection.
   - Certification.
3. Xây soft ranking cho các trường optional.
4. Hiển thị candidate cùng:
   - Matched requirements.
   - Unmatched requirements.
   - Unknown requirements.
   - Rule pass/fail.
   - Source.
5. Cho phép người dùng chọn hoặc từ chối candidate.
6. Đọc báo giá hãng từ PDF/Excel/scan.
7. So sánh model dự kiến với model hãng xác nhận.
8. Gắn cờ khi model, dải đo, số lượng hoặc mô tả không khớp.
9. Không tự gửi file/email cho hãng.

**Đầu ra:**

- Candidate comparison.
- Model/order-code review.
- Vendor quotation extraction.
- Mismatch report.
- Audit record cho quyết định của người dùng.

**Mục tiêu nghiệm thu:**

- Candidate vi phạm hard constraint bị loại.
- Không coi unknown là pass.
- Model hãng xác nhận được lưu độc lập với model dự kiến.
- Người dùng nhìn thấy lý do và nguồn trước khi approve.

---

### Giai đoạn 6 — Tính giá/GP và xuất Excel/PDF

**Thời gian:** Tuần 12–13

**Mục tiêu:** Tạo được báo giá công ty đúng mẫu, giảm việc nhập tay và kiểm soát các lỗi thương mại.

**Công việc:**

1. Tạo form thông tin khách hàng và thông tin báo giá.
2. Import dữ liệu từ báo giá hãng vào quote draft.
3. Cho phép chọn:
   - Dùng nguyên giá hãng.
   - Nhập hệ số nhân.
4. Cho phép nhập thủ công giá vốn, giá bán, hệ số và VAT khi cần.
5. Tính và kiểm tra:

   ```text
   Thành tiền = Số lượng × Đơn giá
   GP = Giá bán - Giá vốn
   GP% = (Giá bán - Giá vốn) / Giá bán × 100%
   Tổng trước VAT
   VAT
   Tổng sau VAT
   ```

6. Tạo checklist xác nhận trước khi xuất.
7. Chặn export nếu thiếu các trường critical chưa xác nhận.
8. Xuất Excel đúng template công ty.
9. Xuất PDF và kiểm tra layout.
10. Neutralize formula injection trong các ô nhận dữ liệu từ file ngoài.

**Đầu ra:**

- Excel báo giá hoàn chỉnh.
- PDF báo giá hoàn chỉnh.
- Validation report.
- Approval checklist.
- Export audit log.

**Mục tiêu nghiệm thu:**

- File Excel mở được bằng Excel/LibreOffice.
- PDF không vỡ bố cục ở các trường hợp mẫu.
- Thành tiền, VAT, tổng tiền và GP tính đúng.
- Không xuất được file nếu chưa xác nhận model, giá, số lượng và điều kiện thương mại.
- File đầu ra không tự gửi đi.

---

### Giai đoạn 7 — Pilot nội bộ và hardening

**Thời gian:** Tuần 14–15

**Mục tiêu:** Kiểm tra công cụ trong workflow thật với founder và 1–2 người nội bộ, đồng thời phát hiện lỗi trước khi mở rộng.

**Công việc:**

1. Chọn 5–10 RFQ/fixture được phép hoặc synthetic.
2. Cho founder xử lý một phần bằng quy trình cũ và một phần bằng công cụ.
3. Cho 1–2 người nội bộ blind-review output nếu có thể.
4. Ghi nhận:
   - Thời gian xử lý.
   - Số lần sửa model.
   - Số lỗi nhập liệu.
   - Số field thiếu bị bỏ sót.
   - Số lần sửa báo giá.
   - Thời gian từ input đến file sẵn sàng trình duyệt.
5. Tạo fixture regression cho từng lỗi.
6. Sửa theo thứ tự:
   - Rule/catalog.
   - Schema/normalization.
   - Parser/OCR.
   - UI/review.
   - Prompt/model sau cùng.
7. Kiểm tra quyền truy cập, mã hóa, log và xóa dữ liệu.
8. Kiểm tra backup/export dữ liệu nếu dùng database.

**Đầu ra:**

- `evals/reports/internal-pilot-v0.md`
- Dashboard hoặc CSV KPI.
- Regression fixtures.
- Bug/correction log.
- Security checklist.

**Mục tiêu nghiệm thu đề xuất:**

- Model family đúng trên 100% fixture đã được duyệt.
- Không có critical field bị bịa.
- Báo giá đúng form ở các case đã kiểm thử.
- Tính toán thương mại đúng 100% trên test cases.
- Giảm đáng kể thao tác nhập liệu so với cách thủ công.
- Người dùng chấp nhận output sau review, không phải làm lại toàn bộ.

---

### Giai đoạn 8 — Gate quyết định tiếp theo

**Thời gian:** Tuần 16

**Mục tiêu:** Quyết định dựa trên số liệu, không tiếp tục xây thêm tính năng chỉ vì prototype đã chạy được.

**Đánh giá ba câu hỏi:**

1. **Có giúp founder không?**
   - Thời gian xử lý giảm chưa?
   - Nhập liệu có ít hơn không?
   - Model và báo giá có đúng hơn không?
2. **Đồng nghiệp có tin dùng không?**
   - Có chấp nhận output sau review không?
   - Có tiếp tục dùng cho RFQ tiếp theo không?
3. **Công ty có muốn dùng lâu dài không?**
   - Có yêu cầu bảo mật/hạ tầng riêng không?
   - Có cần tích hợp email, ERP hoặc quy trình duyệt không?
   - Có người chịu trách nhiệm vận hành không?

**Quyết định:**

- **Go:** tiếp tục làm sâu pressure/DP và tăng dữ liệu fixture.
- **Adjust:** giữ workflow nhưng đổi model family, template hoặc cách xử lý.
- **Harden:** kết quả hữu ích nhưng còn vấn đề bảo mật, ổn định hoặc layout.
- **Stop/Pause:** không có usage lặp lại, model không đáng tin hoặc không được phê duyệt dữ liệu.

**Đầu ra:**

- `docs/decision-log/gate-v0.md`
- Báo cáo KPI trước/sau.
- Backlog quý tiếp theo chỉ gồm các hạng mục có bằng chứng.

---

## XIII. Nhịp làm việc mỗi tuần

Với 12 giờ/tuần, nên chia như sau:

| Hoạt động | Giờ/tuần | Mục tiêu |
|---|---:|---|
| Kiểm tra workflow và dữ liệu | 2 giờ | Bổ sung ground truth, review lỗi |
| Code với AI agent | 5 giờ | Làm một vertical slice nhỏ |
| Test/evaluation | 2 giờ | Chạy unit, integration và fixture |
| Review domain/output | 2 giờ | Kiểm tra model, báo giá, nguồn |
| Ghi tài liệu/decision log | 1 giờ | Không mất context và quyết định |

### Quy tắc mỗi tuần

1. Chọn tối đa một customer outcome và một engineering outcome.
2. Mỗi tính năng quan trọng phải có test hoặc fixture trước.
3. Lỗi AI phải trở thành regression case.
4. Không dùng prompt để sửa lỗi thuộc về schema, catalog hoặc rule.
5. Không thêm product family thứ hai trước khi pressure/DP đạt tiêu chí.
6. Không tự động gửi báo giá, email hoặc quyết định giá trong V0.

---

## XIV. Backlog ưu tiên sau khi thêm kế hoạch

### Must have trong 16 tuần

- PDF/Excel/scan input.
- Pressure/DP schema.
- Source mapping.
- Missing-field rules.
- Catalog subset.
- Candidate/model review.
- Đọc báo giá hãng.
- Nhập giá/hệ số thủ công.
- GP calculation.
- Excel/PDF export.
- Mandatory human approval.
- Audit log.
- Mã hóa và phân quyền nội bộ.

### Should have nếu còn thời gian

- Bản nháp câu hỏi clarification.
- So sánh model dự kiến với model hãng xác nhận.
- Correction log export.
- Template versioning.
- Dashboard thời gian và lỗi.

### Chưa làm trong V0

- Tự gửi email.
- Hãng truy cập hệ thống.
- Tự động duyệt báo giá.
- Tự động quyết định giá/hệ số.
- ERP/CRM integration.
- Multi-tenant SaaS.
- Mobile app.
- Hỗ trợ toàn bộ Yokogawa hoặc nhiều hãng.
- Thương mại hóa ra ngoài công ty.
