const resultContainer = document.querySelector(".result-container");

if (resultContainer) {
    const inputKey = resultContainer.dataset.inputKey;
    const resultsKey = resultContainer.dataset.resultsKey;
    const backUrl = resultContainer.dataset.backUrl;
    const feedbackUrl = resultContainer.dataset.feedbackUrl;
    const loginUrl = resultContainer.dataset.loginUrl;
    const medicineDetailUrl = resultContainer.dataset.medicineDetailUrl;

    const inputSummary = document.getElementById("predictionInputSummary");
    const recommendation = document.getElementById("predictionRecommendation");
    const status = document.getElementById("predictionStatus");
    const title = document.getElementById("predictionTitle");
    const description = document.getElementById("predictionDescription");
    const resultsList = document.getElementById("predictionResultsList");
    const backButton = document.getElementById("btnBackToPredict");
    const timestamp = document.getElementById("predictionTimestamp");
    const token = localStorage.getItem("token");

    const predictionInput = JSON.parse(sessionStorage.getItem(inputKey) || "null");
    const predictionResults = JSON.parse(sessionStorage.getItem(resultsKey) || "[]");

    const redirectToLogin = () => {
        const returnUrl = `${window.location.pathname}${window.location.search}`;
        window.location.href = `${loginUrl}?returnUrl=${encodeURIComponent(returnUrl)}`;
    };

    if (timestamp) {
        timestamp.textContent = new Date().toLocaleTimeString("vi-VN");
    }

    backButton?.addEventListener("click", () => {
        window.location.href = backUrl;
    });

    if (!predictionInput || !Array.isArray(predictionResults) || predictionResults.length === 0) {
        return;
    }

    const topResult = predictionResults[0];
    const rawScore = Number(topResult.score || 0);
    const topScore = rawScore <= 1 ? Math.round(rawScore * 100) : Math.round(rawScore);

    inputSummary.innerHTML = "";

    const fields = [
        `Bệnh đã chọn: ${predictionInput.diseaseName}`,
        predictionInput.severity ? `Mức độ triệu chứng: ${predictionInput.severity}` : "",
        predictionInput.age ? `Tuổi: ${predictionInput.age}` : "",
        predictionInput.gender ? `Giới tính: ${predictionInput.gender}` : "",
        predictionInput.symptoms ? `Triệu chứng: ${predictionInput.symptoms}` : "",
        Array.isArray(predictionInput.allergies) && predictionInput.allergies.length ? `Dị ứng thuốc: ${predictionInput.allergies.join(", ")}` : "",
        Array.isArray(predictionInput.backgroundDiseases) && predictionInput.backgroundDiseases.length ? `Bệnh nền: ${predictionInput.backgroundDiseases.join(", ")}` : "",
        Array.isArray(predictionInput.currentMedicines) && predictionInput.currentMedicines.length ? `Thuốc đang sử dụng: ${predictionInput.currentMedicines.join(", ")}` : ""
    ].filter(Boolean);

    fields.forEach((item) => {
        const row = document.createElement("li");
        row.textContent = item;
        inputSummary.appendChild(row);
    });

    status.textContent = `Độ phù hợp ${topScore}%`;
    title.textContent = topResult.medicineName || "Không có dữ liệu";
    description.textContent = topResult.reason || "Chưa có mô tả từ backend.";
    recommendation.textContent = topResult.dosage || "Chưa có khuyến nghị liều dùng.";
    resultsList.innerHTML = "";

    predictionResults.forEach((item, index) => {
        const wrapper = document.createElement("article");
        wrapper.className = "user-medicine-card";

        const itemScore = Number(item.score || 0);
        const normalizedScore = itemScore <= 1 ? Math.round(itemScore * 100) : Math.round(itemScore);
        const warnings = []
            .concat(item.allergyWarnings || [])
            .concat(item.diseaseWarnings || [])
            .concat(item.drugInteractions || []);
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
                <div>${item.dosage || "Chưa có liều dùng."}</div>
                <div class="user-medicine-highlight">Liều dùng tham khảo: ${item.dosage || "Chưa có dữ liệu"}</div>
                ${warnings.length ? `<div class="user-warning-list">${warnings.map((warning) => `<span class="user-chip is-danger">${warning}</span>`).join("")}</div>` : ""}
                <div class="user-feedback-row">
                    <a class="user-secondary-button" href="${detailUrl}">Xem chi tiết thuốc</a>
                    <button class="user-feedback-button" type="button" data-action="feedback" data-helpful="true" data-result-id="${item.resultId}">Hữu ích</button>
                    <button class="user-feedback-button" type="button" data-action="feedback" data-helpful="false" data-result-id="${item.resultId}">Không hữu ích</button>
                </div>
                <textarea class="user-feedback-note" rows="3" placeholder="Ghi chú thêm nếu muốn..." data-feedback-note="${item.resultId}"></textarea>
                <div class="user-feedback-status" data-feedback-status="${item.resultId}"></div>
            </div>
        `;

        resultsList.appendChild(wrapper);
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
}
