from sentence_transformers import SentenceTransformer
from vector_store import VectorStore

MODEL_NAME = "intfloat/multilingual-e5-small"
W_SIMILARITY = 0.6
W_DO_UU_TIEN = 0.3
W_TRONG_SO_TRIEU_CHUNG = 0.1

class DrugRecommender:
    def __init__(self):
        print("Đang load model multilingual-e5-small...")
        self.model = SentenceTransformer(MODEL_NAME)
        self.store = VectorStore()
        print(f"Vector store hiện có {len(self.store)} thuốc.")

    def build_drug_text(self, drug: dict) -> str:
        text = (
            f"Thuốc {drug['ten_thuoc']}. "
            f"Hoạt chất: {drug['hoat_chat']}. "
            f"Nhóm thuốc: {drug['nhom_thuoc']}. "
            f"Công dụng: {drug['cong_dung']}. "
            f"Dạng bào chế: {drug['dang_bao_che']}."
        )
        return f"passage: {text}"

    def embed_drug(self, drug: dict) -> dict:
        text = self.build_drug_text(drug)
        vector = self.model.encode([text], normalize_embeddings=True)[0]  
        meta = {
            "ten_thuoc": drug["ten_thuoc"],
            "ma_benh": str(drug.get("ma_benh")) if drug.get("ma_benh") is not None else None,
            "ten_benh": drug.get("ten_benh"),
            "hoat_chat": drug["hoat_chat"],
            "nhom_thuoc": drug["nhom_thuoc"],
            "cong_dung": drug["cong_dung"],
            "lieu_dung": drug.get("lieu_dung", ""),
            "can_ke_don": str(drug.get("can_ke_don", "0")),
            "do_uu_tien": str(drug.get("do_uu_tien", "1")),
            "loai_dieu_tri": drug.get("loai_dieu_tri", "primary"),
        }
 
        self.store.upsert(ma_thuoc=drug["ma_thuoc"], vector=vector, meta=meta)
        return {"status": "ok", "ma_thuoc": str(drug["ma_thuoc"]), "total_drugs": len(self.store)}
    
    def remove_drug(self, ma_thuoc) -> dict:
        ok = self.store.delete(ma_thuoc)
        return {"status": "ok" if ok else "not_found", "total_drugs": len(self.store)}

    def build_query_text(self, ten_benh: str, ds_trieu_chung: list) -> str:
        text = f"Bệnh: {ten_benh}. Triệu chứng: {', '.join(ds_trieu_chung)}."
        return f"query: {text}"
    
    def recommend(self, ten_benh: str, ds_trieu_chung: list, ma_benh: str = None, top_k: int = 5) -> list:
        ma_benh = str(ma_benh) if ma_benh is not None else None
        query_text = self.build_query_text(ten_benh, ds_trieu_chung)
        query_vec = self.model.encode([query_text], normalize_embeddings=True)[0]
        all_candidates = self.store.search(query_vec, top_k=len(self.store))

        results = []
        for meta in all_candidates:
            if meta["can_ke_don"] != "0":
                continue
 
            sim_score = meta["similarity_score"]
 
            do_uu_tien = int(meta["do_uu_tien"])
            uu_tien_score = (4 - do_uu_tien) / 3  
 
            same_disease_bonus = 1.0 if (ma_benh and meta["ma_benh"] == ma_benh) else 0.0
 
            final_score = sim_score * W_SIMILARITY + uu_tien_score * W_DO_UU_TIEN + same_disease_bonus * W_TRONG_SO_TRIEU_CHUNG

            results.append({
                "ma_thuoc": meta["ma_thuoc"],
                "ten_thuoc": meta["ten_thuoc"],
                "hoat_chat": meta["hoat_chat"],
                "nhom_thuoc": meta["nhom_thuoc"],
                "cong_dung": meta["cong_dung"],
                "lieu_dung": meta["lieu_dung"],
                "ten_benh": meta["ten_benh"],
                "diem": round(final_score, 4),
                "diem_similarity": round(sim_score, 4),
                "ly_do": self._build_reason(meta, sim_score, ma_benh),
            })
        
        results.sort(key=lambda x: x["diem"], reverse=True)
        return results[:top_k]
    
    def _build_reason(self, meta: dict, sim_score: float, ma_benh: str) -> str:
        reason = (
            f"Thuốc thuộc nhóm '{meta['nhom_thuoc']}', công dụng phù hợp với "
            f"triệu chứng đã mô tả (độ phù hợp {sim_score * 100:.0f}%)."
        )
        
        if ma_benh and meta["ma_benh"] == ma_benh:
            loai = meta["loai_dieu_tri"]
            if loai == "primary":
                reason += " Đây là thuốc điều trị đầu tay cho bệnh này."
            elif loai == "secondary":
                reason += " Đây là thuốc điều trị thay thế cho bệnh này."
        return reason
            