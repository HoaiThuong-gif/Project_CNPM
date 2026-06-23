USE WebsiteDuDoanThuoc;
GO

-- Mo rong dataset: them benh, trieu chung, thuoc, mapping va canh bao.
-- Script an toàn để chạy nhiều lần: các INSERT đều kiểm tra tồn tại trước.

INSERT INTO Benh (TenBenh, MoTa, NhomBenh, MucDoNghiemTrong, DangHoatDong)
SELECT src.TenBenh, src.MoTa, src.NhomBenh, src.MucDoNghiemTrong, 1
FROM (VALUES
    (N'Viêm họng', N'Tình trạng đau rát họng, khàn tiếng, khó nuốt hoặc ho do kích ứng/viêm đường hô hấp trên. Cần đi khám nếu sốt cao, khó thở, đau họng kéo dài hoặc có mủ amidan.', N'Hô hấp', 1),
    (N'Viêm mũi dị ứng', N'Phản ứng viêm niêm mạc mũi do dị nguyên như bụi, phấn hoa, thời tiết hoặc lông thú, thường gây hắt hơi, sổ mũi, nghẹt mũi, ngứa mũi và chảy nước mắt.', N'Dị ứng - hô hấp', 1),
    (N'Táo bón', N'Tình trạng đi tiêu khó, phân khô cứng hoặc số lần đi tiêu giảm, thường liên quan thiếu nước, ít chất xơ, ít vận động hoặc thay đổi thói quen sinh hoạt.', N'Tiêu hoá', 1),
    (N'Trào ngược dạ dày thực quản', N'Tình trạng acid hoặc dịch dạ dày trào ngược lên thực quản, gây ợ chua, nóng rát sau xương ức, khó tiêu, buồn nôn hoặc đau rát thượng vị.', N'Tiêu hoá', 2)
) AS src(TenBenh, MoTa, NhomBenh, MucDoNghiemTrong)
WHERE NOT EXISTS (SELECT 1 FROM Benh b WHERE b.TenBenh = src.TenBenh);
GO

INSERT INTO TrieuChung (TenTrieuChung, MoTa)
SELECT src.TenTrieuChung, src.MoTa
FROM (VALUES
    (N'Nghẹt mũi', N'Cảm giác tắc hoặc khó thở qua mũi do niêm mạc mũi sưng nề hoặc nhiều dịch tiết.'),
    (N'Khàn tiếng', N'Giọng nói thay đổi, khàn hoặc mất tiếng do kích ứng, viêm họng hoặc viêm thanh quản.'),
    (N'Khó nuốt', N'Cảm giác đau hoặc vướng khi nuốt thức ăn, nước uống hoặc nước bọt.'),
    (N'Ngứa mũi', N'Cảm giác ngứa, kích thích trong mũi, thường gặp trong viêm mũi dị ứng.'),
    (N'Táo bón', N'Đi tiêu khó, phân khô cứng hoặc giảm số lần đi tiêu so với bình thường.'),
    (N'Nóng rát sau xương ức', N'Cảm giác nóng rát vùng ngực sau xương ức, thường liên quan trào ngược acid.'),
    (N'Ho có đờm', N'Ho kèm chất nhầy hoặc đờm trong đường hô hấp.')
) AS src(TenTrieuChung, MoTa)
WHERE NOT EXISTS (SELECT 1 FROM TrieuChung t WHERE t.TenTrieuChung = src.TenTrieuChung);
GO

INSERT INTO BenhTrieuChung (MaBenh, MaTrieuChung, TrongSo)
SELECT b.MaBenh, t.MaTrieuChung, src.TrongSo
FROM (VALUES
    (N'Viêm họng', N'Đau họng', 5),
    (N'Viêm họng', N'Khàn tiếng', 4),
    (N'Viêm họng', N'Khó nuốt', 4),
    (N'Viêm họng', N'Ho khan', 3),
    (N'Viêm họng', N'Sốt', 2),
    (N'Viêm mũi dị ứng', N'Hắt hơi', 5),
    (N'Viêm mũi dị ứng', N'Sổ mũi', 5),
    (N'Viêm mũi dị ứng', N'Nghẹt mũi', 4),
    (N'Viêm mũi dị ứng', N'Ngứa mũi', 5),
    (N'Viêm mũi dị ứng', N'Chảy nước mắt', 3),
    (N'Táo bón', N'Táo bón', 5),
    (N'Táo bón', N'Đau bụng', 3),
    (N'Táo bón', N'Đầy hơi chướng bụng', 4),
    (N'Táo bón', N'Khó tiêu', 2),
    (N'Trào ngược dạ dày thực quản', N'Ợ chua', 5),
    (N'Trào ngược dạ dày thực quản', N'Nóng rát sau xương ức', 5),
    (N'Trào ngược dạ dày thực quản', N'Khó tiêu', 4),
    (N'Trào ngược dạ dày thực quản', N'Buồn nôn', 2),
    (N'Trào ngược dạ dày thực quản', N'Đau rát thượng vị', 4)
) AS src(TenBenh, TenTrieuChung, TrongSo)
JOIN Benh b ON b.TenBenh = src.TenBenh
JOIN TrieuChung t ON t.TenTrieuChung = src.TenTrieuChung
WHERE NOT EXISTS (
    SELECT 1 FROM BenhTrieuChung existing
    WHERE existing.MaBenh = b.MaBenh AND existing.MaTrieuChung = t.MaTrieuChung
);
GO

INSERT INTO Thuoc (TenThuoc, HoatChat, NhomThuoc, DangBaoChe, CongDung, LieuDung, CachDung, TacDungPhu, LuuY, CanKeDon, DangHoatDong)
SELECT src.TenThuoc, src.HoatChat, src.NhomThuoc, src.DangBaoChe, src.CongDung, src.LieuDung, src.CachDung, src.TacDungPhu, src.LuuY, 0, 1
FROM (VALUES
    (N'Viên ngậm sát khuẩn họng', N'Dichlorobenzyl alcohol và Amylmetacresol', N'Sát khuẩn họng tại chỗ', N'Viên ngậm', N'Làm dịu đau rát họng và hỗ trợ giảm kích ứng họng nhẹ.', N'Ngậm 1 viên mỗi 2-3 giờ khi cần, tối đa theo hướng dẫn trên bao bì.', N'Ngậm tan chậm trong miệng, không nhai nuốt nguyên viên.', N'Kích ứng miệng nhẹ hoặc khó chịu dạ dày hiếm gặp.', N'Không dùng quá liều; người tiểu đường cần kiểm tra lượng đường trong viên ngậm.', 0),
    (N'Benzydamine xịt họng', N'Benzydamine', N'Kháng viêm giảm đau tại chỗ', N'Dung dịch xịt họng', N'Giảm đau rát họng, viêm họng nhẹ và khó chịu khi nuốt.', N'Xịt 2-4 nhát/lần, 2-6 lần/ngày tùy hướng dẫn sản phẩm.', N'Xịt trực tiếp vào vùng họng đau, tránh hít sâu khi xịt.', N'Tê miệng, châm chích hoặc khô miệng nhẹ.', N'Không dùng nếu dị ứng với benzydamine; đi khám nếu đau họng kéo dài hoặc sốt cao.', 0),
    (N'Nước muối súc họng 0.9%', N'Natri clorid', N'Vệ sinh họng miệng', N'Dung dịch súc họng', N'Hỗ trợ làm sạch họng, giảm kích ứng và cảm giác khô rát họng.', N'Súc họng 2-4 lần/ngày.', N'Ngậm và súc họng 20-30 giây rồi nhổ bỏ, không nuốt lượng lớn.', N'Hiếm gặp; có thể gây khó chịu nếu dùng quá nhiều.', N'Không thay thế điều trị khi có nhiễm khuẩn nặng hoặc sốt cao.', 0),
    (N'Xịt mũi nước muối biển', N'Natri clorid', N'Vệ sinh mũi', N'Dung dịch xịt mũi', N'Làm sạch dịch mũi, hỗ trợ giảm khô mũi và nghẹt mũi nhẹ.', N'Xịt 1-2 nhát mỗi bên mũi, 2-4 lần/ngày.', N'Xịt vào từng bên mũi, lau sạch dịch sau khi xịt.', N'Kích ứng mũi nhẹ hiếm gặp.', N'Dùng riêng chai xịt để tránh lây nhiễm chéo.', 0),
    (N'Xylometazoline 0.05% xịt mũi', N'Xylometazoline', N'Co mạch mũi', N'Dung dịch xịt mũi', N'Giảm nghẹt mũi nhanh trong viêm mũi hoặc cảm lạnh.', N'Xịt 1 nhát mỗi bên mũi, 2-3 lần/ngày, không quá 3-5 ngày.', N'Xì mũi nhẹ trước khi xịt, tránh xịt liên tục kéo dài.', N'Khô mũi, kích ứng mũi, hồi hộp hoặc tăng huyết áp hiếm gặp.', N'Không lạm dụng vì có thể gây nghẹt mũi bật lại; thận trọng với tăng huyết áp.', 0),
    (N'Lactulose siro', N'Lactulose', N'Nhuận tràng thẩm thấu', N'Siro uống', N'Làm mềm phân và hỗ trợ đi tiêu trong táo bón.', N'15-30 ml/ngày, điều chỉnh theo đáp ứng.', N'Uống trực tiếp hoặc pha với nước, nên uống đủ nước trong ngày.', N'Đầy hơi, đau bụng nhẹ hoặc tiêu chảy nếu dùng nhiều.', N'Không dùng khi đau bụng chưa rõ nguyên nhân hoặc nghi tắc ruột.', 0),
    (N'Psyllium husk', N'Chất xơ psyllium', N'Bổ sung chất xơ', N'Bột pha uống', N'Tăng lượng chất xơ, hỗ trợ làm mềm phân và cải thiện táo bón.', N'1 gói/lần, 1-2 lần/ngày.', N'Pha với nhiều nước và uống ngay, uống thêm nước sau đó.', N'Đầy hơi nhẹ trong vài ngày đầu.', N'Không uống khi không đủ nước vì có thể gây nghẹn hoặc tắc nghẽn.', 0),
    (N'Bisacodyl 5mg', N'Bisacodyl', N'Nhuận tràng kích thích', N'Viên bao tan trong ruột', N'Kích thích nhu động ruột giúp đi tiêu khi táo bón ngắn ngày.', N'5-10 mg buổi tối khi cần.', N'Nuốt nguyên viên, không nhai; tránh dùng cùng sữa hoặc antacid trong vòng 1 giờ.', N'Đau quặn bụng, tiêu chảy, buồn nôn.', N'Không dùng kéo dài; tránh dùng khi đau bụng cấp chưa rõ nguyên nhân.', 0),
    (N'Alginate hỗn dịch', N'Sodium alginate và antacid', N'Chống trào ngược', N'Hỗn dịch uống', N'Tạo lớp màng nổi hạn chế acid trào ngược, giảm ợ chua và nóng rát sau xương ức.', N'10-20 ml sau ăn và trước khi ngủ.', N'Lắc kỹ, uống sau bữa ăn hoặc khi có triệu chứng.', N'Đầy bụng, buồn nôn nhẹ hiếm gặp.', N'Người cần hạn chế natri nên kiểm tra thành phần hoặc hỏi dược sĩ.', 0),
    (N'Famotidine 20mg', N'Famotidine', N'Kháng H2 giảm tiết acid', N'Viên nén', N'Giảm tiết acid dạ dày, hỗ trợ giảm ợ chua và khó chịu do trào ngược nhẹ.', N'20 mg khi có triệu chứng hoặc trước bữa ăn dễ gây ợ chua.', N'Uống với nước, có thể dùng trước ăn.', N'Đau đầu, chóng mặt, táo bón hoặc tiêu chảy hiếm gặp.', N'Người suy thận cần hỏi ý kiến nhân viên y tế trước khi dùng.', 0)
) AS src(TenThuoc, HoatChat, NhomThuoc, DangBaoChe, CongDung, LieuDung, CachDung, TacDungPhu, LuuY, CanKeDon)
WHERE NOT EXISTS (SELECT 1 FROM Thuoc t WHERE t.TenThuoc = src.TenThuoc);
GO

INSERT INTO BenhThuoc (MaBenh, MaThuoc, DoUuTien, LoaiDieuTri)
SELECT b.MaBenh, t.MaThuoc, src.DoUuTien, src.LoaiDieuTri
FROM (VALUES
    (N'Viêm họng', N'Viên ngậm sát khuẩn họng', 1, 'primary'),
    (N'Viêm họng', N'Benzydamine xịt họng', 1, 'primary'),
    (N'Viêm họng', N'Nước muối súc họng 0.9%', 2, 'secondary'),
    (N'Viêm họng', N'Dextromethorphan 15mg', 3, 'alternative'),
    (N'Viêm mũi dị ứng', N'Xịt mũi nước muối biển', 1, 'primary'),
    (N'Viêm mũi dị ứng', N'Cetirizine 10mg', 1, 'primary'),
    (N'Viêm mũi dị ứng', N'Loratadine 10mg', 1, 'primary'),
    (N'Viêm mũi dị ứng', N'Fexofenadine 60mg', 2, 'secondary'),
    (N'Viêm mũi dị ứng', N'Xylometazoline 0.05% xịt mũi', 3, 'alternative'),
    (N'Táo bón', N'Lactulose siro', 1, 'primary'),
    (N'Táo bón', N'Psyllium husk', 1, 'primary'),
    (N'Táo bón', N'Bisacodyl 5mg', 2, 'secondary'),
    (N'Táo bón', N'Simethicone 80mg', 3, 'alternative'),
    (N'Trào ngược dạ dày thực quản', N'Alginate hỗn dịch', 1, 'primary'),
    (N'Trào ngược dạ dày thực quản', N'Famotidine 20mg', 1, 'primary'),
    (N'Trào ngược dạ dày thực quản', N'Omeprazole 20mg (OTC)', 1, 'primary'),
    (N'Trào ngược dạ dày thực quản', N'Antacid (Nhôm hydroxit + Magie hydroxit)', 2, 'secondary'),
    (N'Trào ngược dạ dày thực quản', N'Sucralfate 1g', 3, 'alternative')
) AS src(TenBenh, TenThuoc, DoUuTien, LoaiDieuTri)
JOIN Benh b ON b.TenBenh = src.TenBenh
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
WHERE NOT EXISTS (
    SELECT 1 FROM BenhThuoc existing
    WHERE existing.MaBenh = b.MaBenh AND existing.MaThuoc = t.MaThuoc
);
GO

INSERT INTO ThanhPhan (TenThanhPhan)
SELECT src.TenThanhPhan
FROM (VALUES
    (N'Dichlorobenzyl alcohol'),
    (N'Amylmetacresol'),
    (N'Benzydamine'),
    (N'Natri clorid'),
    (N'Xylometazoline'),
    (N'Lactulose'),
    (N'Psyllium'),
    (N'Bisacodyl'),
    (N'Sodium alginate'),
    (N'Famotidine')
) AS src(TenThanhPhan)
WHERE NOT EXISTS (SELECT 1 FROM ThanhPhan tp WHERE tp.TenThanhPhan = src.TenThanhPhan);
GO

INSERT INTO ThuocThanhPhan (MaThuoc, MaThanhPhan)
SELECT t.MaThuoc, tp.MaThanhPhan
FROM (VALUES
    (N'Viên ngậm sát khuẩn họng', N'Dichlorobenzyl alcohol'),
    (N'Viên ngậm sát khuẩn họng', N'Amylmetacresol'),
    (N'Benzydamine xịt họng', N'Benzydamine'),
    (N'Nước muối súc họng 0.9%', N'Natri clorid'),
    (N'Xịt mũi nước muối biển', N'Natri clorid'),
    (N'Xylometazoline 0.05% xịt mũi', N'Xylometazoline'),
    (N'Lactulose siro', N'Lactulose'),
    (N'Psyllium husk', N'Psyllium'),
    (N'Bisacodyl 5mg', N'Bisacodyl'),
    (N'Alginate hỗn dịch', N'Sodium alginate'),
    (N'Alginate hỗn dịch', N'Aluminium hydroxide'),
    (N'Alginate hỗn dịch', N'Magnesium hydroxide'),
    (N'Famotidine 20mg', N'Famotidine')
) AS src(TenThuoc, TenThanhPhan)
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
JOIN ThanhPhan tp ON tp.TenThanhPhan = src.TenThanhPhan
WHERE NOT EXISTS (
    SELECT 1 FROM ThuocThanhPhan existing
    WHERE existing.MaThuoc = t.MaThuoc
      AND existing.MaThanhPhan = tp.MaThanhPhan
);
GO

INSERT INTO CanhBaoBenhNenThuoc (MaThuoc, MaBenhNen, NoiDung)
SELECT t.MaThuoc, bn.MaBenhNen, src.NoiDung
FROM (VALUES
    (N'Xylometazoline 0.05% xịt mũi', N'Tăng huyết áp', N'Thuốc co mạch mũi có thể làm tăng huyết áp hoặc gây hồi hộp ở người nhạy cảm. Không dùng kéo dài.'),
    (N'Xylometazoline 0.05% xịt mũi', N'Suy gan', N'Thận trọng nếu có bệnh gan nặng hoặc đang dùng nhiều thuốc khác; hỏi dược sĩ nếu cần dùng quá vài ngày.'),
    (N'Lactulose siro', N'Tiểu đường', N'Lactulose là đường tổng hợp; người tiểu đường nên theo dõi đường huyết và hỏi ý kiến nhân viên y tế nếu dùng thường xuyên.'),
    (N'Bisacodyl 5mg', N'Viêm loét dạ dày tá tràng', N'Không dùng nếu đau bụng cấp, nôn ói hoặc nghi tắc ruột. Cần đi khám nếu đau bụng dữ dội.'),
    (N'Famotidine 20mg', N'Suy thận mạn', N'Famotidine thải trừ qua thận; người suy thận có thể cần giảm liều. Hỏi bác sĩ hoặc dược sĩ trước khi dùng.'),
    (N'Alginate hỗn dịch', N'Tăng huyết áp', N'Một số chế phẩm alginate có natri. Người tăng huyết áp cần kiểm tra hàm lượng natri trên nhãn.')
) AS src(TenThuoc, TenBenhNen, NoiDung)
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
JOIN BenhNen bn ON bn.TenBenhNen = src.TenBenhNen
WHERE NOT EXISTS (
    SELECT 1 FROM CanhBaoBenhNenThuoc existing
    WHERE existing.MaThuoc = t.MaThuoc
      AND existing.MaBenhNen = bn.MaBenhNen
);
GO

INSERT INTO CanhBaoDiUngThuoc (MaThuoc, MaDiUng, NoiDung)
SELECT t.MaThuoc, d.MaDiUng, src.NoiDung
FROM (VALUES
    (N'Viên ngậm sát khuẩn họng', N'Dị ứng Latex', N'Người có cơ địa dị ứng hoặc kích ứng niêm mạc nên ngưng dùng nếu thấy sưng, ngứa hoặc khó chịu nhiều ở miệng họng.'),
    (N'Benzydamine xịt họng', N'Dị ứng Aspirin / NSAID', N'Người từng dị ứng thuốc giảm đau kháng viêm nên thận trọng và ngưng dùng nếu có phát ban, khó thở hoặc sưng phù.'),
    (N'Xịt mũi nước muối biển', N'Dị ứng bụi nhà', N'Có thể hỗ trợ rửa dị nguyên trong mũi nhưng không thay thế thuốc kiểm soát dị ứng nếu triệu chứng nặng.'),
    (N'Xylometazoline 0.05% xịt mũi', N'Dị ứng phấn hoa', N'Chỉ giúp giảm nghẹt mũi tạm thời, không điều trị nền dị ứng. Không dùng quá 3-5 ngày.')
) AS src(TenThuoc, TenDiUng, NoiDung)
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
JOIN DiUng d ON d.TenDiUng = src.TenDiUng
WHERE NOT EXISTS (
    SELECT 1 FROM CanhBaoDiUngThuoc existing
    WHERE existing.MaThuoc = t.MaThuoc
      AND existing.MaDiUng = d.MaDiUng
);
GO

INSERT INTO CanhBaoBenhNenThuoc (MaThuoc, MaBenhNen, NoiDung)
SELECT t.MaThuoc, bn.MaBenhNen, src.NoiDung
FROM (VALUES
    (N'Berberin 50mg', N'Suy gan', N'Berberin co the khong phu hop voi nguoi co benh gan nang. Nen hoi y kien nhan vien y te neu vang da, men gan cao hoac dang dung nhieu thuoc khac.'),
    (N'Cafein kết hợp Paracetamol', N'Suy gan', N'Paracetamol co nguy co gay doc gan khi dung qua lieu hoac dung chung voi ruou. Nguoi suy gan can tranh tu y dung va can hoi y kien bac si/duoc si.'),
    (N'Cafein kết hợp Paracetamol', N'Tăng huyết áp', N'Cafein co the lam hoi hop, mat ngu hoac tang huyet ap o nguoi nhay cam. Can than trong neu co tang huyet ap hoac roi loan nhip tim.'),
    (N'Dextromethorphan 15mg', N'Suy gan', N'Dextromethorphan duoc chuyen hoa qua gan. Nguoi suy gan nen than trong va khong dung keo dai neu khong co huong dan.'),
    (N'Kem bôi Hydrocortisone 1%', N'Tiểu đường', N'Corticosteroid boi ngoai da khi dung keo dai/boi dien rong co the anh huong kiem soat duong huyet. Chi boi lop mong trong thoi gian ngan.'),
    (N'Men vi sinh Probiotic', N'Tiểu đường', N'Mot so che pham probiotic dang goi co the co duong hoac ta duoc tao ngot. Nguoi tieu duong nen kiem tra nhan san pham.'),
    (N'Nước muối súc họng 0.9%', N'Tăng huyết áp', N'Khong nen nuot luong lon dung dich nuoc muoi. Nguoi can han che natri nen dung dung cach va khong lam dung.'),
    (N'Psyllium husk', N'Tiểu đường', N'Psyllium co the anh huong hap thu duong va mot so thuoc uong. Nguoi tieu duong nen theo doi duong huyet va uong cach thuoc khac.'),
    (N'Smecta (Diosmectite)', N'Tiểu đường', N'Mot so che pham diosmectite co the chua glucose/sucrose. Nguoi tieu duong nen kiem tra thanh phan va khong dung keo dai.')
) AS src(TenThuoc, TenBenhNen, NoiDung)
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
JOIN BenhNen bn ON bn.TenBenhNen = src.TenBenhNen
WHERE NOT EXISTS (
    SELECT 1 FROM CanhBaoBenhNenThuoc existing
    WHERE existing.MaThuoc = t.MaThuoc
      AND existing.MaBenhNen = bn.MaBenhNen
);
GO

INSERT INTO TuongTacThuoc (MaThuoc1, MaThuoc2, MucDoNghiemTrong, MoTa)
SELECT
    CASE WHEN t1.MaThuoc < t2.MaThuoc THEN t1.MaThuoc ELSE t2.MaThuoc END,
    CASE WHEN t1.MaThuoc < t2.MaThuoc THEN t2.MaThuoc ELSE t1.MaThuoc END,
    src.MucDo,
    src.MoTa
FROM (VALUES
    (N'Xylometazoline 0.05% xịt mũi', N'Cetirizine 10mg', 1, N'Dùng chung thường được nhưng cần theo dõi khô mũi, khô miệng hoặc khó chịu tăng lên.'),
    (N'Bisacodyl 5mg', N'Lactulose siro', 2, N'Dùng chung hai thuốc nhuận tràng có thể gây đau quặn bụng hoặc tiêu chảy. Chỉ phối hợp khi có hướng dẫn.'),
    (N'Famotidine 20mg', N'Antacid (Nhôm hydroxit + Magie hydroxit)', 1, N'Antacid có thể ảnh hưởng hấp thu một số thuốc. Nên uống cách Famotidine ít nhất 1-2 giờ nếu cần.'),
    (N'Alginate hỗn dịch', N'Omeprazole 20mg (OTC)', 1, N'Có thể phối hợp trong trào ngược nhưng nên dùng đúng thời điểm: Omeprazole trước ăn, alginate sau ăn.')
) AS src(TenThuoc1, TenThuoc2, MucDo, MoTa)
JOIN Thuoc t1 ON t1.TenThuoc = src.TenThuoc1
JOIN Thuoc t2 ON t2.TenThuoc = src.TenThuoc2
WHERE NOT EXISTS (
    SELECT 1
    FROM TuongTacThuoc existing
    WHERE existing.MaThuoc1 = CASE WHEN t1.MaThuoc < t2.MaThuoc THEN t1.MaThuoc ELSE t2.MaThuoc END
      AND existing.MaThuoc2 = CASE WHEN t1.MaThuoc < t2.MaThuoc THEN t2.MaThuoc ELSE t1.MaThuoc END
);
GO
