USE WebsiteDuDoanThuoc;
GO

-- Bo sung mo ta benh cho database hien tai.
UPDATE Benh SET
    MoTa = N'Bệnh nhiễm siêu vi đường hô hấp trên, thường gây sốt nhẹ, đau họng, ho, sổ mũi, hắt hơi và mệt mỏi. Phần lớn trường hợp tự cải thiện sau vài ngày nếu nghỉ ngơi, uống đủ nước và theo dõi triệu chứng.',
    NhomBenh = N'Hô hấp',
    NgayCapNhat = GETDATE()
WHERE TenBenh = N'Cảm cúm';

UPDATE Benh SET
    MoTa = N'Tình trạng đau hoặc căng tức vùng đầu, có thể liên quan căng thẳng, thiếu ngủ, cảm cúm, đau cơ vùng cổ vai gáy hoặc các nguyên nhân khác. Cần đi khám nếu đau dữ dội đột ngột, kéo dài hoặc kèm dấu hiệu thần kinh.',
    NhomBenh = N'Thần kinh',
    NgayCapNhat = GETDATE()
WHERE TenBenh = N'Đau đầu';

UPDATE Benh SET
    MoTa = N'Tình trạng đi ngoài phân lỏng nhiều lần trong ngày, có thể kèm đau bụng, buồn nôn, mất nước hoặc sốt. Ưu tiên bù nước và theo dõi dấu hiệu mất nước, phân máu hoặc sốt cao.',
    NhomBenh = N'Tiêu hoá',
    NgayCapNhat = GETDATE()
WHERE TenBenh = N'Tiêu chảy';

UPDATE Benh SET
    MoTa = N'Phản ứng quá mẫn biểu hiện bằng ngứa da, nổi mẩn đỏ, mề đay, phát ban, hắt hơi hoặc chảy nước mắt. Cần cảnh giác nếu có khó thở, phù môi mắt hoặc choáng.',
    NhomBenh = N'Da liễu',
    NgayCapNhat = GETDATE()
WHERE TenBenh = N'Dị ứng / Mề đay';

UPDATE Benh SET
    MoTa = N'Tình trạng đau rát thượng vị, ợ chua, khó tiêu, đầy hơi hoặc buồn nôn, thường liên quan tăng acid, kích ứng niêm mạc hoặc rối loạn tiêu hoá. Cần thận trọng với thuốc NSAID và dấu hiệu xuất huyết tiêu hoá.',
    NhomBenh = N'Tiêu hoá',
    NgayCapNhat = GETDATE()
WHERE TenBenh = N'Đau dạ dày';
GO

-- Bo sung thanh phan thuoc.
INSERT INTO ThanhPhan (TenThanhPhan)
SELECT src.TenThanhPhan
FROM (VALUES
    (N'Paracetamol'),
    (N'Ibuprofen'),
    (N'Cetirizine'),
    (N'Dextromethorphan'),
    (N'Acid ascorbic'),
    (N'Acid acetylsalicylic'),
    (N'Cafein'),
    (N'Methyl salicylate'),
    (N'Menthol'),
    (N'Muối bù điện giải ORS'),
    (N'Loperamide'),
    (N'Diosmectite'),
    (N'Lactobacillus acidophilus'),
    (N'Berberin clorid'),
    (N'Loratadine'),
    (N'Chlorpheniramine maleate'),
    (N'Hydrocortisone'),
    (N'Fexofenadine'),
    (N'Calamine'),
    (N'Kẽm oxit'),
    (N'Aluminium hydroxide'),
    (N'Magnesium hydroxide'),
    (N'Omeprazole'),
    (N'Simethicone'),
    (N'Sucralfate'),
    (N'Domperidone')
) AS src(TenThanhPhan)
WHERE NOT EXISTS (
    SELECT 1 FROM ThanhPhan tp WHERE tp.TenThanhPhan = src.TenThanhPhan
);
GO

INSERT INTO ThuocThanhPhan (MaThuoc, MaThanhPhan)
SELECT t.MaThuoc, tp.MaThanhPhan
FROM (VALUES
    (N'Paracetamol 500mg', N'Paracetamol'),
    (N'Ibuprofen 200mg', N'Ibuprofen'),
    (N'Cetirizine 10mg', N'Cetirizine'),
    (N'Dextromethorphan 15mg', N'Dextromethorphan'),
    (N'Vitamin C 500mg', N'Acid ascorbic'),
    (N'Ibuprofen 400mg', N'Ibuprofen'),
    (N'Aspirin 500mg', N'Acid acetylsalicylic'),
    (N'Cafein kết hợp Paracetamol', N'Paracetamol'),
    (N'Cafein kết hợp Paracetamol', N'Cafein'),
    (N'Cao dán giảm đau Salonpas', N'Methyl salicylate'),
    (N'Cao dán giảm đau Salonpas', N'Menthol'),
    (N'Oresol', N'Muối bù điện giải ORS'),
    (N'Loperamide 2mg', N'Loperamide'),
    (N'Smecta (Diosmectite)', N'Diosmectite'),
    (N'Men vi sinh Probiotic', N'Lactobacillus acidophilus'),
    (N'Berberin 50mg', N'Berberin clorid'),
    (N'Loratadine 10mg', N'Loratadine'),
    (N'Chlorpheniramine 4mg', N'Chlorpheniramine maleate'),
    (N'Kem bôi Hydrocortisone 1%', N'Hydrocortisone'),
    (N'Fexofenadine 60mg', N'Fexofenadine'),
    (N'Calamine Lotion', N'Calamine'),
    (N'Calamine Lotion', N'Kẽm oxit'),
    (N'Antacid (Nhôm hydroxit + Magie hydroxit)', N'Aluminium hydroxide'),
    (N'Antacid (Nhôm hydroxit + Magie hydroxit)', N'Magnesium hydroxide'),
    (N'Omeprazole 20mg (OTC)', N'Omeprazole'),
    (N'Simethicone 80mg', N'Simethicone'),
    (N'Sucralfate 1g', N'Sucralfate'),
    (N'Domperidone 10mg', N'Domperidone')
) AS src(TenThuoc, TenThanhPhan)
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
JOIN ThanhPhan tp ON tp.TenThanhPhan = src.TenThanhPhan
WHERE NOT EXISTS (
    SELECT 1
    FROM ThuocThanhPhan existing
    WHERE existing.MaThuoc = t.MaThuoc
      AND existing.MaThanhPhan = tp.MaThanhPhan
);
GO

-- Bo sung them mot so canh bao de test ro hon.
INSERT INTO CanhBaoBenhNenThuoc (MaThuoc, MaBenhNen, NoiDung)
SELECT t.MaThuoc, bn.MaBenhNen, src.NoiDung
FROM (VALUES
    (N'Vitamin C 500mg', N'Suy thận mạn', N'Vitamin C liều cao có thể làm tăng nguy cơ sỏi thận hoặc tích lũy oxalat ở người suy thận. Hỏi ý kiến nhân viên y tế nếu dùng kéo dài.'),
    (N'Oresol', N'Tăng huyết áp', N'Một số dung dịch bù điện giải có natri. Người tăng huyết áp cần pha đúng tỉ lệ và không lạm dụng.'),
    (N'Loperamide 2mg', N'Suy gan', N'Loperamide chuyển hóa qua gan. Người suy gan cần thận trọng và theo dõi dấu hiệu buồn ngủ, chóng mặt.'),
    (N'Antacid (Nhôm hydroxit + Magie hydroxit)', N'Suy thận mạn', N'Antacid chứa nhôm/magie có thể tích lũy ở người suy thận. Hỏi bác sĩ trước khi dùng.'),
    (N'Simethicone 80mg', N'Viêm loét dạ dày tá tràng', N'Simethicone thường an toàn nhưng không xử lý nguyên nhân loét. Cần đi khám nếu đau rát kéo dài hoặc nôn ra máu.')
) AS src(TenThuoc, TenBenhNen, NoiDung)
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
JOIN BenhNen bn ON bn.TenBenhNen = src.TenBenhNen
WHERE NOT EXISTS (
    SELECT 1
    FROM CanhBaoBenhNenThuoc existing
    WHERE existing.MaThuoc = t.MaThuoc
      AND existing.MaBenhNen = bn.MaBenhNen
);
GO

INSERT INTO CanhBaoDiUngThuoc (MaThuoc, MaDiUng, NoiDung)
SELECT t.MaThuoc, d.MaDiUng, src.NoiDung
FROM (VALUES
    (N'Cao dán giảm đau Salonpas', N'Dị ứng Aspirin / NSAID', N'Sản phẩm có methyl salicylate. Người dị ứng Aspirin/NSAID nên thận trọng và thử trên vùng da nhỏ trước khi dùng.'),
    (N'Calamine Lotion', N'Dị ứng Latex', N'Người dị ứng Latex hoặc da quá nhạy cảm nên thử trên vùng da nhỏ trước khi bôi rộng.'),
    (N'Chlorpheniramine 4mg', N'Dị ứng phấn hoa', N'Có thể giúp giảm triệu chứng dị ứng nhưng gây buồn ngủ. Theo dõi đáp ứng và tránh lái xe sau khi dùng.')
) AS src(TenThuoc, TenDiUng, NoiDung)
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
JOIN DiUng d ON d.TenDiUng = src.TenDiUng
WHERE NOT EXISTS (
    SELECT 1
    FROM CanhBaoDiUngThuoc existing
    WHERE existing.MaThuoc = t.MaThuoc
      AND existing.MaDiUng = d.MaDiUng
);
GO
