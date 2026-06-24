from flask import Flask, request, jsonify
from predict import DrugRecommender

app = Flask(__name__)
recommender = DrugRecommender()

@app.route("/embed-drug", methods=["POST"])
def embed_drug():
    drug = request.get_json()
 
    required_fields = ["ma_thuoc", "ten_thuoc", "hoat_chat", "nhom_thuoc", "cong_dung", "dang_bao_che"]
    missing = [f for f in required_fields if f not in drug]
    if missing:
        return jsonify({"error": f"Thiếu các field: {', '.join(missing)}"}), 400
 
    result = recommender.embed_drug(drug)
    return jsonify(result)

@app.route("/remove-drug", methods=["POST"])
def remove_drug():
    data = request.get_json()
    if "ma_thuoc" not in data:
        return jsonify({"error": "Thiếu ma_thuoc"}), 400
 
    result = recommender.remove_drug(data["ma_thuoc"])
    return jsonify(result)

@app.route("/predict", methods=["POST"])
def predict():
    data = request.get_json()
 
    ten_benh = data.get("ten_benh", "")
    trieu_chung = data.get("trieu_chung", [])
    ma_benh = data.get("ma_benh")
    top_k = data.get("top_k", 5)
 
    if not ten_benh or not trieu_chung:
        return jsonify({"error": "Thiếu ten_benh hoặc trieu_chung"}), 400
 
    results = recommender.recommend(
        ten_benh=ten_benh,
        ds_trieu_chung=trieu_chung,
        ma_benh=ma_benh,
        top_k=top_k,
    )
 
    return jsonify({"results": results})

@app.route("/health", methods=["GET"])
def health():
    return jsonify({"status": "ok", "total_drugs": len(recommender.store)})
 
 
if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000, debug=False)