import requests
import json

API_URL = "http://localhost:5000/predict"

def main():
    # Giả lập User đang thao tác trên giao diện web:
    # Chọn bệnh "Đau đầu" (Theo log của bạn thì Đau đầu là MaBenh=2)
    # Tích chọn các triệu chứng: "đau nhói vùng trán", "căng thẳng gáy", "hơi buồn nôn"
    payload = {
        "ma_benh": "2",  
        "ten_benh": "Đau đầu",
        "trieu_chung": ["đau nhói vùng trán", "căng thẳng gáy", "hơi buồn nôn"],
        "top_k": 3  # Lấy 3 thuốc tốt nhất
    }

    print("Đang gửi yêu cầu dự đoán đến API Flask...")
    print(f"Truy vấn: Bệnh {payload['ten_benh']} | Triệu chứng: {', '.join(payload['trieu_chung'])}\n")

    # Bắn request
    response = requests.post(API_URL, json=payload)

    # Xử lý kết quả trả về
    if response.status_code == 200:
        data = response.json()
        results = data.get("results", [])
        
        print(f"✅ HỆ THỐNG GỢI Ý {len(results)} LOẠI THUỐC:\n")
        
        for i, res in enumerate(results, 1):
            print(f"⭐ TOP {i}: {res['ten_thuoc']} (Mã thuốc: {res['ma_thuoc']})")
            print(f"   - Hoạt chất : {res['hoat_chat']}")
            print(f"   - Nhóm thuốc: {res['nhom_thuoc']}")
            print(f"   - Điểm tổng : {res['diem']} (Độ khớp ML: {res['diem_similarity']})")
            print(f"   - Lời khuyên: {res['ly_do']}")
            print("-" * 70)
    else:
        print(f"❌ LỖI API: {response.status_code}")
        print(response.text)

if __name__ == "__main__":
    main()