import os
import sys

import pyodbc
import requests


API_URL = os.getenv("AI_API_URL", "http://localhost:5000/embed-drug")
CONN_STR = os.getenv(
    "SQLSERVER_CONN_STR",
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=localhost,1433;"
    "DATABASE=WebsiteDuDoanThuoc;"
    "UID=sa;"
    "PWD=Kh@ng09102005;"
    "TrustServerCertificate=yes;",
)

QUERY = """
SELECT
    t.MaThuoc,
    t.TenThuoc,
    ISNULL(t.HoatChat, '') AS HoatChat,
    ISNULL(t.NhomThuoc, '') AS NhomThuoc,
    ISNULL(t.CongDung, '') AS CongDung,
    ISNULL(t.DangBaoChe, '') AS DangBaoChe,
    ISNULL(t.LieuDung, '') AS LieuDung,
    ISNULL(t.CanKeDon, 0) AS CanKeDon,
    b.MaBenh,
    b.TenBenh,
    ISNULL(bt.DoUuTien, 1) AS DoUuTien,
    ISNULL(bt.LoaiDieuTri, 'primary') AS LoaiDieuTri
FROM Thuoc t
JOIN BenhThuoc bt ON bt.MaThuoc = t.MaThuoc
JOIN Benh b ON b.MaBenh = bt.MaBenh
WHERE t.DangHoatDong = 1
  AND b.DangHoatDong = 1
  AND b.DeleteAt IS NULL
ORDER BY b.TenBenh, bt.DoUuTien, t.TenThuoc;
"""


def build_payload(row):
    return {
        "ma_thuoc": int(row.MaThuoc),
        "ten_thuoc": row.TenThuoc,
        "hoat_chat": row.HoatChat,
        "nhom_thuoc": row.NhomThuoc,
        "cong_dung": row.CongDung,
        "dang_bao_che": row.DangBaoChe,
        "lieu_dung": row.LieuDung,
        "can_ke_don": int(row.CanKeDon or 0),
        "ma_benh": int(row.MaBenh),
        "ten_benh": row.TenBenh,
        "do_uu_tien": int(row.DoUuTien or 1),
        "loai_dieu_tri": row.LoaiDieuTri or "primary",
    }


def main():
    try:
        health = requests.get(API_URL.replace("/embed-drug", "/health"), timeout=10)
        health.raise_for_status()
    except requests.RequestException as exc:
        print(f"AI backend is not reachable at {API_URL}: {exc}")
        return 1

    conn = pyodbc.connect(CONN_STR)
    cursor = conn.cursor()
    rows = cursor.execute(QUERY).fetchall()
    print(f"Syncing {len(rows)} disease-medicine records to vector store...")

    ok_count = 0
    for index, row in enumerate(rows, start=1):
        payload = build_payload(row)
        try:
            response = requests.post(API_URL, json=payload, timeout=60)
            response.raise_for_status()
            ok_count += 1
            print(f"[{index}/{len(rows)}] OK - {payload['ten_benh']} -> {payload['ten_thuoc']}")
        except requests.RequestException as exc:
            print(f"[{index}/{len(rows)}] ERROR - {payload['ten_thuoc']}: {exc}")

    conn.close()
    print(f"Done. Synced {ok_count}/{len(rows)} records.")
    return 0 if ok_count == len(rows) else 1


if __name__ == "__main__":
    sys.exit(main())
