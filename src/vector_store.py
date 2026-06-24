import json
import os
import numpy as np

class VectorStore:
    def __init__(self, vector_file="drugs_vector.npy", meta_file="drug_metadata.json", vector_dim=384):
        self.vector_file = vector_file
        self.meta_file = meta_file
        self.vector_dim = vector_dim
        self.vectors = None
        self.metadata = []
        self.load_data()

    def load_data(self):
        if os.path.exists(self.vector_file) and os.path.exists(self.meta_file):
            self.vectors = np.load(self.vector_file)
            with open(self.meta_file, "r", encoding="utf-8") as f:
                self.metadata = json.load(f)
        else:
            self.vectors = np.empty((0, self.vector_dim), dtype=np.float32)
            self.metadata = []
        
    def _save_data(self):
        np.save(self.vector_file, self.vectors)
        with open(self.meta_file, "w", encoding="utf-8") as f:
            json.dump(self.metadata, f, ensure_ascii=False, indent=2)

    def upsert(self, ma_thuoc: str, vector: np.ndarray, meta: dict):
        if vector.ndim == 1:
            vector = vector.reshape(1, -1)

        idx = next((i for i, m in enumerate(self.metadata) if m.get("ma_thuoc") == str(ma_thuoc)), None)
        meta["ma_thuoc"] = str(ma_thuoc)

        if idx is not None:
            self.vectors[idx] = vector
            self.metadata[idx] = meta
        else:
            self.vectors = np.vstack([self.vectors, vector])
            self.metadata.append(meta)

        self._save_data()

    def delete(self, ma_thuoc: str) -> bool:
        idx = next((i for i, m in enumerate(self.metadata) if m.get("ma_thuoc") == str(ma_thuoc)), None)
        
        if idx is not None:
            self.vectors = np.delete(self.vectors, idx, axis=0)
            self.metadata.pop(idx)
            self._save_data()
            return True
        return False
    
    def search(self, query_vector: np.ndarray, top_k: int = 5) -> list:
        if len(self.metadata) == 0:
            return []
 
        similarities = np.dot(self.vectors, query_vector)
        top_indices = np.argsort(similarities)[::-1][:top_k]
 
        results = []
        for idx in top_indices:
            result = self.metadata[idx].copy()
            result["similarity_score"] = float(similarities[idx])
            results.append(result)
 
        return results
 
    def __len__(self):
        return len(self.metadata)
