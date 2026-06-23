const resultContainer = document.querySelector(".result-container");

if (resultContainer) {
    const inputKey = resultContainer.dataset.inputKey;
    const resultsKey = resultContainer.dataset.resultsKey;
    const backUrl = resultContainer.dataset.backUrl;
    const feedbackUrl = resultContainer.dataset.feedbackUrl;
    const historyUrl = resultContainer.dataset.historyUrl;
    const loginUrl = resultContainer.dataset.loginUrl;
    const medicineDetailUrl = resultContainer.dataset.medicineDetailUrl;

    const inputSummary = document.getElementById("predictionInputSummary");
    const recommendation = document.getElementById("predictionRecommendation");
    const status = document.getElementById("predictionStatus");
    const title = document.getElementById("predictionTitle");
    const description = document.getElementById("predictionDescription");
    const resultsList = document.getElementById("predictionResultsList");
    const safetyPanel = document.getElementById("predictionSafetyPanel");
    const safetyList = document.getElementById("predictionSafetyList");
    const backButton = document.getElementById("btnBackToPredict");
    const timestamp = document.getElementById("predictionTimestamp");
    const token = localStorage.getItem("token");

    const redirectToLogin = () => {
        const returnUrl = `${window.location.pathname}${window.location.search}`;
        window.location.href = `${loginUrl}?returnUrl=${encodeURIComponent(returnUrl)}`;
    };

    const normalizeWarnings = (item) => {
        const warnings = []
            .concat(item.allergyWarnings || [])
            .concat(item.diseaseWarnings || [])
            .concat(item.drugInteractions || []);

        if (typeof item.warnings === "string" && item.warnings.trim()) {
            warnings.push(...item.warnings.split("|").map((warning) => warning.trim()).filter(Boolean));
        }

        return warnings;
    };

    const normalizeHistoryItem = (historyItem) => {
        const results = Array.isArray(historyItem?.results)
            ? historyItem.results.map((item) => ({
                resultId: item.resultId,
                medicineId: item.medicineId,
                medicineName: item.medicineName,
                uses: item.uses,
                dosage: item.dosage,
                howToUse: item.howToUse,
                sideEffects: item.sideEffects,
                notes: item.notes,
                contraindications: item.contraindications,
                score: item.score,
                reason: item.reason,
                warnings: item.warnings
            }))
            : [];

        return {
            input: {
                diseaseName: historyItem?.diseaseName || "",
                symptoms: historyItem?.symptoms || "",
                createdAt: historyItem?.createdAt || ""
            },
            results
        };
    };

    const loadLatestHistory = async () => {
        if (!token || !historyUrl) {
            return null;
        }

        const response = await fetch(historyUrl, {
            headers: {
                Accept: "application/json",
                Authorization: `Bearer ${token}`
            }
        });

        if (response.status === 401) {
            redirectToLogin();
            return null;
        }

        if (!response.ok) {
            return null;
        }

        const payload = await response.json();
        const items = Array.isArray(payload.data) ? payload.data : [];
        return items.length ? normalizeHistoryItem(items[0]) : null;
    };

    const showEmptyState = (message = "Chưa có dữ liệu thuốc để hiển thị.") => {
        if (safetyPanel) {
            safetyPanel.style.display = "none";
        }
        status.textContent = "Chưa có kết quả";
        title.textContent = "Chưa có dữ liệu";
        description.textContent = message;
        recommendation.textContent = "Hãy quay lại nhập thông tin và gửi yêu cầu dự đoán.";
        resultsList.innerHTML = `<div class="user-empty-state">${message}</div>`;
    };

    const renderPrediction = (predictionInput, predictionResults) => {
        if (!predictionInput || !Array.isArray(predictionResults) || predictionResults.length === 0) {
            showEmptyState();
            return;
        }

        const topResult = predictionResults[0];
        const rawScore = Number(topResult.score || 0);
        const topScore = rawScore <= 1 ? Math.round(rawScore * 100) : Math.round(rawScore);

        inputSummary.innerHTML = "";

        const fields = [
            predictionInput.diseaseName ? `Bệnh đã chọn: ${predictionInput.diseaseName}` : "",
            predictionInput.severity ? `Mức độ triệu chứng: ${predictionInput.severity}` : "",
            predictionInput.age ? `Tuổi: ${predictionInput.age}` : "",
            predictionInput.gender ? `Giới tính: ${predictionInput.gender}` : "",
            predictionInput.symptoms ? `Thông tin đã nhập: ${predictionInput.symptoms}` : "",
            Array.isArray(predictionInput.allergies) && predictionInput.allergies.length ? `Dị ứng thuốc: ${predictionInput.allergies.join(", ")}` : "",
            Array.isArray(predictionInput.backgroundDiseases) && predictionInput.backgroundDiseases.length ? `Bệnh nền: ${predictionInput.backgroundDiseases.join(", ")}` : "",
            Array.isArray(predictionInput.currentMedicines) && predictionInput.currentMedicines.length ? `Thuốc đang sử dụng: ${predictionInput.currentMedicines.join(", ")}` : "",
            predictionInput.createdAt ? `Thời gian: ${new Date(predictionInput.createdAt).toLocaleString("vi-VN")}` : ""
        ].filter(Boolean);

        fields.forEach((item) => {
            const row = document.createElement("li");
            row.textContent = item;
            inputSummary.appendChild(row);
        });

        status.textContent = `Độ phù hợp ${topScore}%`;
        title.textContent = topResult.medicineName || "Chưa có tên thuốc";
        description.textContent = topResult.reason || "Chưa có lý do gợi ý.";
        recommendation.textContent = topResult.dosage || "Chưa có liều dùng tham khảo.";
        resultsList.innerHTML = "";

        const safetyItems = [];
        predictionResults.forEach((item) => {
            const medicineName = item.medicineName || "Thuốc chưa rõ tên";
            const warnings = normalizeWarnings(item);
            const contraindications = typeof item.contraindications === "string" && item.contraindications.trim()
                ? item.contraindications.split("|").map((warning) => warning.trim()).filter(Boolean)
                : [];

            [...contraindications, ...warnings].forEach((warning) => {
                safetyItems.push(`${medicineName}: ${warning}`);
            });
        });

        const uniqueSafetyItems = [...new Set(safetyItems)];
        if (safetyPanel && safetyList) {
            if (uniqueSafetyItems.length) {
                safetyPanel.style.display = "";
                safetyList.innerHTML = uniqueSafetyItems
                    .map((warning) => `<span class="user-chip is-danger">${warning}</span>`)
                    .join("");
            } else {
                safetyPanel.style.display = "none";
                safetyList.innerHTML = "";
            }
        }

        predictionResults.forEach((item, index) => {
            const wrapper = document.createElement("article");
            wrapper.className = "user-medicine-card";

            const itemScore = Number(item.score || 0);
            const normalizedScore = itemScore <= 1 ? Math.round(itemScore * 100) : Math.round(itemScore);
            const warnings = normalizeWarnings(item);
            const detailUrl = (medicineDetailUrl || "").replace("__ID__", String(item.medicineId || 0));

            wrapper.innerHTML = `
                <div class="user-medicine-card-header">
                    <div class="user-medicine-index">${String(index + 1).padStart(2, "0")}</div>
                    <div>
                        <h3 class="user-medicine-name">${item.medicineName || "Chưa có tên thuốc"}</h3>
                        <div class="user-medicine-meta">${item.reason || "Chưa có lý do gợi ý."}</div>
                    </div>
                    <div class="user-score">
                        ${normalizedScore}
                        <span>phù hợp</span>
                    </div>
                </div>
                    <div class="user-medicine-body">
                    ${item.uses ? `<div><strong>Công dụng:</strong> ${item.uses}</div>` : ""}
                    <div class="user-medicine-highlight">Liều dùng tham khảo: ${item.dosage || "Chưa có dữ liệu"}</div>
                    ${item.howToUse ? `<div><strong>Cách dùng:</strong> ${item.howToUse}</div>` : ""}
                    ${item.contraindications ? `<div><strong>Chống chỉ định / cảnh báo:</strong> ${item.contraindications}</div>` : ""}
                    ${item.notes ? `<div><strong>Lưu ý:</strong> ${item.notes}</div>` : ""}
                    ${item.sideEffects ? `<div><strong>Tác dụng phụ:</strong> ${item.sideEffects}</div>` : ""}
                    ${warnings.length ? `<div class="user-warning-list">${warnings.map((warning) => `<span class="user-chip is-danger">${warning}</span>`).join("")}</div>` : ""}
                    <div class="user-feedback-row">
                        ${item.medicineId ? `<a class="user-secondary-button" href="${detailUrl}">Xem chi tiết thuốc</a>` : ""}
                        ${item.resultId ? `<button class="user-feedback-button" type="button" data-action="feedback" data-helpful="true" data-result-id="${item.resultId}">Hữu ích</button>
                        <button class="user-feedback-button" type="button" data-action="feedback" data-helpful="false" data-result-id="${item.resultId}">Không hữu ích</button>` : ""}
                    </div>
                    ${item.resultId ? `<textarea class="user-feedback-note" rows="3" placeholder="Ghi chú thêm nếu muốn..." data-feedback-note="${item.resultId}"></textarea>
                    <div class="user-feedback-status" data-feedback-status="${item.resultId}"></div>` : ""}
                </div>
            `;

            resultsList.appendChild(wrapper);
        });
    };

    if (timestamp) {
        timestamp.textContent = new Date().toLocaleTimeString("vi-VN");
    }

    backButton?.addEventListener("click", () => {
        window.location.href = backUrl;
    });

    resultsList.addEventListener("click", async (event) => {
        const button = event.target.closest('[data-action="feedback"]');
        if (!button) {
            return;
        }

        if (!token) {
            redirectToLogin();
            return;
        }

        const resultId = Number(button.dataset.resultId);
        const isHelpful = button.dataset.helpful === "true";
        const noteInput = resultsList.querySelector(`[data-feedback-note="${resultId}"]`);
        const statusNode = resultsList.querySelector(`[data-feedback-status="${resultId}"]`);
        const buttons = resultsList.querySelectorAll(`[data-result-id="${resultId}"]`);

        buttons.forEach((node) => {
            node.disabled = true;
        });

        if (statusNode) {
            statusNode.textContent = "Đang gửi phản hồi...";
        }

        try {
            const response = await fetch(feedbackUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify({
                    resultId,
                    isHelpful,
                    note: noteInput?.value.trim() || null
                })
            });

            if (response.status === 401) {
                redirectToLogin();
                return;
            }

            const result = await response.json();
            if (!response.ok) {
                throw new Error(result.message || "Gửi feedback thất bại.");
            }

            buttons.forEach((node) => {
                node.classList.toggle("is-active", node === button);
            });

            if (statusNode) {
                statusNode.textContent = result.message || "Đã gửi feedback.";
            }
        } catch (error) {
            if (statusNode) {
                statusNode.textContent = error.message || "Gửi feedback thất bại.";
            }
        } finally {
            buttons.forEach((node) => {
                node.disabled = false;
            });
        }
    });

    (async () => {
        const predictionInput = JSON.parse(sessionStorage.getItem(inputKey) || "null");
        const predictionResults = JSON.parse(sessionStorage.getItem(resultsKey) || "[]");

        if (predictionInput && Array.isArray(predictionResults) && predictionResults.length > 0) {
            renderPrediction(predictionInput, predictionResults);
            return;
        }

        showEmptyState("Đang tải lại kết quả dự đoán gần nhất...");
        const latestHistory = await loadLatestHistory();
        if (latestHistory) {
            renderPrediction(latestHistory.input, latestHistory.results);
            return;
        }

        showEmptyState();
    })();
}
