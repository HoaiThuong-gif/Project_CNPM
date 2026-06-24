import csv
import requests
import pyodbc

API_URL = "http://localhost:5000/embed-drug"
CSV_FILE = "D:\Project_CNPM\Data\\thuoc.csv"

# Cấu hình kết nối SQL Server - sửa lại theo môi trường thật
CONN_STR = (
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=localhost,1433;"
    "DATABASE=WebsiteDuDoanThuoc;"
    "UID=sa;"
    "PWD=Kh@ng09102005;"
)

def main():
    with open(CSV_FILE, encoding="utf-8") as f:
        rows = list(csv.DictReader(f))

    print(f"Đang seed {len(rows)} thuốc...")

    conn = pyodbc.connect(CONN_STR)
    cursor = conn.cursor()

    for i, r in enumerate(rows, start=1):
        cursor.execute("SELECT MaBenh FROM Benh WHERE TenBenh = ?", r["ten_benh"])
        benh_row = cursor.fetchone()
        if benh_row is None:
            print(f"  [{i}/{len(rows)}] BỎ QUA - không tìm thấy bệnh '{r['ten_benh']}' trong DB")
            continue
        ma_benh_that = benh_row[0]

        cursor.execute("SELECT MaThuoc FROM Thuoc WHERE TenThuoc = ?", r["ten_thuoc"])
        thuoc_row = cursor.fetchone()
        if thuoc_row is None:
            print(f"  [{i}/{len(rows)}] BỎ QUA - không tìm thấy thuốc '{r['ten_thuoc']}' trong DB")
            continue
        ma_thuoc_that = thuoc_row[0]

        payload = {
            "ma_thuoc": ma_thuoc_that,   
            "ten_thuoc": r["ten_thuoc"],
            "hoat_chat": r["hoat_chat"],
            "nhom_thuoc": r["nhom_thuoc"],
            "cong_dung": r["cong_dung"],
            "dang_bao_che": r["dang_bao_che"],
            "lieu_dung": r["lieu_dung"],
            "can_ke_don": int(r["can_ke_don"]),
            "do_uu_tien": int(r["do_uu_tien"]),
            "ma_benh": ma_benh_that,    
            "ten_benh": r["ten_benh"],
            "loai_dieu_tri": r["loai_dieu_tri"],
        }

        resp = requests.post(API_URL, json=payload)
        if resp.status_code == 200:
            print(f"  [{i}/{len(rows)}] OK - {r['ten_thuoc']} (MaThuoc={ma_thuoc_that}, MaBenh={ma_benh_that})")
        else:
            print(f"  [{i}/{len(rows)}] LỖI - {r['ten_thuoc']}: {resp.text}")

    conn.close()
    print("Hoàn tất seed.")


if __name__ == "__main__":
    main()